## 开始
```csharp
var client = new MongoClient();
var client = new MongoClient("mongodb://localhost:27017");
var client = new MongoClient("mongodb://localhost:27017,localhost:27018,localhost:27019");
var database = client.GetDatabase("foo");
//集合
var collection = database.GetCollection<BsonDocument>("bar");
//GridFS
var bucket = new GridFSBucket(database);
```

## 包含子类

使用BsonKnownTypes特性
```csharp
 [BsonKnownTypes(typeof(RedirectInfo), typeof(PluralFillInfo)
        , typeof(FillInfo),typeof(UploadInfo),typeof(DwonloadInfo),typeof(AnalysisInfo))]
```

## 扩展信息

当mongodb中新增一个字段时，模型类必须要添加上相应的字段，否则会报错。

* 解决方案1：   
使用`[BsonExtraElements]`特性     
mongodb中没有匹配到字段的信息会放在`[BsonExtraElements]`特性标记的属性中


```
[BsonExtraElements]
public BsonDocument array { get; set; }
```
* 方案2

使用select new    
相当于shell的$project     
```csharp
var client = new MongoClient(GlobalConfig.MongoDbConnectStr);
var database = client.GetDatabase("test");
var clCol = database.GetCollection<classesFixed>("classes");
var result = clCol.AsQueryable().Select(m => new classesFixed
{
    enrollmentlist = m.enrollmentlist,
    title = m.title,
    _id = m._id
}).ToList();
```

## 查找文件
```csharp
IGridFSBucket bucket;
var filter = Builders<GridFSFileInfo>.Filter.And( 
    Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, "securityvideo"),
    Builders<GridFSFileInfo>.Filter.Gte(x => x.UploadDateTime, new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
    Builders<GridFSFileInfo>.Filter.Lt(x => x.UploadDateTime, new DateTime(2015, 2, 1, 0, 0, 0, DateTimeKind.Utc)));
var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
var options = new GridFSFindOptions
{
    Limit = 1,
    Sort = sort
};
using (var cursor = bucket.Find(filter, options))
{
   var fileInfo = cursor.ToList().FirstOrDefault();
   // var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
}
```
根据ID来查找文件
```csharp
IGridFSBucket bucket;
ObjectId obid = new ObjectId(id);
var filter = Builders<GridFSFileInfo>.Filter.And( 
    Builders<GridFSFileInfo>.Filter.Eq("_id", obid));
//这里用"_id"代替x => x.Id,否则会报错
var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
var options = new GridFSFindOptions
{
    Limit = 1,
    Sort = sort
};
using (var cursor = bucket.Find(filter, options))
{
   var fileInfo = cursor.ToList().FirstOrDefault();
   // var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
}
```

## 更新嵌套数组
```csharp
public async Task<UpdateResult> UpdateNode(string taskID, string subTaskID, Node node)
{
    var collection = database.GetCollection<TempleTask>("Task");

    var filter = new BsonDocument() {
        { "_id",new ObjectId(taskID)},
    };
    var subfilter = new BsonDocument() {
        { "i.ID",subTaskID}
    };
    var nodefilter= new BsonDocument() {
        { "j.ID",node.ID}
    };
    var update = Builders<TempleTask>.Update.Set("SubTasks.$[i].Nodes.$[j]", node);
    UpdateOptions option = new UpdateOptions()
    {
        ArrayFilters = new BsonDocumentArrayFilterDefinition<TempleTask>[] {
            new BsonDocumentArrayFilterDefinition<TempleTask>(subfilter),
            new BsonDocumentArrayFilterDefinition<TempleTask>(nodefilter)
        }
    };
    
    UpdateResult result = await collection.UpdateManyAsync(filter, update, option);
    return result;
}
```
ArrayFilters只有3.6及以上版本有

## 包含ObjectId对象的序列化和反序列化

```csharp
//序列化BsonExtensionMethods的ToJson扩展方法方法

Temple temple=new Temple(_id=new ObjectId(idstring);
string json=temple.ToJson();

//反序列化
Temple temple=BsonSerializer.Deserialize<Temple>(json);

```

## lookup操作
>使用Lambada表达式进行操作

