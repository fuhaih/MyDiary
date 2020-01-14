# 低版本迁移到高本版数据库

**注意：** 3.6之前和之后的存储引擎是不一样的。

mongodb有现成的工具进行数据转移，虽然3.6之后的版本的存储引擎和旧版本的不一样，但是通过mongodb的工具能把旧版本的数据库的数据正常地迁移到新版本上。

## 数据备份

```
>mongodump -h dbhost -d dbname -o dbdirectory
```

-h：
MongDB所在服务器地址，例如：127.0.0.1，当然也可以指定端口号：127.0.0.1:27017
-d：
需要备份的数据库实例，例如：test
-o：
备份的数据存放位置，例如：c:\data\dump，当然该目录需要提前建立，在备份完成后，系统自动在dump目录下建立一个test目录，这个目录里面存放该数据库实例的备份数据。

如果是要全部备份
```
>mongodump
```
会直接把mongodb所有的数据备份到当前目录的dump文件夹。目录的话看情况
命令行默认在`C:\Windows\System32`目录下   
如果是跳转到mongodb安装路径，那么就在该安装路径下。

## mongorestore 恢复数据
```
>mongorestore -h <hostname><:port> -d dbname <path>
```
--host <:port>, -h <:port>：
MongoDB所在服务器地址，默认为： localhost:27017
--db , -d ：
需要恢复的数据库实例，例如：test，当然这个名称也可以和备份时候的不一样，比如test2
--drop：
恢复的时候，先删除当前数据，然后恢复备份的数据。就是说，恢复后，备份后添加修改的数据都会被删除，慎用哦！
<path>：
mongorestore 最后的一个参数，设置备份数据所在位置，例如：c:\data\dump\test。
你不能同时指定 <path> 和 --dir 选项，--dir也可以设置备份目录。
--dir：
指定备份的目录
你不能同时指定 <path> 和 --dir 选项。

如果恢复到当前的系统的mongodb中，可以直接使用该命令

```
>mongorestore --dir [dump文件路径]
```

该命令会把dump文件路径下所有的数据恢复到当前mongodb数据库中。

## 记一次无缝升级
备份好数据后，先别停3.0版本的mongodb，依旧占用着27017端口    
先安装好3.6.4版本的mongodb，配置端口为27018  
命令启动3.6.4版本的mongodb
```
mongod -f D:\MongoDB\3.6\mongod.cfg
```
然后再另外启用一个命令行工具，进行数据恢复
```sh
mongorestore -h localhost:27018 --dir D:\MongoDB\dump
#可以先用命令 mongo localhost:27018 看看服务是否启动了
```
现在已经把数据恢复到新版本的mongodb了，只要把新版本的mongodb服务替换原来的服务就行了    

把3.6.4版本mongodb关闭（命令行中ctrl+c）  
把3.6.4版本mongodb的端口配置为27017   
通过reinstall命令来重新安装服务   

```
mongod -f D:\MongoDB\3.6\mongod.cfg --reinstall --serviceName "MongoDB"
```
这样，mongodb服务就重新安装了，用的是新版本的mongodb    
再通过命令重启服务
```
net start MongoDB
```

# windows中 mongodb数据备份定时计划

用到bat脚本和mongodb的数据备份命令 mongodump

```sh
mongodump -h dbhost -d dbname -o dbdirectory
```

## 创建bat脚本


新建一个mongodb backup.bat 文件

```bat
cd C:\Program Files\MongoDB\Server\3.6\bin
mongodump -o C:\MongoDB\backup
```
## 添加定时任务

使用windows自带的任务计划程序来创建定时任务

常规里 选择安全权限     
触发器里 选择触发规则       
操作里 选择bat脚本          
条件里 去掉一些条件限制     

# MongoDB数据存储空间清理

MongoDB由于存储机制问题，不会释放已经占用的磁盘空间，所以需要注意MongoDB的磁盘容量，当容量不够时要清理磁盘空间。

磁盘清理方式

>db.repairDatabase()

这个方式会让数据库在清理时禁止读写

>副本集

新增一个副本，数据会同步到新副本中，然后设置新副本为master，原来的库再删除重建。
这样的好处就是不影响正在mongodb的使用，不会因清理磁盘空间而影响到业务。
