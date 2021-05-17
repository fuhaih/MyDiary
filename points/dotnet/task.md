# task理解

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

button事件进来后，ui线程处理，直到await Task.Factory.StartNew，新建的任务是在新线程里运行的，所以这里的thread3的线程id变了，由新的线程处理任务，ui线程释放，新线程会有等待操作，但是并不会阻塞ui线程，所以不会阻塞窗体。在任务3等待结束后，后续的内容继续是丢到ui线程的队列中由ui线程来执行，所以这里的thread4线程id还是ui线程的id

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

>TaskScheduler

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


>一个阻塞例子

```csharp
// My "top-level" method.
public void Button1_Click(...)
{
  var jsonTask = GetJsonAsync(...);
  textBox1.Text = jsonTask.Result;//主线程阻塞
}

// My "library" method.
public static async Task<JObject> GetJsonAsync(Uri uri)
{
  // (real-world code shouldn't use HttpClient in a using block; this is just example code)
  using (var client = new HttpClient())
  {
    var jsonString = await client.GetStringAsync(uri);
    // 这里开始是需要主线程来操作的，但是主线程又处在阻塞状态，所以就会发生死锁
    return JObject.Parse(jsonString);
  }
}

```

这里Button1事件中，使用了`jsonTask.Result`,主线程会阻塞，并一直持有着上下文对象，而jsonTask中，`await client.GetStringAsync(uri)`完成之后，后续操作是丢到主线程的队列中的，这样就产生了死锁，Button1事件在等待jsonTask任务完成，jsonTask又在等待Button1事件完成，释放上下文对象。


解决方法1:

```csharp
// My "top-level" method.
public async void Button1_Click(...)
{
  var json =await GetJsonAsync(...);
  textBox1.Text = json;
}

// My "library" method.
public static async Task<JObject> GetJsonAsync(Uri uri)
{
  // (real-world code shouldn't use HttpClient in a using block; this is just example code)
  using (var client = new HttpClient())
  {
    var jsonString = await client.GetStringAsync(uri);
    return JObject.Parse(jsonString);
  }
}

```

解决方法2:

```csharp
// My "top-level" method.
public void Button1_Click(...)
{
  var jsonTask = GetJsonAsync(...);
  textBox1.Text = jsonTask.Result;//主线程阻塞
}

// My "library" method.
public static async Task<JObject> GetJsonAsync(Uri uri)
{
  // (real-world code shouldn't use HttpClient in a using block; this is just example code)
  using (var client = new HttpClient())
  {
    var jsonString = await client.GetStringAsync(uri).ConfigureAwait(false);
    //这里由于ConfigureAwait(false)，所以不是在主线程中执行的
    return JObject.Parse(jsonString);
  }
}

```

前面介绍过`ConfigureAwait(false)`，添加了这个后，后续的操作就不是在主线程中进行的，所以就不会请求主线程的上下文，也就不会产生死锁。


但是并不建议在异步操作中使用`Result`进行阻塞，所以还是推荐解决方法1

>TaskCreationOptions


> Task.run 和Task.Factory.StartNew区别

Task.Factory.StartNew能比较精确的控制任务，例如需要设置TaskCreationOptions和TaskScheduler的时候，可以使用Task.Factory.StartNew来创建Task

Task.Run是简化了Task.Factory.StartNew的操作，

```csharp
// Task<int>
var t1 = Task.Run(async () =>
{
    return await Task.FromResult<int>(1);
});
// Task<Task<int>>
var t2 =  Task.Factory.StartNew(async()=> {
    return await Task.FromResult<int>(1);
});
// Task<int>
int t3 = await await t2;
```
`Task.Factory.StartNew`会用Task包装Action操作或者Func操作，这些操作如果返回的是Task类型，就会出现Task嵌套，如上例子中的t2类型是`Task<Task<int>>`，需要使用Unwrap方法来获取里面的实际的Task，而`Task.Run`会自动Unwrap



