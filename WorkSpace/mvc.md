# session
## session的生命周期
这个可以在mvc和iis上配置，默认是30分钟
## mvc中的session
浏览器在每次访问服务器的时候，都会生成一个新的sessionid，只有在session赋值的情况下，才会把sessionid存储到cookies中，返回到浏览器，然后浏览器每次访问的时候都会在cookies中携带这个sessionid，传递到服务器，服务器再根据sessionid获取session，在java中，用户可以手动根据sessionid来获取session，而在.net中，获取session这个过程不受用户控制。

如果需要在session没赋值的情况下，每次访问mvc获取到的sessionid都一样，可以注册下面两个事件。
```csharp
//在global写上这两个事件
void Session_Start(object sender, EventArgs e)
{
    //不是每次请求都调用
    //会话开始时执行
}

void Session_End(object sender, EventArgs e)
{
    //不是每次请求都调用
    //会话结束或过期时执行
    //不管在代码中显式的清空Session或者Session超时自动过期，此方法都将被调用
}
```

**注意**

* 正常后台跳转的时候，跳转到新链接也是会携带sessionid的，但是如果是ajax访问后台的时候，如果后台跳转了，跳转到新链接时没携带sessionid，具体还需要再仔细验证。 