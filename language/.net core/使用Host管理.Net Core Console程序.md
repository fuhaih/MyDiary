## 简单使用host
>添加引用

```csharp
Microsoft.Extensions.Configuration
Microsoft.Extensions.DependencyInjection
Microsoft.Extensions.Hosting
Microsoft.Extensions.Logging
```

>编写 HostedService

```csharp
public class CustomServer : IHostedService
{
    ILogger<CustomServer> _logger;
    private IConfiguration _config;
    private IServiceProvider _serviceProvider;
    public CustomServer(ILogger<CustomServer> logger, IConfiguration config, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider;
    }
    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        _logger.LogInformation("服务启动");
    }

    async Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        _logger.LogInformation("服务关闭");
    }
}
```

>使用Host

修改Program.cs

```csharp
class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<CustomServer>();
        });
}
```

## 使用json配置

>编写配置文件

```json
{
  "database":{
    "connectStr": "Server=192.168.68.104; User Id=SYSDBA; PWD=tt@52415188"
  }
}
```

>配置

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        //config.AddInMemoryCollection(arrayDict);
        config.AddJsonFile("hostingconfig.json", optional: true, reloadOnChange: true);
        //config.AddCommandLine(args);
    })
```

>使用配置

使用DI容器获取`IConfiguration`对象config

```csharp
//获取字符串
var connectSection = config.GetSection("database:connectStr");
string connectStr = connectSection.Value;

//绑定到对象
var databaseSection = config.GetSection("database");
var dbinfo = databaseSection.Get<DataBaseConfig>();
```

## 使用Nlog

>引用类库

```csharp
Nlog.Web.AspNetCore
```

>编写Nlog配置文件

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <targets>
    <target name="logfile" xsi:type="File" fileName="${basedir}/logs/${shortdate}_logfile.txt" maxArchiveFiles="10"  archiveAboveSize="10485760" layout="${longdate}|${level:uppercase=true}|${logger}${newline}${message}${newline}${exception:format=toString,Data}"/>
    <target name="Tracefile" xsi:type="File" fileName="${basedir}/Traces/${shortdate}_logfile.txt" maxArchiveFiles="10"  archiveAboveSize="10485760" layout="${longdate}|${level:uppercase=true}|${logger}${newline}${message}${newline}${exception:format=toString,Data}" />
    <target name="logconsole" xsi:type="ColoredConsole" useDefaultRowHighlightingRules="true" layout="${longdate}|${level:uppercase=true}|${logger}${newline}${message}${newline}${exception:format=toString,Data} " />
    <target xsi:type="Network"
      name="logstash_apiinsight"
      keepConnection="false"
      layout="[subscriber]${longdate}|${level:uppercase=true}|${message}|${exception}" address ="tcp://192.168.68.35:5044">
    </target>
  </targets>

  <rules>
    <logger name="*" minlevel="Info" writeTo="logconsole" />
    <logger name="*" minlevel="Info" writeTo="logfile" />
    <logger name="*" minlevel="Trace" maxlevel="Trace" writeTo="Tracefile" />
    <logger name="*" minlevel="Info" writeTo="logstash_apiinsight"/>
  </rules>
</nlog>
```

>配置Nlog

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
    .ConfigureLogging((logging) => {
        logging.ClearProviders();//清除Providers
        logging.AddNLog("NLog.config");
    }).UseNLog();
```

>使用Nlog

.Net Core中与日志相关的对象有`Logger`、`LoggerProvider`、`LoggerFactory`

在配置了Nlog后，会使用Nlog来替换原生的日志，不过使用方式上是一样的。

使用DI容器获取ILogger<T>对象_logger
```csharp
_log.LogInformation("服务启动");
```