```csharp
string[] tags = new string[] { "一年级", "困难" };
var client = new MongoClient(GlobalConfig.MongoDbConnectStr);
var database = client.LASSTS();
var colletion = database.Questions<ExamQuestions>();
var sourceCol = database.Source<SourceGroup>();
var query = from q in colletion.AsQueryable().AsEnumerable()
            join s in sourceCol.AsQueryable()
            on q.SourceID equals s._id
            where tags.All(m => q.Tags.Contains(m)) && !s.Share && q.Authorization.Contains("S001T0002")
            select q;
List<ExamQuestions> result = query.Count() == 0 ? new List<ExamQuestions>() : query.ToList();
```
试题库ExamQuestions   
资源库SourceGroup   
关联关系 question.SourceID ---- source._id    
需要注意，当要返回整个document时，需要使用`AsEnumerable`

>使用linq进行操作

classes
```sh
db.classes.insert( [
   { _id: 1, title: "Reading is ...", enrollmentlist: [ "giraffe2", "pandabear", "artie" ], days: ["M", "W", "F"] },
   { _id: 2, title: "But Writing ...", enrollmentlist: [ "giraffe1", "artie" ], days: ["T", "F"] }
])
```
members
```sh
db.members.insert( [
   { _id: 1, name: "artie", joined: new Date("2016-05-01"), status: "A" },
   { _id: 2, name: "giraffe", joined: new Date("2017-05-01"), status: "D" },
   { _id: 3, name: "giraffe1", joined: new Date("2017-10-01"), status: "A" },
   { _id: 4, name: "panda", joined: new Date("2018-10-11"), status: "A" },
   { _id: 5, name: "pandabear", joined: new Date("2018-12-01"), status: "A" },
   { _id: 6, name: "giraffe2", joined: new Date("2018-12-01"), status: "D" }
])
```

```csharp
var client = new MongoClient(GlobalConfig.MongoDbConnectStr);
var database = client.GetDatabase("test");
var clCol = database.GetCollection<classes>("classes");
var memberCol = database.GetCollection<members>("members");

var pipeline = PipelineStageDefinitionBuilder.Lookup<classes,members,classes>(memberCol, "enrollmentlist", "name", "enrollee_info");

var aggregate = clCol
    .Aggregate()
    .AppendStage(pipeline);
var organizationsList = aggregate.ToList();
```

上诉方法对应的shell
```sh
db.classes.aggregate([
   {
      $lookup:
         {
            from: "members",
            localField: "enrollmentlist",
            foreignField: "name",
            as: "enrollee_info"
        }
   }
])
```

输出
```sh
{
   "_id" : 1,
   "title" : "Reading is ...",
   "enrollmentlist" : [ "giraffe2", "pandabear", "artie" ],
   "days" : [ "M", "W", "F" ],
   "enrollee_info" : [
      { "_id" : 1, "name" : "artie", "joined" : ISODate("2016-05-01T00:00:00Z"), "status" : "A" },
      { "_id" : 5, "name" : "pandabear", "joined" : ISODate("2018-12-01T00:00:00Z"), "status" : "A" },
      { "_id" : 6, "name" : "giraffe2", "joined" : ISODate("2018-12-01T00:00:00Z"), "status" : "D" }
   ]
}
{
   "_id" : 2,
   "title" : "But Writing ...",
   "enrollmentlist" : [ "giraffe1", "artie" ],
   "days" : [ "T", "F" ],
   "enrollee_info" : [
      { "_id" : 1, "name" : "artie", "joined" : ISODate("2016-05-01T00:00:00Z"), "status" : "A" },
      { "_id" : 3, "name" : "giraffe1", "joined" : ISODate("2017-10-01T00:00:00Z"), "status" : "A" }
   ]
}
```

>pipeline

pipeline管道可以用来对外链集合进行复杂处理，这里处了使用name字段来匹配外，还通过status字段来过滤members集合

