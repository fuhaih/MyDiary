# 启动一个带有ui上下文的线程
>通过SynchronizationContext更新ui
```csharp
SynchronizationContext context = SynchronizationContext.Current;
Task reportProgressTask = Task.Factory.StartNew(() =>
{
    for (int i = 0; i < 10; i++)
    {
        Thread.Sleep(1000);
        context.Post(state =>
        {
            this.Text = state.ToString();
        }, i);
    }
});
await reportProgressTask;
```

>通过TaskScheduler更新ui
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

FromCurrentSynchronizationContext()方法会获取一个SynchronizationContextTaskScheduler对象，该对象的构造方法中会获取SynchronizationContext.Current存储在其属性中，所以TaskScheduler更新ui的原理也是通过SynchronizationContext来实现的。


# ExecutionContext 执行上下文

每一个线程都关联了一个执行上下文数据结构，执行上下文中包含了SynchronizationContext、SecurityContext、CallContext等上下文信息。当一个线程（初始线程）使用另一个线程（辅助线程）执行任务时，前者的执行上下文应该流向辅助线程，但是执行上下文中的SynchronizationContext不会流向辅助线程。


```csharp
ExecutionContext maincontext = ExecutionContext.Capture();
ThreadPool.QueueUserWorkItem(state =>
{
    ExecutionContext subcontext = ExecutionContext.Capture();
    ExecutionContext.Run(maincontext, _ =>
    {
        SynchronizationContext context = SynchronizationContext.Current;
        Console.WriteLine("MainContext SynchronizationContext {0}",context ==null?"Is Null":"Is Not Null");
    }, null);
    ExecutionContext.Run(subcontext, _ =>
    {
        SynchronizationContext context = SynchronizationContext.Current;
        Console.WriteLine("SubContext SynchronizationContext {0}", context == null ? "Is Null" : "Is Not Null");
    }, null);
});
//输出
MainContext SynchronizationContext Is Not Null
SubContext SynchronizationContext Is Null
```
也可以通过`ExecutionContext.SuppressFlow()`来禁止执行上下文流向辅助线程

# Task Async和Task
>task
```csharp
Task task = Task.Factory.StartNew(()=> {
    for (int i = 0; i < 5; i++)
    {
        Thread.Sleep(1000);
        Console.WriteLine("打印{0}",i);
    }
});
await task;
Console.WriteLine("task completed");

//输出结果
打印0
打印1
打印2
打印3
打印4
task completed
```
>task async
``` csharp
Task task = Task.Factory.StartNew(async ()=> {
    for (int i = 0; i < 5; i++)
    {
        await Task.Delay(1000);
        Console.WriteLine("打印{0}",i);
    }
});
await task;
Console.WriteLine("task completed");

//输出结果
task completed
打印0
打印1
打印2
打印3
打印4
```

上面例子可以看出，如果用异步委托来构造task，异步委托还没完成，该task就置为了完成状态，而异步委托会继续执行，感觉像是异步委托内又单独有另一个状态机来执行委托里的异步方法。

# await/async

>GetAwaiter()方法

```csharp
public class MyTask
{
    public TaskAwaiter GetAwaiter()
    {
        Task task = new Task(() => { });
        return task.GetAwaiter();
    }
}

MyTask mytask = new MyTask();
await mytask;
```



# 扩展方法
>WithCancellation

有些异步方法没有CancellationToken参数，可以通过CancellationToken、TaskCompletionSource和Task.WhenAny的巧妙的配合来实现这些异步操作的退出。
```csharp
public static async Task<T> WithCancellation1<T>(this Task<T> task, CancellationToken cancellationToken)
{
    var tcs = new TaskCompletionSource<bool>();
    using (cancellationToken.Register(() => tcs.TrySetResult(true)))
        if (task != await Task.WhenAny(task, tcs.Task))
            throw new OperationCanceledException(cancellationToken);
    return await task;
}
```

