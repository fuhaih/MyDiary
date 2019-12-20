>httpclient 携带cookie

```csharp

public static async Task<ResponData> Login(string userid, string pwd)
{
    ResponData result = null;
    cookie = null;
    int index = 0;
    while ((cookie == null || cookie.Count == 0)&& index<3)
    {
        result = await RetryLogin(userid, pwd);
        index++;
    }
    if (cookie.Count == 0) result.msg = null;
    return result;
}

public static async Task<ResponData> RetryLogin(string userid, string pwd)
{
    UserParameter parameter = new UserParameter
    {
        ID = userid,
        Password = pwd.MD5_32()
    };
    HttpClientHandler handler = GetHandler();
    string url = host + "/api/login/teacher";
    using (HttpClient client = new HttpClient(handler))
    {
        string data = JsonConvert.SerializeObject(parameter);
        var content = new StringContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var respon = await client.PostAsync(url, content);
        var json = await respon.Content.ReadAsStringAsync();
        cookie = handler.CookieContainer.GetCookies(new Uri(host));
        return JsonConvert.DeserializeObject<ResponData>(json);
    }
}


public static HttpClientHandler GetHandler()
{
    HttpClientHandler handler = new HttpClientHandler()
    {
        AllowAutoRedirect = true,
        ClientCertificateOptions = ClientCertificateOption.Automatic,
        UseCookies = true,
        CookieContainer = new CookieContainer()
        
    };
    if (cookie != null)
    {
        handler.CookieContainer.Add(cookie);
    }
    return handler;
}
```

登录的时候，通过`HttpClientHandler`获取cookie信息，存储起来，在每次访问时，再创建`HttpClientHandler`，把cookie放到`HttpClientHandler`中，传递给httpclient;

>httpclient 访问https


需要一个全局配置

```csharp
ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, error) =>
{
    return true;
};
```

然后还是通过`HttpClientHandler`来配置https访问，设置handler的属性

```csharp
HttpClientHandler handler = new HttpClientHandler()
{
    AllowAutoRedirect = true,
    ClientCertificateOptions = ClientCertificateOption.Automatic,
    UseCookies = true,
    CookieContainer = new CookieContainer()
    
};
```