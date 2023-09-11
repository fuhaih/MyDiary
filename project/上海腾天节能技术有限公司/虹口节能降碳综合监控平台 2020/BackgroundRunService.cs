using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using System.Collections.Concurrent;
using System.Reflection;
using HongKouEnergyPlatform.Service.Handles;
using HongKouEnergyPlatform.DataContent.Public;

namespace HongKouEnergyPlatform.BackgroundWork
{
    public class BackgroundRunService : IHostedService
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private IBackgroundWorker worker;
        ILogger<BackgroundRunService> logger;
        public BackgroundRunService(IBackgroundWorker worker, ILogger<BackgroundRunService> logger)
        {
            this.worker = worker;
            this.logger = logger;
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            
            _executingTask = worker.StartExecuteAsync(_stoppingCts.Token);
            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }
            worker.Handle<SimultaneousHandle>(async handle =>
            {
                await handle.CacheLoadRateYear();
            }).Run();
            worker.Handle<IPublicContent>(async content =>
            {
                await content.CacheWeatherNow();
            }).Run();
            // Otherwise it's running
            return Task.CompletedTask;
        }

        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                                                              cancellationToken));
            }
        }

        
    }
}
