﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using ReGoap.Core;
using ReGoap.Planner;
using ReGoap.Utilities;
using Stride.Engine;
using Stride.Core.Annotations;
using Stride.Core;

namespace ReGoap.Stride
{ // every thread runs on one of these classes
    public class ReGoapPlannerThread<T, W>
    {
        private readonly ReGoapPlanner<T, W> planner;
        public static ConcurrentQueue<ReGoapPlanWork<T, W>> WorksQueue;
        private bool isRunning = true;
        private readonly Action<ReGoapPlannerThread<T, W>, ReGoapPlanWork<T, W>, IReGoapGoal<T, W>> onDonePlan;

        public ReGoapPlannerThread(ReGoapPlannerSettings plannerSettings, Action<ReGoapPlannerThread<T, W>, ReGoapPlanWork<T, W>, IReGoapGoal<T, W>> onDonePlan)
        {
            planner = new ReGoapPlanner<T, W>(plannerSettings);
            this.onDonePlan = onDonePlan;
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void MainLoop()
        {
            while (isRunning)
            {
                CheckWorkers();
                Thread.Sleep(0);
            }
        }

        public void CheckWorkers()
        {
            if (WorksQueue.TryDequeue(out ReGoapPlanWork<T, W> checkWork)) {
                var work = checkWork;
                planner.Plan(work.Agent, work.BlacklistGoal, work.Actions,
                            (newGoal) => onDonePlan(this, work, newGoal));
            }
        }
    }

// behaviour that should be added once (and only once) to a gameobject in your unity's scene
    public class ReGoapPlannerManager<T, W> : SyncScript
    {
        public static ReGoapPlannerManager<T, W> Instance;

        public bool MultiThread;
        [DataMember("Used only if MultiThread is set to true.")]
		[DataMemberRange(1, 128, 1, 4, 0)]
		public int ThreadsCount = 1;
        private ReGoapPlannerThread<T, W>[] planners;

        private List<ReGoapPlanWork<T, W>> doneWorks;
        private Thread[] threads;

        public ReGoapPlannerSettings PlannerSettings;

        public ReGoapLogger.DebugLevel LogLevel = ReGoapLogger.DebugLevel.Full;

        public int NodeWarmupCount = 1000;
        public int StatesWarmupCount = 10000;

        public override void Start()
        {
            ReGoapNode<T, W>.Warmup(NodeWarmupCount);
            ReGoapState<T, W>.Warmup(StatesWarmupCount);

            ReGoapLogger.Level = LogLevel;

            Instance = this;

            doneWorks = new List<ReGoapPlanWork<T, W>>();
            ReGoapPlannerThread<T, W>.WorksQueue = new ConcurrentQueue<ReGoapPlanWork<T, W>>();
            planners = new ReGoapPlannerThread<T, W>[ThreadsCount];
            threads = new Thread[ThreadsCount];

            if (MultiThread)
            {
                ReGoapLogger.Log(String.Format("[GoapPlannerManager] Running in multi-thread mode ({0} threads).", ThreadsCount));
                for (int i = 0; i < ThreadsCount; i++)
                {
                    planners[i] = new ReGoapPlannerThread<T, W>(PlannerSettings, OnDonePlan);
                    var thread = new Thread(planners[i].MainLoop);
                    thread.Start();
                    threads[i] = thread;
                }
            } // no threads run
            else
            {
                ReGoapLogger.Log("[GoapPlannerManager] Running in single-thread mode.");
                planners[0] = new ReGoapPlannerThread<T, W>(PlannerSettings, OnDonePlan);
            }
        }

        void OnDestroy()
        {
            foreach (var planner in planners)
            {
                if (planner != null)
                    planner.Stop();
            }
            // should wait here?
            foreach (var thread in threads)
            {
                if (thread != null)
                    thread.Abort();
            }
        }

        public override void Update()
        {
            ReGoapLogger.Level = LogLevel;
            if (doneWorks.Count > 0)
            {
                lock (doneWorks)
                {
                    foreach (var work in doneWorks)
                    {
                        work.Callback(work.NewGoal);
                    }
                    doneWorks.Clear();
                }
            }
            if (!MultiThread)
            {
                planners[0].CheckWorkers();
            }
        }

        // called in another thread
        private void OnDonePlan(ReGoapPlannerThread<T, W> plannerThread, ReGoapPlanWork<T, W> work, IReGoapGoal<T, W> newGoal)
        {
            work.NewGoal = newGoal;
            lock (doneWorks)
            {
                doneWorks.Add(work);
            }
            if (work.NewGoal != null && ReGoapLogger.Level == ReGoapLogger.DebugLevel.Full)
            {
                ReGoapLogger.Log("[GoapPlannerManager] Done calculating plan, actions list:");
                var i = 0;
                foreach (var action in work.NewGoal.GetPlan())
                {
                    ReGoapLogger.Log(string.Format("{0}: {1}", i++, action.Action));
                }
            }
        }

        public ReGoapPlanWork<T, W> Plan(IReGoapAgent<T, W> agent, IReGoapGoal<T, W> blacklistGoal, Queue<ReGoapActionState<T, W>> currentPlan, Action<IReGoapGoal<T, W>> callback)
        {
            var work = new ReGoapPlanWork<T, W>(agent, blacklistGoal, currentPlan, callback);
            lock (ReGoapPlannerThread<T, W>.WorksQueue)
            {
                ReGoapPlannerThread<T, W>.WorksQueue.Enqueue(work);
            }
            return work;
        }
    }

    public struct ReGoapPlanWork<T, W>
    {
        public readonly IReGoapAgent<T, W> Agent;
        public readonly IReGoapGoal<T, W> BlacklistGoal;
        public readonly Queue<ReGoapActionState<T, W>> Actions;
        public readonly Action<IReGoapGoal<T, W>> Callback;

        public IReGoapGoal<T, W> NewGoal;

        public ReGoapPlanWork(IReGoapAgent<T, W> agent, IReGoapGoal<T, W> blacklistGoal, Queue<ReGoapActionState<T, W>> actions, Action<IReGoapGoal<T, W>> callback) : this()
        {
            Agent = agent;
            BlacklistGoal = blacklistGoal;
            Actions = actions;
            Callback = callback;
        }
    }
}