```csharp
var client = new MongoClient(GlobalConfig.MongoDbConnectStr);
var database = client.GetDatabase("test");
var clCol = database.GetCollection<classes>("classes");
var memberCol = database.GetCollection<members>("members");

BsonDocument let = new BsonDocument {
  new BsonElement("enrollment","$enrollmentlist")
};

var qfilter = new BsonDocument {
    new BsonElement("$expr",new BsonDocument {
        new BsonElement("$and",new BsonArray {
            new BsonDocument("$eq",new BsonArray { "$name", "$$enrollment"}),
            new BsonDocument("$eq",new BsonArray { "$status", "D"})
        })
    })
};
PipelineDefinition<members,members> lookuppipeline = new EmptyPipelineDefinition<members>()
    .Match(qfilter);


var pipeline = PipelineStageDefinitionBuilder.Lookup<classes,members, members,IEnumerable<members>,classes>(memberCol, let, lookuppipeline, m=>m.enrollee_info);

var aggregate = clCol
    .Aggregate()
    .AppendStage(pipeline);
var organizationsList = aggregate.ToList();
```

上诉方法对应的shell
```sh
db.classes.aggregate([
   {
      $lookup:
         {
            from: "members",
            localField: "enrollmentlist",
            foreignField: "name",
            let: { enrollment: "$enrollmentlist" },
            pipeline: [
                { $match:
                  { $expr:
                      { $and:
                        [
                          { $eq: [ "$name", "$$enrollment" ] },
                          { $eq: [ "$status", "D" ] }
                        ]
                      }
                  }
                }
            ],
            as: "enrollee_info"
        }
   }
])
```
输出
```sh
{
   "_id" : 1,
   "title" : "Reading is ...",
   "enrollmentlist" : [ "giraffe2", "pandabear", "artie" ],
   "days" : [ "M", "W", "F" ],
   "enrollee_info" : [
      { "_id" : 6, "name" : "giraffe2", "joined" : ISODate("2018-12-01T00:00:00Z"), "status" : "D" }
   ]
}
{
   "_id" : 2,
   "title" : "But Writing ...",
   "enrollmentlist" : [ "giraffe1", "artie" ],
   "days" : [ "T", "F" ],
   "enrollee_info" : []
}
```

>其他操作

