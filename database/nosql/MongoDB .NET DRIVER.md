# GridFS操作
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
