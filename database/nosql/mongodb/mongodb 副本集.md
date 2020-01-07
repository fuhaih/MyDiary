MongoDB副本集也就是MongoDB集群，通过主从备份来实现数据库的高可用。

* 版本 MongoDB 4.2.2
* 操作系统 windows

## 好处

> 高可用

当主节点崩溃的时候，选举算法会选举出一个从节点作为主节点，整个系统不会受到影响， 避免了数据库崩溃时整个系统都不能录入和读取。

> 提高吞吐

MongoDB的副本集中，只有主节点可以进行写入操作，所有节点都能进行读取操作，很适合少写多度的系统。

## 准备工作

> 安装MongoDB4.2.2

默认的安装路径为`C:\Program Files\MongoDB\Server\4.2`，命令在该路径的bin目录下，可以在环境变量里配置，也可以直接跳转到该路径进行命令操作。

>副本集配置

在D盘创建一个路径`D:\MongoDB`,路径下创建三个文件夹`cluster1`、`cluster2`、`cluster3`，每个文件夹创建一个mongod.cfg配置文件和data、log两个文件夹。
```yml
MongoDB
--cluster1
----data
----log
----mongod.cfg
--cluster2
----data
----log
----mongod.cfg
--cluster3
----data
----log
----mongod.cfg
```

cluster1(节点1)的mongod.cfg配置
```yml

# mongod.conf

# for documentation of all options, see:
#   http://docs.mongodb.org/manual/reference/configuration-options/

# Where and how to store data.
storage:
  dbPath: D:\MongoDB\cluster1\data
  journal:
    enabled: true
#  engine:
#  mmapv1:
#  wiredTiger:

# where to write logging data.
systemLog:
  destination: file
  logAppend: true
  path:  D:\MongoDB\cluster1\log\mongod.log

# network interfaces
net:
  port: 27018
  bindIp: 0.0.0.0

#processManagement:

#security:

#operationProfiling:

replication:
  replSetName: rs0
  enableMajorityReadConcern: true

#sharding:

## Enterprise-Only Options:

#auditLog:

#snmp:

```

集群名称为rs0，数据存储路径storage.dataPath和日志路径systemLog.path要根据不同节点进行配置。net.port端口号也是不同节点使用不同的端口号，这里三个节点的端口号分别为`27018`、`27019`、`27020`,在生产环境中可能不同节点在不同的ip中。



## 开始配置副本集

>启动节点

`mongod`命令启动cluster1的MongoDB实例

```sh
# 跳转到命令所在目录下
cd C:\Program Files\MongoDB\Server\4.2
mongod -f D:\MongoDB\cluster1\mongod.cfg
```

其他两个节点同理

>初始化副本集

`mongo`命令进入cluster1实例命令行模式

```
mongo localhost:27018
```
这里用cluster1作为主节点master

初始化副本集
```sh
config = {
  _id:"rs0",
  members:[
    {_id:0,host:"192.168.68.36:27018"},
    {_id:1,host:"192.168.68.36:27019"},
    {_id:2,host:"192.168.68.36:27020"}
  ]
}
rs.initiate(config)
```
查看副本集状态

```
rs.status()
```

`副本集的配置只需要在master节点配置就行了，配置完成后，进入其他节点的命令行模式也能查看到副本集状态`

**注意：**

`这里的host最好别用localhost，绑定广域网ip或者局域网ip，否则客户端使用MongoDB.Driver来使用时会出现异常；用MongoDB.Driver连接副本集时，会返回副本集所有的节点连接，如果配置host为localhost：27017，就会把localhost：27017这样的连接返回到客户端，客户端如果和MongoDB不在一个物理机上，就会访问不到MongoDB`

>添加或者修改节点

新增节点：
```sh
rs.add({_id:3,host:"192.168.68.36:27021"})
```
修改节点
```sh
var config = rs.config()
config.members[0].host = "192.168.68.36:27017"
rs.reconfig(config)
```

>从节点进行读取

进入从节点的命令行模式后，会发现数据库读取功能会异常

```sh
rs.slaveOk(true)
```

执行上面命令后，就可以进行正常的数据库读取操作了，但是增删改操作都是不允许的。

## 注意事项
>副本集节点数要是奇数

由于选举算法问题，副本集的节点数要是奇数，否则可能无法选出master节点，单个节点也是可以配置的。

**偶数节点时的解决方案：**

添加一个仲裁节点，仲裁节点不分享数据集，只是在投票无法选出master节点时，出来进行仲裁操作，并不会占用太多资源。

```sh
config = {
  _id:"rs0",
  members:[
    {_id:0,host:"192.168.68.36:27018"},
    {_id:1,host:"192.168.68.36:27019"},
    {_id:2,host:"192.168.68.36:27020", arbiterOnly: true}
  ]
}
rs.initiate(config)
```

或者

```sh
rs.addArb("192.168.68.36:27020")
```


