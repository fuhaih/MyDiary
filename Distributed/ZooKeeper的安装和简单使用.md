# 安装
## 下载安装zookeeper

>下载到本地
```sh
# 如果没有wget，先安装wget
# yum -y install wget
cd /usr/local/src
#当前路径为/usr/local/src
wget http://mirrors.hust.edu.cn/apache/zookeeper/zookeeper-3.4.13/zookeeper-3.4.13.tar.gz

```

>解压
```sh
#当前路径为/usr/local/src
tar -xzvf zookeeper-3.4.13.tar.gz
```

>创建安装目录，并移动到该目录中
```sh
#当前路径为/usr/local/src
#创建的安装目录为/usr/local/zookeeper
mkdir -p ../zookeeper
mv zookeeper-3.4.11/ ../zookeeper
```
>创建配置
```sh
#当前目录为/usr/local/src
cd ../zookeeper/zookeeper-3.4.11/conf
#当前目录为/usr/local/zookeeper/zookeeper-3.4.11/conf
cp zoo_sample.cfg zoo.cfg
```
>修改配置
```sh
#当前目录为/usr/local/zookeeper/zookeeper-3.4.11/conf
#创建数据目录
mkdir -p /var/lib/zookeeper
vim zoo.cfg
# 修改 dataDir=/var/lib/zookeeper
```

## 启动服务
>启动服务
```sh
# 当前目录/usr/local/zookeeper/zookeeper-3.4.11/conf
cd ..
# 当前目录/usr/local/zookeeper/zookeeper-3.4.11
./bin/zkServer.sh start
```
>查看服务状态
```sh
# 当前目录/usr/local/zookeeper/zookeeper-3.4.11
./bin/zkServer.sh status
```

# c#中使用
## 安装驱动

zookeeper的 .NET 驱动是 `ZooKeeper.Net`,可以通过NuGet进行安装

## 使用
```csharp
public class Watcher : IWatcher
{
    public void Process(WatchedEvent @event)
    {
        if (@event.Type == EventType.NodeDataChanged)
        {
            Console.WriteLine(@event.Path);
        }
    }
}
```

```csharp
//构造函数中的sessiontimeout是用来设置会话超时的
//客户端会不断发送心跳包给zookeeper，更新会话超时，如果超过sessiontimeout还没有接受到心跳，表示已经断开连接
//默认的Session超时时间是在2 * tickTime ~ 20 * tickTime，tickTime可以在zookeeper的配置文件中进行配置，
ZooKeeper zk = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 0, 50000), new Watcher());
//判断root节点是否存在，不存在就创建一个
var rootstate = zk.Exists("/root", true);
if (rootstate == null)
{
    //判断然后创建不是原子操作，在节点已经存在时创建会报错。
    try{
        zk.Create("/root", "root".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
    }catch{}
    
}
//在root下面创建一个node1 节点,数据为node1,不进行ACL权限控制，节点为永久性的 
string node1 = zk.Create("/root/node1", "node1".GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.Persistent);
string node2 = zk.Create("/root/node2", "node1".GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.Persistent);
//取得/root节点下的子节点名称,返回List<String>
zk.GetChildren("/root", true);
//取得/root/node1 节点下的数据,返回byte[] 
zk.GetData("/root/node1", true, null);
zk.GetData("/root/node2", true, null);
//修改节点/root/node1下的数据，第三个参数为版本，如果是-1，那会无视被修改的数据版本，直接改掉
zk.SetData("/root/node1", "node1modify".GetBytes(), -1);
//删除/root/node1这个节点，第二个参数为版本，－1的话直接删除，无视版本 
zk.Delete("/root/node1", -1);
```
## CreateMode
>Persistent

无序的持久节点
>PersistentSequential

有序的持久节点
>Ephemeral

无序的临时节点
>EphemeralSequential

有序的临时节点

>持久性节点和临时性节点比较

持久性节点和临时节点的区别在于，当客户端与服务器断开连接时，临时节点会被删除，持久性节点还在。  
临时节点比较适合用来作为锁，当客户端与zookeeper断开连接时，节点删除，相当于解锁，这样就不会因客户端网络等原因而长期占有锁。

>无序节点和有序节点

无序节点在创建节点时：
```csharp
string nodename1=zk.Create("/root/node", "node".GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.Persistent);
string nodename2=zk.Create("/root/node", "node".GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.Persistent);
```
上述代码第二行会异常，因为已经在root下有一个node节点了    
有序节点在创建节点时会进行编号：
```csharp
string nodename1=zk.Create("/root/node", "node".GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.Persistent);
string nodename2=zk.Create("/root/node", "node".GetBytes(), Ids.OPEN_ACL_UNSAFE,CreateMode.Persistent);
zk.GetData(nodename2, true, null);
//下面代码异常
zk.GetData("/root/node", true, null);

//nodename1=/root/node00001
//nodename2=/root/node00002
```
所以上诉代码不会异常，但是在获取节点的时候需要传入的是Create返回来的节点名称，删除时也一样

## ConnectionLossException
这个异常发生的可能原因有两个    
1、服务器异常，这个需要查看服务器   
2、在创建zookeeper对象的时候，zookeeper对象是异步连接服务器的，所以可能发生zookeeper还没连接上服务器就开始发送指令（调用Exits等方法）的情况，这时候就报错

>针对情况2的解决方案

重写Watcher
```csharp
public class Watcher : IWatcher
{
    private AutoResetEvent ConnectedEven;
    public void Process(WatchedEvent @event)
    {
        if (@event.State == KeeperState.SyncConnected)
        {
            ConnectedEven.Set();
        }
    }

    public Watcher(AutoResetEvent connectedEven)
    {
        ConnectedEven = connectedEven;
    }
}
```
AutoResetEvent阻塞线程，直到zk连接成功或者超时
```csharp
AutoResetEvent autoResetEvent = new AutoResetEvent(false);
Watcher watcher = new Watcher(autoResetEvent);
ZooKeeper zk = new ZooKeeper(district.Zookeeper.Host, new TimeSpan(0, 0, 0, district.Zookeeper.SessionTimeout), watcher);
bool waitzk = autoResetEvent.WaitOne(15000);
if (!waitzk) 
{
    //超时未连接成功
}
var rootstate = zk.Exists("/root", true);
```

## 分布式锁
NuGet 安装ZooKeeper.Net.Recipes
```csharp
ZooKeeper zk = new ZooKeeper("47.106.162.159:2181", new TimeSpan(0, 0, 0, 5000), new Watcher());
AutoResetEvent autoResetEvent = new AutoResetEvent(false);
WriteLock writelock = new WriteLock(zk, "/testroot/lock", Ids.OPEN_ACL_UNSAFE);
writelock.LockAcquired += () =>
{
    Console.WriteLine("获取到锁");
    autoResetEvent.Set();
};
bool bl= writelock.Lock();
bool waitzk = autoResetEvent.WaitOne(15000);
if (!waitzk) 
{
    //超时未连接成功
}
```

