# 安装
>安装redis
```
#如果没有redis源，先安装fedora的epel仓库
$ yum install epel-release
$ yum install redis
```
>修改配置
```vim
$ vim /etc/redis.conf
```

配置如下信息
```config
port 6379
requirepass 111
# 127.0.0.1会使得redis只能本机访问
bind 0.0.0.0
protected-mode no
```

>启动服务
```vim
$ systemctl start redis.service
$ systemctl enable redis.service
```
# StackExchange.Redis
>基本用法
```csharp
ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("server1:6379,server2:6379");
IDatabase db = redis.GetDatabase();
//存取
string valueset = "abcdefg";
db.StringSet("mykey", valueset);
string valueget = db.StringGet("mykey");
//发布订阅
ISubscriber sub = redis.GetSubscriber();
sub.Subscribe("messages", (channel, message) => {
    Console.WriteLine((string)message);
});
sub.Publish("messages", "hello");
//获取服务信息
IServer server = redis.GetServer("localhost", 6379);
DateTime lastSave = server.LastSave();
ClientInfo[] clients = server.ClientList();
//异步
string valueset = "abcdefg";
await db.StringSetAsync("mykey", valueset);
string valueget = await db.StringGetAsync("mykey");
```
>ConfigurationOptions
```csharp
var options = ConfigurationOptions.Parse(configString);
options.ClientName = GetAppName(); // only known at runtime
options.AllowAdmin = true;
options.Password="11111";
options.EndPoints.Add("server1:6379");
conn = ConnectionMultiplexer.Connect(options);
```

>事务
```csharp
IDatabase db= redis.GetDatabase();
ITransaction transcation= db.CreateTransaction();
transcation.AddCondition(Condition.HashNotExists("testHash", "UniqueID"));
transcation.HashSetAsync("testHash", "UniqueID", newId.ToString());
bool bl= transcation.Execute();
if (!bl)
{
    Console.WriteLine("添加失败");
}
```
redis中hash有Field和Value两个字段   
上面事务：查找Field为UniqueID的hash值，如果没有就添加一个
>锁
```csharp
//加锁，加上过期时间，避免死锁
bool bl = db.LockTake("uniqueid", "111111", DateTime.Now.AddMilliseconds(10)-DateTime.Now);
//dosomething
await Task.Delay(10000);
if (bl)
{
    //释放锁
    db.LockRelease("uniqueid", "111111");
}
```
>批量
```csharp
IBatch batch = db.CreateBatch();
batch.StringSetAsync("test1","1");
batch.StringSetAsync("test2","2");
batch.StringSetAsync("test3","3");
batch.StringSetAsync("test4","4");
batch.Execute();
```

>CommandFlags.FireAndForget

表示方法不关注执行结果，执行方法时会立刻返回一个默认值。

>线程安全性  

StackExchange.Redis操作是线程安全的。

>StringIncrement        

StringIncrement和StringDecrement默认的增减为1，通过重载方法可以自定义增减系数
```csharp
//增1
db.StringIncrement("testInCrement");
//增2
db.StringIncrement("testInCrement",2);
//减1
db.StringDecrement("testInCrement";
//减2
db.StringDecrement("testInCrement",2);
```

# redis集群
# redis生产环境问题
>缓存雪崩

>缓存击穿