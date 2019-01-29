# 配置
>两个服务器

    服务器1：192.168.69.143，数据库服务名 DESKTOP-1PTRBD6
    服务器2: 192.168.69.142，数据库服务名 DESKTOP-O64CJ7G

要使用发布订阅功能，需要在内网中使用（也就是同一子网内使用），但是可以通过别名来解决外网访问问题。

在进行发布订阅功能配置时，需要使用数据库服务名来登录

>使用服务器名来进行登录
```sql
<!--查看服务器名-->
sp_helpserver

```
|name	|network_name	|status	|id	|collation_name	|connect_timeout	|query_timeout
|--     |--             |--     |-- |--             |--                 |--               
|DESKTOP-1PTRBD6	|DESKTOP-1PTRBD6               	|rpc,rpc out,use remote collation	|0   	|NULL	|0	|0

>配置机器名和数据库服务名一致

查看两个名称是否一致
```sql
use master
go
select @@servername
select serverproperty('servername')
```
配置计算机名与服务器名一致
```sql
USE master
 GO
 if serverproperty('servername') <> @@servername  
begin  
       declare @server sysname  
       set   @server = @@servername  
       exec sp_dropserver @server = @server  
       set   @server = cast(serverproperty('servername') as sysname)  
       exec sp_addserver @server = @server , @local = 'LOCAL'  
```

DESKTOP-O64CJ7G

>添加别名

如果要在服务器1中使用数据库服务器名来登录服务器2的数据库，可以进行别名配置

打开计算机管理->服务和应用程序->SQL Server配置管理器->SQL Native Client->别名

别名：DESKTOP-O64CJ7G（这个需要和服务器2的数据库服务名一致）    
端口号：1433    
服务器：192.168.69.142（这个是服务器2的ip地址）     
协议:TCP/IP 



>SQL Server 代理“已禁用代理 XP”

这个是Agent XPs禁用了，启用一下就行了

```sql
sp_configure 'show advanced options', 1;
GO
RECONFIGURE;
GO
sp_configure 'Agent XPs', 1;
GO
```

```sql
RECONFIGURE
GO
```
重启服务

>新建发布

要发布的表需要有主键，否则无法进行发布


