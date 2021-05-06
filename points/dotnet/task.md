
>TaskCreationOptions


>TaskScheduler

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

