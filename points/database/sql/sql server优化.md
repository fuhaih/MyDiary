1、使用parameter时，可能会导致查询变慢，特别是有varchar类型parameter时，可以的话尽可能sql拼接。
2、小心建立索引
一个表`CreateTime`和`Section`还有`Nonce`字段都有单独索引，为了方便查询

```sql
SELECT TOP 60 Nonce FROM [TTVVP_System].[dbo].[T_BC_Transaction] 
WHERE CreateTime>='2021-08-14 00:00:00' AND CreateTime<'2021-08-15 00:00:00' AND Section='ttbemsSub'  
```
这个时候时查询很快的，但是当加上`order by Nonce`后变得很慢
```sql
SELECT TOP 60 Nonce FROM [TTVVP_System].[dbo].[T_BC_Transaction] 
WHERE CreateTime>='2021-08-14 00:00:00' AND CreateTime<'2021-08-15 00:00:00' AND Section='ttbemsSub'  
order by Nonce
```

然后删除了Nonce索引后，有order by的查询稍微变快了，感觉是WHERE的时候走了索引，但是在order by Nonde的时候又走None索引(in?)，导致变慢。

由于查询条件有`CreateTime`和`Section`这两个，所以再创建一个包含这两个字段的非聚集索引

**可以再进行优化**:CreateTime和Section两个查询条件只需要用到Nonce值，可以创建一个包含索引。

3、进行TOP查询的时候要主要order by

