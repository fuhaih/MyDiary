# 入门使用

## 创建连接
```csharp
> conn = new Mongo("localhost:27017")
connection to localhost:27017
```
## 创建数据库
mongo数据库是不需要手动创建的，可以通过use切换到任何数据库，即便该数据库还没有创建，当往数据库中插入数据时才会创建数据库。
```csharp
>use mytest //创建数据库mytest
switched to db mytest
```
## 切换数据库
```csharp
> show dbs //列出所有的数据库
mytest
test
TTBEMS
> db = conn.getDB("mytest")//切换到mytest数据库中
mytest
>use mytest//同样是切换到mytest数据库中
switched to db mytest
>db//显示当前数据库
mytest
```

## 创建表
 db.createCollection(name, options)

 **参数**
 
|参数	|类型	|描述|
|:---|:---|:---|
|name|字符串|所要创建的集合名称
|options	|文档	|可选。指定有关内存大小及索引的选项

**options**

|参数	|类型	|描述|
|:---|:---|:---|
|capped	|布尔	|（可选）如果为 true，则创建固定集合。固定集合是指有着固定大小的集合，当达到最大值时，它会自动覆盖最早的文档。当该值为 true 时，必须指定 size 参数。|
|autoIndexID	|布尔	|（可选）如为 true，自动在 _id 字段创建索引。默认为 false。|
|size	|数值	|（可选）为固定集合指定一个最大值（以字节计）。如果 capped 为 true，也需要指定该字段。|
|max	|数值	|（可选）指定固定集合中包含文档的最大数量。|

```csharp
>db.createCollection("mycollection")//创建mycollection集合
{ "ok" : 1 }
```

## 数据查询
```csharp
>show tables//显示当前数据库中的所有集合(collection)
mycollection
fs.chunks
fs.files
system.indexes
system.js
>db.mycollection.find().limit(10)//查询mycollection集合中的前10条数据
```

## 数据更新

**数组更新**

