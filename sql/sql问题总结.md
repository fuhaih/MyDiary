# 表中字段是一个关键字
比如说有一个表有个字段是”group“，在进行表设计的时候，把字段的名称写成“[group]”就行了
# 表中数据包含中括号（[]）
表中数据包含中括号的时候，在进行模糊查询的时候是有问题的

比如说查询name字段中以“[DB]”开头的行

查询语句： 
```sql
select * from tablename
where name like '[DB]%'
```
该语句的实际意思是
```sql
select * from tablename
where name like 'D%' or name like 'B%'
```
也就是说这里的中括号已经有特殊含义了

同理
```sql
select * from tablename
where name like '%[DB]%'
```
语句的功能和下列的语句相同
```sql
select * from tablename
where name like '%D%' or name like '%B%'
```
所以想要得出正确结果需要把中括号进行转义操作，sql中可以用ESCAPEl来自定义转义字符
```sql
select * from tablename
where name like '/[DB/]%' ESCAPE '/'
```
这里把“/”符号设置为转义字符，然后把中括号进行转义