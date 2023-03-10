#define DEBUG
using System;

namespace ReGoap.Utilities
{
    public static class ReGoapLogger
    {
    private class GenericTraceListener : IListener
    {
        public void Write(string message)
        {
            Write(message, "");
        }

        public void Write(string message, string category)
        {
            Console.WriteLine(message);
        }
    }

        [Flags]
        public enum DebugLevel
        {
            None, ErrorsOnly, WarningsOnly, Full
        }
        public static DebugLevel Level = DebugLevel.Full;
        public static bool RunOnlyOnMainThread = true;

        private static readonly IListener listener;

        private static readonly int mainThreadId;

        static ReGoapLogger()
        {
            mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            listener = new GenericTraceListener();
        }

        private static bool InMainThread()
        {
            return !RunOnlyOnMainThread || System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId;
        }

        public static void Log(string message)
        {
            if (Level != DebugLevel.Full || !InMainThread()) return;
            listener.Write(message);
        }

        public static void LogWarning(string message)
        {
            if (Level < DebugLevel.WarningsOnly || !InMainThread()) return;
            listener.Write(message, "warning");
        }

        public static void LogError(string message)
        {
            if (Level < DebugLevel.ErrorsOnly || !InMainThread()) return;
            listener.Write(message, "error");
        }
    }

    internal interface IListener
    {
        void Write(string text);
        void Write(string text, string category);
    }
}