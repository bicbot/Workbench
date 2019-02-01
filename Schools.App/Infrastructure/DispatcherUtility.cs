using System;
using System.Threading;
using System.Windows.Threading;

namespace SchoolsApp.Infrastructure
{
    /// <summary>
    ///     Execute a long running operation and wait for it to finish
    ///     without blocking the windows messaging pump
    /// </summary>
    public static class DispatcherUtility
    {
        public static T ExecuteWait<T>(Func<T> action)
        {
            var result = default(T);

            void ActionDelegate() => result = action();

            ExecuteWait((ActionDelegate) ActionDelegate);

            return result;
        }

        public static void ExecuteWait(Action action)
        {
            void ActionDelegate() => action();

            ExecuteWait((ActionDelegate) ActionDelegate);
        }

        private static void ExecuteWait(ActionDelegate actionDelegate)
        {
            var waitFrame = new DispatcherFrame();

            // Use callback to "pop" dispatcher frame
            var op = actionDelegate.BeginInvoke(dummy => waitFrame.Continue = false, null);

            // this method will block here but window messages are pumped
            Dispatcher.PushFrame(waitFrame);

            // this method may throw if the action threw. caller's responsibility to handle.
            actionDelegate.EndInvoke(op);
        }

        /// <summary>
        ///     Stop execution for a specific amount of time without blocking the UI
        /// </summary>
        /// <param name="interval">The time to wait in milliseconds</param>
        public static void Wait(int interval)
        {
            void ActionDelegate() => Thread.Sleep(interval);
            ExecuteWait((ActionDelegate) ActionDelegate);
        }

        #region Nested type: ActionDelegate

        private delegate void ActionDelegate();

        #endregion
    }
}
