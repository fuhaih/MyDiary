using NetTaste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HongKouEnergyPlatform.BackgroundWork
{
    public class WorkerAction
    {
        public Delegate HandleAction { get; set; }
        public Object HandleState { get; set; }
        public Delegate ContinuationAction { get; set; }
        public Object ContinuationState { get; set; }
        private WorkerAction(WorkerBuilder builder)
        {
            HandleAction = builder.HandleAction;
            HandleState = builder.HandleState;
            ContinuationAction = builder.ContinuationAction;
            ContinuationState = builder.ContinuationState;
        }
        public class WorkerBuilder
        {

            public Delegate HandleAction { get; set; }
            public Object HandleState { get; set; }
            public Delegate ContinuationAction { get; set; }
            public Object ContinuationState { get; set; }

            private IBackgroundWorker worker;
            public WorkerBuilder(Delegate handleAction,IBackgroundWorker worker)
            {
                this.HandleAction = handleAction;
                this.worker = worker;
            }

            public WorkerBuilder(Delegate handleAction, object handleState, IBackgroundWorker worker):this(handleAction,worker)
            {
                this.HandleState = handleState;
            }

            public void Run()
            {
                WorkerAction action = new WorkerAction(this);
                worker.Run(action);
            }

            private WorkerBuilder SetAction(Delegate action)
            {
                this.ContinuationAction = action;
                return this;
            }

            private WorkerBuilder SetActionState<T>(Delegate action, T state)
            {
                this.ContinuationAction = action;
                this.ContinuationState = state;
                return this;
            }

            public WorkerBuilder ContinueWith(Action<Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1>(Action<Task,T1> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1,T2>(Action<Task, T1, T2> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3>(Action<Task, T1, T2, T3> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4>(Action<Task, T1, T2, T3, T4> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5>(Action<Task, T1, T2, T3, T4, T5> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T6>(Action<Task, T1, T2, T3, T4, T5, T6> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T6, T7>(Action<Task, T1, T2, T3, T4, T5, T6, T7> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T>(Action<Task, T> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T>(Action<Task, T1, T> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T>(Action<Task, T1, T2, T> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T>(Action<Task, T1, T2, T3, T> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T>(Action<Task, T1, T2, T3, T4, T> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T>(Action<Task, T1, T2, T3, T4, T5, T> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T6, T>(Action<Task, T1, T2, T3, T4, T5, T6, T> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T6, T7, T>(Action<Task, T1, T2, T3, T4, T5, T6, T7, T> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith(Func<Task, Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1>(Func<Task, T1, Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2>(Func<Task, T1, T2, Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3>(Func<Task, T1, T2, T3, Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4>(Func<Task, T1, T2, T3, T4, Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5>(Func<Task, T1, T2, T3, T4, T5, Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T6>(Func<Task, T1, T2, T3, T4, T5, T6, Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T6, T7>(Func<Task, T1, T2, T3, T4, T5, T6, T7, Task> action) => SetAction(action);
            public WorkerBuilder ContinueWith<T>(Func<Task, T, Task> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T>(Func<Task, T1, T, Task> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T>(Func<Task, T1, T2, T, Task> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T>(Func<Task, T1, T2, T3, T, Task> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T>(Func<Task, T1, T2, T3, T4, T, Task> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T>(Func<Task, T1, T2, T3, T4, T5, T, Task> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T6, T>(Func<Task, T1, T2, T3, T4, T5, T6, T, Task> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWith<T1, T2, T3, T4, T5, T6, T7, T>(Func<Task, T1, T2, T3, T4, T5, T6, T7, T, Task> action, T state) => SetActionState(action, state);
            public WorkerBuilder ContinueWithHandleState(Action<Task, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1>(Action<Task, T1, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2>(Action<Task, T1, T2, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3>(Action<Task, T1, T2, T3, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3, T4>(Action<Task, T1, T2, T3, T4, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3, T4, T5>(Action<Task, T1, T2, T3, T4, T5, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3, T4, T5, T6>(Action<Task, T1, T2, T3, T4, T5, T6, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3, T4, T5, T6, T7>(Action<Task, T1, T2, T3, T4, T5, T6, T7, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState(Func<Task, object, Task> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1>(Func<Task, T1, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2>(Func<Task, T1, T2, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3>(Func<Task, T1, T2, T3, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3, T4>(Func<Task, T1, T2, T3, T4, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3, T4, T5>(Func<Task, T1, T2, T3, T4, T5, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3, T4, T5, T6>(Func<Task, T1, T2, T3, T4, T5, T6, Object> action) => SetActionState(action, HandleState);
            public WorkerBuilder ContinueWithHandleState<T1, T2, T3, T4, T5, T6, T7>(Func<Task, T1, T2, T3, T4, T5, T6, T7, Object> action) => SetActionState(action, HandleState);
        }
    }
}
