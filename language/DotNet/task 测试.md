> 基本任务

```csharp
private async void button1_Click(object sender, EventArgs e)
{
    Console.WriteLine("thread1 id ",Thread.CurrentThread.ManagedThreadId.ToString());
    await ChangeText();
    //this.textBox1.Text = "has changed";
}

private async Task ChangeText()
{
    //SynchronizationContext.Current 为ui上下文
    Console.WriteLine("thread2 id ", Thread.CurrentThread.ManagedThreadId.ToString());
    await Task.Factory.StartNew(() =>
    {
        //SynchronizationContext.Current 为空
        Console.WriteLine("thread3 id ", Thread.CurrentThread.ManagedThreadId.ToString());
        Thread.Sleep(5000);      
    });
    Console.WriteLine("thread4 id ", Thread.CurrentThread.ManagedThreadId.ToString());
}
```

输出:

```s
thread1 id 10
thread2 id 10
thread3 id 6
thread4 id 10
```

button事件进来后，ui线程处理，直到await Task.Factory.StartNew，新建的任务是在新线程里运行的，所以这里的thread3的线程id变了，由新的线程处理任务，ui线程释放，新线程会有等待操作，但是并不会阻塞ui线程，所以不会阻塞窗体。在任务3等待结束后，后续的内容继续是由ui线程来执行，所以这里的thread4线程id还是ui线程的id

>ConfigureAwait(false)

```csharp
private async void button1_Click(object sender, EventArgs e)
{
    Console.WriteLine("thread1 id {0}",Thread.CurrentThread.ManagedThreadId.ToString());
    await ChangeText();
    //this.textBox1.Text = "has changed";
}

private async Task ChangeText()
{
    Console.WriteLine("thread2 id  {0}", Thread.CurrentThread.ManagedThreadId.ToString());
    var current = SynchronizationContext.Current;
    await Task.Factory.StartNew(() =>
    {
        
        Console.WriteLine("thread3 id  {0}", Thread.CurrentThread.ManagedThreadId.ToString());
        Thread.Sleep(5000);
    }).ConfigureAwait(false);
    Console.WriteLine("thread4 id  {0}", Thread.CurrentThread.ManagedThreadId.ToString());
}

```
输出
```s
thread1 id 10
thread2 id 10
thread3 id 6
thread4 id 6
```

在使用了ConfigureAwait(false)后，await结束后，并没有继续使用ui线程来执行后续代码，这里看到thread4的线程id并不是ui线程的线程id

>scheduler

```csharp
private async void button1_Click(object sender, EventArgs e)
{
    Console.WriteLine("thread1 id {0}",Thread.CurrentThread.ManagedThreadId.ToString());
    await ChangeText();
    //this.textBox1.Text = "has changed";
}

private async Task ChangeText()
{
    Console.WriteLine("thread2 id  {0}", Thread.CurrentThread.ManagedThreadId.ToString());
    var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
    await Task.Factory.StartNew(() =>
    {
        var context = SynchronizationContext.Current;
        Console.WriteLine("thread3 id  {0}", Thread.CurrentThread.ManagedThreadId.ToString());
        Thread.Sleep(5000);
    },CancellationToken.None,TaskCreationOptions.None,scheduler);
    Console.WriteLine("thread4 id  {0}", Thread.CurrentThread.ManagedThreadId.ToString());
}
```

输出
```s
thread1 id 10
thread2 id 10
thread3 id 10
thread4 id 10
```

`var scheduler = TaskScheduler.FromCurrentSynchronizationContext();`
从当前的SynchronizationContext中创建scheduler，当前的线程是ui线程，所以当前的上下文是ui上下文

Task.Factory.StartNew传入这个scheduler时，会直接使用当前的线程也就是ui线程来执行新创建的任务，所以thread3的thread id也是ui线程的thread id，这时候task里阻塞了5秒钟，ui线程也就会阻塞5秒钟，窗体也就会跟着阻塞。

>SynchronizationContext

```csharp
private async void button1_Click(object sender, EventArgs e)
{
    Console.WriteLine("thread1 id {0}",Thread.CurrentThread.ManagedThreadId.ToString());
    await ChangeText();
    //this.textBox1.Text = "has changed";
}

private async Task ChangeText()
{
    Console.WriteLine("thread2 id  {0}", Thread.CurrentThread.ManagedThreadId.ToString());
    var current = SynchronizationContext.Current;
    await Task.Factory.StartNew((state) =>
    {
        SynchronizationContext context = state as SynchronizationContext;
        Console.WriteLine("thread3 id ", Thread.CurrentThread.ManagedThreadId.ToString());
        Thread.Sleep(5000);
        context.Post(state1 =>
        {
            this.textBox1.Text = "change1";
        }, null);
    }, current);
    Console.WriteLine("thread4 id  {0}", Thread.CurrentThread.ManagedThreadId.ToString());
}
```
输出
```s
thread1 id 10
thread2 id 10
thread3 id 6
thread4 id 10
```

这个跟第一种一样情况，只有Task.Factory.StartNew中创建的任务是使用的其他线程来执行，但是使用SynchronizationContext也能对ui控件进行操作。