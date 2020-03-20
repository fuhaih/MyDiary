# linux 发布

## console程序
> 程序

名称：SHEnergyServer

发布位置：/usr/local/console/SHEnergyServer/publish/

启动程序命令：dotnet SHEnergyServer.dll

> 编写服务文件

```
cd /usr/lib/systemd/system
vim SHEnergyServer.service
```

```
[Unit]
Description=SHEnergyServer2
[Service]
Type=simple
WorkingDirectory=/usr/local/console/SHEnergyServer/publish/
ExecStart=/usr/local/bin/dotnet SHEnergyServer.dll
Restart=always
[Install]
WantedBy=multi-user.target
```

`WorkingDirectory`为程序的发布路径

`ExecStart`的dotnet命令需要写完整路径。

> 启动程序

```sh
# 重载系统服务
systemctl daemon-reload
# 启动程序
systemctl start SHEnergyServer.service
# 查看状态
systemctl status SHEnergyServer.service
# 开机启动
systemctl enable SHEnergyServer.service
```


## web程序

[Kestrel](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-3.1)

web程序的部署和console是类似的，不过需要配置web监听地址和端口,可以使用以下几种方式来设置地址端口，比较推荐第三种方式来配置Kestrel，并且调试时可以使用Kestrel来代替IIS Express

* 1、.NET Core CLI

```
dotnet SHEnergyWeb.dll --urls "http://*:8080"
```

* 2、配置

通过`ConfigureKestrel`进行配置 (这个目前没有配置成功)
```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(config => { 
            
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.ConfigureKestrel(listionOption=> {
                //配置Kestrel
            }).UseStartup<Startup>();
        })
        .ConfigureLogging((logging) => {
            logging.ClearProviders();//清除Providers
            logging.AddNLog("NLog.config");
        }).UseNLog();
```

* 3、配置kestrel

在`appsettings.json`中进行如下配置

```json
{
  "Kestrel": {
    "Limits":{
      "MaxConcurrentConnections": 100,
      "MaxConcurrentUpgradedConnections": 100
    },
    "DisableStringReuse": true,
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5000"
      },
      "HttpsInlineCertFile": {
        "Url": "https://localhost:5001",
        "Certificate": {
          "Path": "<path to .pfx file>",
          "Password": "<certificate password>"
        }
      },
      "HttpsInlineCertStore": {
        "Url": "https://localhost:5002",
        "Certificate": {
          "Subject": "<subject; required>",
          "Store": "<certificate store; required>",
          "Location": "<location; defaults to CurrentUser>",
          "AllowInvalid": "<true or false; defaults to false>"
        }
      },
      "HttpsDefaultCert": {
        "Url": "https://localhost:5003"
      },
      "Https": {
        "Url": "https://*:5004",
        "Certificate": {
          "Path": "<path to .pfx file>",
          "Password": "<certificate password>"
        }
      }
    },
    "Certificates": {
      "Default": {
        "Path": "<path to .pfx file>",
        "Password": "<certificate password>"
      }
    }
  }
}

```

然后在修改`Startup`的`ConfigureServices`

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews().AddJsonOptions(options=> {
        options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
    });
    //设置kestrel
    services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
}
```

## 关于程序配置文件路径问题

## 开放端口号

```sh
# 添加端口
firewall-cmd --permanent --add-port=8081/tcp
# 查看开放端口
firewall-cmd --list-all
# 重新加载配置
firewall-cml --reload 
```

# windows 发布

## web程序 

[.Net Core](https://dotnet.microsoft.com/download/dotnet-core)      

从上面链接中找到对应版本的 .Net Core Hosting安装程序，安装后就能在iis中看到

## console程序

nuget 安装`Microsoft.Extensions.Hosting.WindowsServices`

>修改Program.cs文件
```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
    .UseWindowsService();

```

>通过命令安装(以管理员身份运行)

```sh
sc create SHEnergyServer2 binpath=D:\punlish\SHEnergyServer2\SHEnergyServer2.exe
```

# docker发布