在3.6版本前，不支持多维数组对象的更新
```csharp
>db.task.update({
  {"id_":"001"},
  {"tasks.id":"t001"}
},{$set:{"tasks.$.name":"test"}})
input:
{_id:001,tasks:[{ id:t001,name:fuhai},{id:t002,name:haizi}]}
output:
{_id:001,tasks:[{id:t001,name:test},{id:t002,name:haizi}]}
```
[3.6版本](https://jira.mongodb.org/browse/SERVER-831)

**更新数组中所有文档**
```csharp
>db.coll.update({}, {$set: {“a.$[].b”: 2}})
Input: {a: [{b: 0}, {b: 1}]}
Output: {a: [{b: 2}, {b: 2}]}
```
**更新数组中所有匹配的文档**
```csharp
>db.coll.update({}, {$set: {“a.$[i].b”: 2}}, {arrayFilters: [{“i.b”: 0}]})
Input: {a: [{b: 0}, {b: 1}]}
Output: {a: [{b: 2}, {b: 1}]}
```
**更新数组中所有匹配的数组元素**
```csharp
>db.coll.update({}, {$set: {“a.$[i]”: 2}}, {arrayFilters: [{i: 0}]})
Input: {a: [0, 1]}
Output: {a: [2, 1]}
```
**更新数组中所有匹配的嵌套数组**
```csharp
>db.coll.update({}, {$set: {“a.$[i].c.$[j].d”: 2}}, {arrayFilters: [{“i.b”: 0}, {“j.d”: 0}]})
Input: {a: [{b: 0, c: [{d: 0}, {d: 1}]}, {b: 1, c: [{d: 0}, {d: 1}]}]}
Output: {a: [{b: 0, c: [{d: 2}, {d: 1}]}, {b: 1, c: [{d: 0}, {d: 1}]}]}

```

**更新数组中所有匹配逻辑语句的数组**
```csharp
>db.coll.update({}, {$set: {“a.$[i]”: 2}}, {arrayFilters: [{$or: [{i: 0}, {i: 3}]}]})
Input: {a: [0, 1, 3]}
Output: {a: [2, 1, 2]}
```
## 删除表/数据库
```csharp
>db.mycollection.drop()//删除表
true
> db.dropDatabase()//删除数据库
{ "dropped" : "mytest", "ok" : 1 }
```

## 索引管理
### 创建索引
db.COLLECTION_NAME.ensureIndex(keys,options)
 **参数**
 
|参数	|类型	|描述|
|:---|:---|:---|
|keys|文档|要建立索引的参数列表。如：{KEY:1}，其中key表示字段名，1表示升序排序，也可使用使用数字-1降序
|options	|文档	|可选参数，表示建立索引的设置

**options**

|参数	|类型	|描述|
|:---|:---|:---|
|background	|布尔	|在后台建立索引，以便建立索引时不阻止其他数据库活动。默认值 false。|
|unique	|布尔	|创建唯一索引。默认值 false。|
|name	|数值	|指定索引的名称。如果未指定，MongoDB会生成一个索引字段的名称和排序顺序串联。|
|dropDups|布尔|创建唯一索引时，如果出现重复删除后续出现的相同索引，只保留第一个。|
|sparse|布尔|对文档中不存在的字段数据不启用索引。默认值是 false。|
|v|数值|索引的版本号|
|weights|数值|索引权重值，数值在 1 到 99,999 之间，表示该索引相对于其他索引字段的得分权重。|

```csharp
> db.mycollection.ensureIndex({"name": 1, "domain": -1})
{
  "createdCollectionAutomatically" : false,
  "numIndexesBefore" : 1,
  "numIndexesAfter" : 2,
  "ok" : 1
}
```

### 重建索引
db.COLLECTION_NAME.reIndex()
```csharp
> db.mycollection.reIndex()
{
  "nIndexesWas" : 2,
  "nIndexes" : 2,
  "indexes" : [
    {
	  "key" : {
		"_id" : 1
	  },
	  "name" : "_id_",
		"ns" : "mytest.mycollection"
	},
	{
	  "key" : {
		"name" : 1,
		"domain" : -1
	  },
	  "name" : "name_1_domain_-1",
	  "ns" : "mytest.mycollection"
	}
  ],
  "ok" : 1
}
```
### 查看索引
db.COLLECTION_NAME.getIndexes()
```csharp
>db.mycollection.getIndexes()//查询mycollection集合的索引
[
    { "v" : 1, "key" : { "_id" : 1 }, "name" : "_id_", "ns" : "mytest.mycollection" },
    { "v" : 1, "key" : {"name" : 1,"domain" : -1},"name" : "name_1_domain_-1","ns" : "mytest.mycollection"}
]
```

### 查看数据库中所有的索引
```csharp
>db.system.indexes.find()//查询数据库中所有索引
{ "v" : 1, 
  "key" : { "_id" : 1 }, 
  "name" : "_id_", 
  "ns" :"mytest.mycollection" }
{ "v" : 1, 
  "key" : {"name" : 1,"domain" : -1},
  "name" : "name_1_domain_-1","ns" : 
  "mytest.mycollection"}
{ 
  "v" : 1, 
  "key" : { "_id" : 1 }, 
  "name" : "_id_", 
  "ns" : "mytest.fs.files" }
{ 
  "v" : 1, 
  "key" : { "filename" : 1, "uploadDate" : 1 }, 
  "name" : "filename_1_uploadDate_1", 
  "ns" : "mytest.fs.files" }
{ 
  "v" : 1, 
  "key" : { "_id" : 1 }, 
  "name" : "_id_", 
  "ns" :"mytest.fs.chunks" }
{ 
  "v" : 1, 
  "unique" : true, 
  "key" : { "files_id" : 1, "n" : 1 }, 
  "name" : "files_id_1_n_1", 
  "ns" : "mytest.fs.chunks" }
{ 
  "v" : 1, 
  "key" : { "_id" : 1 }, 
  "name" : "_id_", 
  "ns" : "mytest.system.js" }
>db.system.indexes.find({“ns”:"mytest.mycollection"})//查询mycollection集合的索引
{ 
  "v" : 1, 
  "key" : { "_id" : 1 }, 
  "name" : "_id_", 
  "ns" : "mytest.mycollection" }
{ 
  "v" : 1, 
  "key" : {"name" : 1,"domain" : -1},
  "name" : "name_1_domain_-1",
  "ns" : "mytest.mycollection"}
```

### 查看集合中的索引大小
db.COLLECTION_NAME.totalIndexSize()
```csharp
> db.mycollection.totalIndexSize()
16352
```
### 删除索引
db.COLLECTION_NAME.dropIndex("INDEX-NAME")

db.COLLECTION_NAME.dropIndexes()
```csharp
>db.mycollection.dropIndex("name_1_domain_-1")//删除指定索引
{ "nIndexesWas" : 2, "ok" : 1 }
> db.mycollection.dropIndexes()//删除集合中所有索引
{
  "nIndexesWas" : 1,
  "msg" : "non-_id indexes dropped for collection",
  "ok" : 1
}
```

## 其他
### 查看mongodb的存储引擎
```csharp
>db.serverStatus()
```