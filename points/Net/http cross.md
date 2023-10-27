跨域解决方案：



## 1、设置Response Header

浏览器同源策略原因，不能跨域访问资源，是为了保障资源的安全，但是服务端是可以设置Response Headers，告诉浏览器该资源是可以跨域访问的。


解决方式：在web api所有Response中添加上相关的Response Headers，

```csharp
/// <summary>
/// 跨域管道、在所有的接口的返回结果中添加上跨域头信息
/// </summary>
public class CrocsHandle : DelegatingHandler
{
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        //response.Headers.Add("Access-Control-Allow-Origin", GlobalConfig.Origin);
        response.Headers.Add("Access-Control-Allow-Origin", GlobalConfig.Origin);

        response.Headers.Add("Access-Control-Allow-Credentials", "true");
        response.Headers.Add("Access-Control-Allow-Methods", "POST,GET");
        response.Headers.Add("Access-Control-Allow-Headers", "x-requested-with,content-type");
        return response;
    }
}
```
Origin是访问该接口的域，确保资源不被所有的网站访问到。

这样前端就能跨域访问了，但是如果前端要传递cookie的话，还需要配置一下

```js
$.ajax({
    type:'POST',
    url:'',
    xhrFields:{
      withCredentials:true,
    },
    crossDomain:true,
    data:{},
    dataType:json,
    success:function(respon){

    }
});
```

设置 `withCredentials:true,`


**在使用过程中还发现了下列问题**

问题描述：登录成功后，获取不到session信息。

分析：
在登录时手动再设置一个cookie test=test      
使用Filddler拦截请求，发现登录时Response有两个Set-Cookie头

```http
Set-Cookie: ASP.NET_SessionId=z2meli1rsnd0mqeopy0zbhhb; path=/; HttpOnly; SameSite=Lax
Set-Cookie: test=test
```
浏览器收到Set-Cookie后，会把Set-Cookie的内容写入到Cookie中，下次访问时，正常情况下会携带上这两个Cookie    
然后获取登录信息时，发现Request的头中只带有一个Cookie

```
Cookie: test=test
```
ASP.NET_SessionId没有传递到web api中，也就获取不到session     
发现这个现象后，就是要找一下为什么ASP.NET_SessionId没有传递。

之前response返回的Set-Cookie中，ASP.NET_SessionId比test多了两个信息，HttpOnly这个比较常见，还有一个时`SameSite=Lax`，查了下资料，发现正是这个属性限制了跨域cookie的传递。

SameSite=Strict：设置了该属性的cookie将不会添加到跨域请求里。     
SameSite=Lax：设置了该属性的cookie将不会添加到除了GET方法以外的跨域请求里。   

解决方式：登录时，不设置ASP.NET_SessionId的SameSite属性。

## 2、jsonp

主要是利用了标签src不受同源策略限制这个特性

## 代理

这个是后台代理，如使用nginx

nginx: `192.168.68.01`
web view: `192.168.68.02`
web api: `192.168.68.02`

浏览器访问nginx `192.168.68.01`,nginx把请求转发到web view `192.168.68.02`,如果是api的请求，就转发到web api`192.168.68.02`,这样的话，浏览器角度的话，只访问了`192.168.68.01`，是同源的，不会有跨域问题，而后台其实web view和web api是发布到了两个不同地方。