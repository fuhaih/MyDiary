# 启动一个带有ui上下文的线程
``` csharp
TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
Task reportProgressTask = Task.Factory.StartNew(async () =>
{
    for (int i = 0; i < 10; i++)
    {
        this.Text = i.ToString();
        await Task.Delay(1000);
    }
    // We are running on the UI thread here.
    // Update the UI with our progress.
}, CancellationToken.None, TaskCreationOptions.None,taskScheduler);

//DoSomething()
await reportProgressTask;
```
ui上下文不能长期占用，因为ui上下文同一时间只能一个线程使用，所以，如果非主线程长期占用ui线程，会造成主线程阻塞。


# SynchronizationContext 

