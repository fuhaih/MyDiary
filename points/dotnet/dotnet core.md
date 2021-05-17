> launchSettings.json

这个文件在项目的Properties文件夹中，主要是用来配置调试信息的，调试的时候默认启动的url和路径

```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:6045",
      //"sslPort": 44346
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "weatherforecast",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "TTBEMSOpenApi": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "weatherforecast",
      "applicationUrl": "http://localhost:5001",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

```

这里配置了`"applicationUrl": "http://localhost:5001"`和`"launchUrl": "weatherforecast"`，在调试时会直接使用浏览器打开`http://localhost:5001/weatherforecast`,如果这里配置的url和服务的`Endpoints`不一致，那么僵无法打开浏览器

>AddControllers和AddControllersWithViews
```csharp
services.AddControllers();
services.AddControllersWithViews()
```

>MapControllers、 MapControllerRoute


MapControllers是用来绑定[Route][HttpGet]等特性配置的路由

MapControllerRoute是用来绑定控制器路由

如果不需要用到控制器视图这些内容，纯粹使用web api，就可以把MapControllerRoute注释掉
```csharp
app.UseEndpoints(endpoints =>
{
    // 这个是特性控制器路由，[Route][HttpGet]等特性配置路由，一般就是指webapi的路由
    endpoints.MapControllers();
    // 这个是传统控制器路由
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
```

>MapGet
