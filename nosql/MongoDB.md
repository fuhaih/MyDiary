# GridFS
[官网说明](https://docs.mongodb.com/manual/core/gridfs/#chunk-disambiguation)
## 什么时候使用GridFS
* GridFS可以存储超出16M大小的文件，MongoDB Collection中也可以存储文件，但是文件存储的上限是16M，所以超出该上限的时候需要考虑用GridFS进行文件存储
* GridFS可以从一个大文件中获取一部分信息，而不需要将整个文件读取到内存中。
* GridFS可以实现文件自动同步到多台服务器

GridFS不适用于原子级地更新整个文件的需求。

## GridFS集合
GridFS文件是存储在两个集合中的
* chunks 存储文件字节数据 
* files 存储文件的 metadata
GridFS在存储文件的时候，会根据files中chunkSize大小（默认256kb）把文件切割为多个chunks进行存储。然后在获取文件的时候，会根据files_id查找到所有的chunks，再根据chunks中的n字段进行拼接，得到整个文件。

![示例图](https://github.com/fuhaih/MyDiary/blob/master/nosql/gridfs存储示意图.png)

### chunk集合
```csharp
{
  "_id" : <ObjectId>,
  "files_id" : <ObjectId>,
  "n" : <num>,
  "data" : <binary>
}
```
每个chunk中包含上面几个字段

**chunks._id**：chunks的id

**chunks.files_id**：对应的files_id

**chunks.n**：chunks的序列号

**chunks.data**：BSON 二进制数据

### files集合
```csharp
{
  "_id" : <ObjectId>,
  "length" : <num>,
  "chunkSize" : <num>,
  "uploadDate" : <timestamp>,
  "md5" : <hash>,
  "filename" : <string>,
  "contentType" : <string>,
  "aliases" : <string array>,
  "metadata" : <any>,
}
```
每个files包含以上字段

**files._id**：id，可以使BSON ObjectId，也可以用guid.

**files.length**：文件字节大小

**files.chunkSize**：规定每个chunk的大小，版本2.4.10: 默认chunk size从256kB变为 255kB.

**files.uploadDate**：文件首次存储的时间

**files.md5**：文件的md5值

**files.filename**：文件名

**files.contentType**：文件类型

**files.aliases**：文件别名

**files.metadata**：元数据，可以存储任意类型的信息（object），一般BSON格式存储用户自定义的文件信心，用来进行文件检索
### GridFS中索引
#### chunks 索引
chunks用files_id and n字段作为唯一的聚集索引，这样可以有效地检索文件
```csharp
db.fs.chunks.find( { files_id: myFileID } ).sort( { n: 1 } )
```
如果索引不存在，可以用mongo shell进行索引创建
```csharp
db.fs.chunks.createIndex( { files_id: 1, n: 1 }, { unique: true } );
```
#### files索引
files集合用 filename 和 uploadDate两个字段作为索引
```csharp
db.fs.files.find( { filename: myFileName } ).sort( { uploadDate: 1 } )
```
如果索引不存在
```csharp
db.fs.files.createIndex( { filename: 1, uploadDate: 1 } );
```
## 通过metadata进行检索
```csharp
BasicDBObject query = new BasicDBObject("metadata.target_field", "abcdefg"));
List<GridFSDBFile> files = gridFs.find(query);
```