$mergeObjects 操作，可以merge关联的两个document   
详情看[文档](https://docs.mongodb.com/manual/reference/operator/aggregation/lookup/index.html)

## 副本集连接

连接字符串`mongodb://192.168.68.36:27018,192.168.68.36:27019,192.168.68.36:27020/?replicaSet=rs0`

其他正常操作就行了

```csharp
var client = new MongoClient("mongodb://192.168.68.36:27018,192.168.68.36:27019,192.168.68.36:27020/?replicaSet=rs0");
var database = client.GetDatabase("test");
var clCol = database.GetCollection<classes>("classes");
var memberCol = database.GetCollection<members>("members");
```

也可以通过`MongoClientSettings`进行连接

```csharp
MongoClientSettings setting = new MongoClientSettings();
//setting.ConnectionMode = ConnectionMode.
setting.ReplicaSetName = "rs0";
//setting.Server = new MongoServerAddress("192.168.68.36");
setting.Servers = new List<MongoServerAddress>() {
    new MongoServerAddress("192.168.68.36",27018),
    new MongoServerAddress("192.168.68.36",27019),
    new MongoServerAddress("192.168.68.36",27020)
};
var client = new MongoClient(setting);
var database = client.GetDatabase("test");
var clCol = database.GetCollection<classes>("classes");
var memberCol = database.GetCollection<members>("members");
```

这里需要注意的是，mongodb用副本集模式来进行操作时，会用配置的连接连接到MongoDB，获取副本集的信息(_id,host等)，找到master，连接master来进行增删改操作，所以副本集在配置host时，要配置成`ip：port`格式，配置成`localhost:port`的话，获取副本集信息时就是返回`localhost:port`，直接连接`localhost:port`，会报错。

## 读偏好配置

副本集中`Primary`节点和`Secondary`节点都能进行读操作，如果从`Secondary`读取，可以扩大读取吞吐量。

readPreference
```yml

primary：仅从primary进行读操作，缺点就是primary节点压力比较大，Secondary节点的资源也是挺浪费的

primaryPreferred：默认是读取primary节点，当primary节点宕掉时，才读取Secondary节点，缺点跟primary一样。

secondary：只读取secondary，缺点就是，当所有secondary都宕掉时，读取操作将异常。

secondaryPreferred：默认读取secondary，当所有secondary宕掉时，读取primary

```

```csharp
MongoClientSettings setting = new MongoClientSettings();
setting.ReplicaSetName = "rs0";
setting.ReadPreference = ReadPreference.SecondaryPreferred;
setting.Servers = new List<MongoServerAddress>() {
    new MongoServerAddress("192.168.68.36",27018),
    new MongoServerAddress("192.168.68.36",27019),
    new MongoServerAddress("192.168.68.36",27020)
};
var client = new MongoClient(setting);
var database = client.GetDatabase("test");
var clCol = database.GetCollection<classes>("classes");
var memberCol = database.GetCollection<members>("members");
```

或者使用连接字符串

```csharp
var client = new MongoClient("mongodb://192.168.68.36:27018,192.168.68.36:27019,192.168.68.36:27020/?replicaSet=rs0&readPreference=secondaryPreferred&maxStalenessSeconds=120");
var database = client.GetDatabase("test");
var clCol = database.GetCollection<classes>("classes");
var memberCol = database.GetCollection<members>("members");
```
`maxStalenessSeconds`最大过时时间，也就是当副本集secondary节点的数据落后于primary节点的时间，当超过最大过期时间时，会停止对secondary节点的读取。

当选择了使用maxStalenessSeconds进行读操作的服务端，客户端会通过比较从节点和主节点的最后一次写时间来估计从节点的过期程度。客户端会把连接指向估计落后小于等于maxStalenessSeconds的从节点。

`readPreferenceTags`通过节点的tags来对偏好进行设置。

## 事务操作

```csharp
var client = new MongoClient(GlobalConfig.MongoDbConnectStr);
var database = client.GetDatabase("test");
var collection = database.GetCollection<Question>("Question");
using (var session = client.StartSession())
{
    var transactionOptions = new TransactionOptions(
    readPreference: ReadPreference.Primary,
    readConcern: ReadConcern.Local,
    writeConcern: WriteConcern.WMajority);
    session.StartTransaction(transactionOptions);
    try
    {
        for (int i = 0; i < 3; i++)
        {
            //if (i == 1) throw new Exception("test_transaction");
            Question question = new Question
            {
                Answers = null,
                Name = "qtest" + i
            };
            //操作时必须带入session
            collection.InsertOne(session,question);
        }
        session.CommitTransaction();
    }
    catch (Exception ex)
    {
        session.AbortTransaction();
    }
}
```

```csharp
CancellationTokenSource source = new CancellationTokenSource();
MongoClientSettings setting = new MongoClientSettings();
setting.ConnectionMode = ConnectionMode.ReplicaSet;
setting.ReplicaSetName = "rs0";
setting.Servers = new List<MongoServerAddress>() {
    new MongoServerAddress("192.168.68.36",27018),
    new MongoServerAddress("192.168.68.36",27019),
    new MongoServerAddress("192.168.68.36",27020)
};
var client = new MongoClient(setting);
var database = client.GetDatabase("test");
var collection = database.GetCollection<Question>("Question");
using (var session = client.StartSession())
{
    var transactionOptions = new TransactionOptions(
    readPreference: ReadPreference.Primary,//这里可以设置从secondary读，但是事务时只能从primary读
    readConcern: ReadConcern.Local,
    writeConcern: WriteConcern.WMajority);

    var result = session.WithTransaction(
    (s, ct) =>
    {
        for (int i = 0; i < 3; i++)
        {
            //if (i == 1) throw new Exception("test_transaction");
            Question question = new Question
            {
                Answers = null,
                Name = "qtest" + i
            };
            //集合操作时，必须要把session带进去。
            collection.InsertOne(s,question,cancellationToken: ct);
        }
        return "ok";
    },
    transactionOptions,
    source.Token);
}
```

**事务注意事项：**

* writeConcern: WriteConcern.WMajority

* readPreference: ReadPreference.Primary

也就是只能从primary进行读取。

* session

在进行操作时需要带上session