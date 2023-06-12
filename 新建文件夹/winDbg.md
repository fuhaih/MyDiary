# 安装
[官网](https://learn.microsoft.com/en-us/windows-hardware/drivers/debugger/debugger-download-tools)

总的来说两种安装方式

* 微软商店可以直接安装WinDbg Preview

*  Windows SDK 安装包安装，最后只选择Debugging Tools for Windows选项

[windows sdk](https://developer.microsoft.com/zh-cn/windows/downloads/windows-sdk/)

# 使用

## dump文件生成

托管在iis上的应用可以通过任务管理器来生成

任务管理器查找到对应的`IIS Worker Process` ,右键点击`创建转储文件`，指定文件路径，就会生成到指定路径下

现场可能不是`IIS Worker Process`,而是`w3wp.exe`

## 打开dump文件

file->open crash dump

## dump文件提炼出代码

`0:000>lm` 查看dump文件包含的模块

```cmd
start             end                 module name
00000234`59ba0000 00000234`59bdb000   iisres     (deferred)             
00000234`5ab50000 00000234`5ab76000   Serilog    (deferred)             
00000234`5ab80000 00000234`5ab8e000   Serilog_Sinks_Console   (deferred)             
00000234`5ab90000 00000234`5ab98000   Serilog_Sinks_Async   (deferred)             
00000234`5aeb0000 00000234`5aebe000   Serilog_Sinks_File   (deferred)             
00000234`5aec0000 00000234`5aeca000   Microsoft_Extensions_Configuration_Abstractions   (deferred)             
00000234`5aed0000 00000234`5aeda000   Autofac_Extensions_DependencyInjection   (deferred)             
00000234`5aee0000 00000234`5aeea000   Serilog_Extensions_Hosting   (deferred)             
00000234`5aef0000 00000234`5aefa000   Serilog_Extensions_Logging   (deferred)             
00000234`5af10000 00000234`5af64000   Autofac    (deferred)             
00000239`67930000 00000239`679c8000   SR_HDIS_HttpApi   (deferred)             
00000239`693e0000 00000239`6a630000   SR_HDIS_EF   (deferred)   
```

查找到`SR_HDIS_HttpApi`模块，这个是程序里一个命名空间

` !SaveModule SR_HDIS_HttpApi E:\MyCode\FromDumpFileToCode\Code\SR_HDIS_HttpApi.dll` 就可以直接把dll提出来，然后用反编译工具可以直接查看代码


## 加载sos

```
.loadby sos clr
```

加载sos和clr，如果报错的话会显示sos和clr的dll目录

windbg分32位版本和64位版本，可以根据不同版本来加载sos和clr

默认的加载路径好像是
```
C:\Windows\Microsoft.NET\Framework\v4.0.30319\sos.dll
C:\Windows\Microsoft.NET\Framework\v4.0.30319\clr.dll
```
如果报错，可能是打开了64位的windbg，加载64位的dll即可

```
.load C:\Windows\Microsoft.NET\Framework64\v4.0.30319\sos.dll
.load C:\Windows\Microsoft.NET\Framework64\v4.0.30319\clr.dll
```

## 加载sos等报错问题

这个可能是版本问题

有三个版本要注意

windbg版本（32/64）
dmp文件版本（生成dmp文件的系统版本）
应用程序的版本

这三个版本最好是一致的


## window卡死问题

> !clrstack

首先使用该指令