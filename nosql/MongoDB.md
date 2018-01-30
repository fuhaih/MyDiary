# GridFS
[官网说明](https://docs.mongodb.com/manual/core/gridfs/#chunk-disambiguation)
## 什么时候使用GridFS
* GridFS可以存储超出16M大小的文件，MongoDB Collection中也可以存储文件，但是文件存储的上限是16M，所以超出该上限的时候需要考虑用GridFS进行文件存储
* GridFS可以从一个大文件中获取一部分信息，而不需要将整个文件读取到内存中。
* GridFS可以实现文件自动同步到多台服务器

GridFS不适用于原子级地更新整个文件的需求。
## 通过metadata进行检索
```csharp
BasicDBObject query = new BasicDBObject("metadata.target_field", "abcdefg"));
List<GridFSDBFile> files = gridFs.find(query);
```