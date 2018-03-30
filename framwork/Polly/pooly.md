# [polly 服务容错处理库](https://github.com/App-vNext/Polly) 

## 重试（retry）
```csharp
//同步
Policy.Handle<Exception>()
.Retry(3,(ex,index)=> {
    Console.WriteLine(string.Format("第{0}次重试",index));
})
.Execute(() =>{
    //do something
    throw new Exception("test");
});
//异步
Policy.Handle<Exception>()
.RetryAsync(3,(ex,index)=> {
    Console.WriteLine(string.Format("第{0}次重试",index));
})
.ExecuteAsync(() =>{
    //do something
    throw new Exception("test");
}).ContinueWith(m=>
    Console.WriteLine(m.Exception.Message),
    TaskContinuationOptions.OnlyOnFaulted 
);
```
**注意：** 使用异步方法（如ExecuteAsync）时，Retry方法也要更改为异步的RetryAsync，否则会发生异常。

## 根据返回结果重试
```csharp
//重试，直到生成随机数为6为止
Random random = new Random();
var retry = Policy.HandleResult<bool>(n => n == false)
.RetryForeverAsync((ex, content) =>
{
    Console.WriteLine("重试");
});
var result =await retry.ExecuteAsync(() =>
{
    Task<bool> t = new Task<bool>(() =>
    {
        int i= random.Next(1,10);
        if (i % 6 == 0)
        {
            return true;
        }
        else {
            return false;
        }
    });
    t.Start();
    return t;
});
```

## 指定时间间隔重试
```csharp
//方式1
Policy.Handle<Exception>()
.WaitAndRetryAsync(new[]
{
    TimeSpan.FromSeconds(2),
    TimeSpan.FromSeconds(2),
    TimeSpan.FromSeconds(2)
}, (exception, timeSpan, retryCount, context) =>
{
    Console.WriteLine(string.Format("第{0}次重试", retryCount));
})
.ExecuteAsync(() =>
{
    //do something
    throw new Exception("test");
}).ContinueWith(m =>
    Console.WriteLine(m.Exception.Message),
    TaskContinuationOptions.OnlyOnFaulted
);
```