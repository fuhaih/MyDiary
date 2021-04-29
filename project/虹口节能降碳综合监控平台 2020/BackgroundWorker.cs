using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using NLog.Targets;
using NetTaste;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static HongKouEnergyPlatform.BackgroundWork.WorkerAction;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace HongKouEnergyPlatform.BackgroundWork
{
    /**
     * 使用方法

     */
    public class BackgroundWorker : IBackgroundWorker
    {
        private readonly SemaphoreSlim _slim;
        private BlockingCollection<WorkerAction> workerActions;
        private ILogger<BackgroundWorker> logger;
        private IServiceProvider serviceProvider;

        public BackgroundWorker(IServiceProvider serviceProvider, ILogger<BackgroundWorker> logger)
        {
            _slim = new SemaphoreSlim(10);
            workerActions = new BlockingCollection<WorkerAction>();
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        WorkerBuilder IBackgroundWorker.Handle(Action action)=>new WorkerBuilder(action,this);
        WorkerBuilder IBackgroundWorker.Handle<T>(Action<T> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2>(Action<T1, T2> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3>(Action<T1, T2, T3> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle(Delegate @delegate) => new WorkerBuilder(@delegate, this);
        WorkerBuilder IBackgroundWorker.Handle(Func<Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T>(Func<T, Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2>(Func<T1, T2, Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3>(Func<T1, T2, T3, Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action) => new WorkerBuilder(action, this);
        WorkerBuilder IBackgroundWorker.Handle<T>(Action<T> action, T parameter) => new WorkerBuilder(action,parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T>(Action<T1, T> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T>(Action<T1, T2, T> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T>(Action<T1, T2, T3, T> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T>(Action<T1, T2, T3, T4, T> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T>(Action<T1, T2, T3, T4, T5, T> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6, T>(Action<T1, T2, T3, T4, T5, T6, T> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6, T7, T>(Action<T1, T2, T3, T4, T5, T6, T7, T> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T>(Func<T, Task> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T>(Func<T1, T, Task> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T>(Func<T1, T2, T, Task> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T>(Func<T1, T2, T3, T, Task> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T>(Func<T1, T2, T3, T4, T, Task> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T>(Func<T1, T2, T3, T4, T5, T, Task> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6, T>(Func<T1, T2, T3, T4, T5, T6, T, Task> action, T parameter) => new WorkerBuilder(action, parameter, this);
        WorkerBuilder IBackgroundWorker.Handle<T1, T2, T3, T4, T5, T6, T7, T>(Func<T1, T2, T3, T4, T5, T6, T7, T, Task> action, T parameter) => new WorkerBuilder(action, parameter, this);
        void IBackgroundWorker.Run(WorkerAction action) => workerActions.Add(action);
        async Task IBackgroundWorker.StartExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield(); 
            foreach (var action in workerActions.GetConsumingEnumerable())
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _slim.WaitAsync(cancellationToken);

                Delegate handle = action.HandleAction;

                AsyncStateMachineAttribute attr = handle.Method.GetCustomAttribute<AsyncStateMachineAttribute>();

                //if (handle.Method.ReturnType == typeof(Task))
                //不能直接通过返回类型为Task就判断为异步方法，类似以下情况是可以发生的，但是下面这个方法并不是异步方法
                //public Task TestError()
                //{
                //    throw new Exception("test");
                //}
                if (attr !=null )
                {
                    List<Object> parameters = GetHanleParameter(action);
                    Task bgtask = ((Task)handle.DynamicInvoke(parameters.ToArray())).ContinueWith(ContinueWithAction, action);
                }
                else
                {
                    Task bgtask = Task.Factory.StartNew(waction =>
                    {
                        WorkerAction work = (WorkerAction)waction;
                        List<Object> parameters = GetHanleParameter(work);
                        work.HandleAction.DynamicInvoke(parameters.ToArray());
                    }, action).ContinueWith(ContinueWithAction, action);
                }
            }
        }

        private List<Object> GetHanleParameter(WorkerAction action)
        {
            Delegate handle = action.HandleAction;

            ParameterInfo[] parameterInfos = handle.Method.GetParameters();

            List<Object> parameters = new List<object>();

            Type type = action.HandleState==null? null :action.HandleState.GetType() ;
            // 使用scoped，确保注入DbContext的时候不会出现异常
            using (var scoped = serviceProvider.CreateScope())
            {
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    if (i == parameterInfos.Length - 1 && type != null && parameterInfos[i].ParameterType == type)
                    {
                        parameters.Add(action.HandleState);
                        continue;
                    }
                    Object pr = scoped.ServiceProvider.GetService(parameterInfos[i].ParameterType);
                    parameters.Add(pr);
                }
            }

            //foreach (var item in parameterInfos)
            //{
            //    Object pr = serviceProvider.GetService(item.ParameterType);
            //    parameters.Add(pr);
            //}
            return parameters;
        }

        private List<Object> GetContinueParameter(Task task, WorkerAction action)
        {
            Delegate continuation = action.ContinuationAction;

            ParameterInfo[] parameterInfos = continuation.Method.GetParameters();

            List<Object> parameters = new List<object>();

            Type type = action.ContinuationState == null ? null : action.ContinuationState.GetType();
            using (var scoped = serviceProvider.CreateScope()) {
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    if (i == 0 && parameterInfos[i].ParameterType == typeof(Task))
                    {
                        parameters.Add(task);
                        continue;
                    }
                    if (i == parameterInfos.Length - 1 && type != null && parameterInfos[i].ParameterType == type)
                    {
                        parameters.Add(action.ContinuationState);
                        continue;
                    }
                    Object pr = scoped.ServiceProvider.GetService(parameterInfos[i].ParameterType);
                    parameters.Add(pr);
                }
            }


            //foreach (var item in parameterInfos)
            //{
            //    Object pr = serviceProvider.GetService(item.ParameterType);
            //    parameters.Add(pr);
            //}
            return parameters;
        }

        private async Task ContinueCallback(Task task, WorkerAction action)
        {
            if (action.ContinuationAction == null)
            {
                if (task.IsFaulted)
                {
                    task.Exception.Handle(ex =>
                    {
                        logger.LogWarning(ex, "后台线程异常");
                        return true;
                    });
                }
                return;
            }
            AsyncStateMachineAttribute attr = action.ContinuationAction.Method.GetCustomAttribute<AsyncStateMachineAttribute>();
            if (attr != null)
            {
                List<Object> parameters = GetContinueParameter(task, action);
                await(Task)action.ContinuationAction.DynamicInvoke(parameters.ToArray());
            }
            else
            {
                List<Object> parameters = GetContinueParameter(task, action);
                action.ContinuationAction.DynamicInvoke(parameters.ToArray());
            }
        }

        private async Task ContinueWithAction(Task task, Object cstate)
        {
                WorkerAction work = (WorkerAction)cstate;
                try
                {
                    await ContinueCallback(task, work);
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    _slim.Release();
                }
        }
                
        async Task IBackgroundWorker.StopExecuteAsync()
        {
            await Task.Yield();
            workerActions.CompleteAdding();
        }

        private class Builder
        {
            public Builder()
            { 
                
            }
        }
    }
}
