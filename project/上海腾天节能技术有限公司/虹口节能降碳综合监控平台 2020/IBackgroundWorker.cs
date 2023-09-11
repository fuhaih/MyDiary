using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using static HongKouEnergyPlatform.BackgroundWork.WorkerAction;

namespace HongKouEnergyPlatform.BackgroundWork
{
    public interface IBackgroundWorker
    {
        Task StartExecuteAsync(CancellationToken cancellationToken);
        Task StopExecuteAsync();
        void Run(WorkerAction action);
        WorkerBuilder Handle(Action action);
        WorkerBuilder Handle<T>(Action<T> action);
        WorkerBuilder Handle(Delegate @delegate);
        WorkerBuilder Handle<T1, T2>(Action<T1, T2> action);
        WorkerBuilder Handle<T1, T2, T3>(Action<T1, T2, T3> action);
        WorkerBuilder Handle<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action);
        WorkerBuilder Handle<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action);
        WorkerBuilder Handle(Func<Task> action);
        WorkerBuilder Handle<T>(Func<T, Task> action);
        WorkerBuilder Handle<T1, T2>(Func<T1, T2, Task> action);
        WorkerBuilder Handle<T1, T2, T3>(Func<T1, T2, T3, Task> action);
        WorkerBuilder Handle<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> action);
        WorkerBuilder Handle<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Task> action);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Task> action);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, Task> action);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action);
        WorkerBuilder Handle<T>(Action<T> action, T parameter);
        WorkerBuilder Handle<T1, T>(Action<T1, T> action, T parameter);
        WorkerBuilder Handle<T1, T2, T>(Action<T1, T2, T> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T>(Action<T1, T2, T3, T> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T4, T>(Action<T1, T2, T3, T4, T> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T>(Action<T1, T2, T3, T4, T5, T> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6, T>(Action<T1, T2, T3, T4, T5, T6, T> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6, T7, T>(Action<T1, T2, T3, T4, T5, T6, T7, T> action, T parameter);
        WorkerBuilder Handle<T>(Func<T, Task> action, T parameter);
        WorkerBuilder Handle<T1, T>(Func<T1, T, Task> action, T parameter);
        WorkerBuilder Handle<T1, T2, T>(Func<T1, T2, T, Task> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T>(Func<T1, T2, T3, T, Task> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T4, T>(Func<T1, T2, T3, T4, T, Task> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T>(Func<T1, T2, T3, T4, T5, T, Task> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6, T>(Func<T1, T2, T3, T4, T5, T6, T, Task> action, T parameter);
        WorkerBuilder Handle<T1, T2, T3, T4, T5, T6, T7, T>(Func<T1, T2, T3, T4, T5, T6, T7, T, Task> action, T parameter);
    }
}
