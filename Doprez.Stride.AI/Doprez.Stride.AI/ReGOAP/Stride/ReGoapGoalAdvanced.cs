using System;
using System.Collections.Generic;
using System.Linq;
using ReGoap.Core;
using ReGoap.Planner;

// generic goal, should inherit this to do your own goal
namespace ReGoap.Stride
{
    public class ReGoapGoalAdvanced<T, W> : ReGoapGoal<T, W>
    {
        public float WarnDelay = 2f;
        private float warnCooldown;

        #region UnityFunctions
        public override void Update()
        {
            if (planner != null && !planner.IsPlanning() && Game.TargetElapsedTime.Milliseconds > warnCooldown)
            {
                warnCooldown = Game.TargetElapsedTime.Milliseconds + WarnDelay;
                var currentGoal = planner.GetCurrentGoal();
                var plannerPlan = currentGoal == null ? null : currentGoal.GetPlan();
                var equalsPlan = ReferenceEquals(plannerPlan, plan);
                var isGoalPossible = IsGoalPossible();
                // check if this goal is not active but CAN be activated
                //  or
                // if this goal is active but isn't anymore possible
                if ((!equalsPlan && isGoalPossible) || (equalsPlan && !isGoalPossible))
                    planner.GetCurrentAgent().WarnPossibleGoal(this);
            }
        }
        #endregion
    }
}