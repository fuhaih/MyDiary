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


![示例图](https://github.com/fuhaih/MyDiary/blob/master/nosql/gridfs%E5%AD%98%E5%82%A8%E7%A4%BA%E6%84%8F%E5%9B%BE.png)
## 通过metadata进行检索
```csharp
BasicDBObject query = new BasicDBObject("metadata.target_field", "abcdefg"));
List<GridFSDBFile> files = gridFs.find(query);
```