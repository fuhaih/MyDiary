# 获取当前日期
```sql
declare @now datetime
set @now= getdate()
```
# 获取日期单独部分信息

获取日期单独部分信息有两个方法,DATENAME和DATEPART方法，这两种方法的用法是一样的。

DATENAME(datepart,date)
```sql
declare @now datetime
set @now=GETDATE()
select DATENAME(YY,@now)
select DATENAME(MM,@now)
select DATENAME(DD,@now)
select DATENAME(HH,@now)
select DATENAME(MI,@now)
select DATENAME(SS,@now)
select DATENAME(MS,@now)
```
DATEPART 方法同理
# 日期加减
DATEADD(datepart,number,date)

例子：获取前一天日期

```sql
declare @now datetime
set @now=dateadd(day, -1, getdate())
select @now
```

# datepart缩写表
| datepart | 缩写|
|:------:  |--:  |
|年	      |yy, yyyy|
|季度      |qq, q|
|月	      |mm, m|
|年中的日	|dy, y|
|日	      |dd, d|
|周	      |wk, ww|
|星期	     |dw, w|
|小时	     |hh|
|分钟	     |mi, n|
|秒	      |ss, s|
|毫秒	     |ms|
|微妙	     |mcs|
|纳秒	     |ns|

# 日期格式化
CONVERT(data_type,e­xpression,[style])
```sql
select CONVERT(varchar(100), GETDATE(), 23)
```

# 日期格式化用表
|Style(2位表示年份) | Style(4位表示年份) | 输入输出格式 |
|:---------------: |:------------------:|:------------|
|0 | 100 | mon dd yyyy hh:miAM(或PM) 
|1 | 101 | mm/dd/yy 
|2 | 102 | yy-mm-dd 
|3 | 103 | dd/mm/yy 
|4 | 104 | dd-mm-yy 
|5 | 105 | dd-mm-yy 
|6 | 106 | dd mon yy 
|7 | 107 | mon dd,yy 
|8 | 108 | hh:mm:ss 
|9 | 109 | mon dd yyyy hh:mi:ss:mmmmAM(或PM)
|10 | 110 | mm-dd-yy 
|11 | 111 | yy/mm/dd 
|12 | 112 | yymmdd 
|13 | 113 | dd mon yyyy hh:mi:ss:mmm(24小时制) 
|14 | 114 | hh:mi:ss:mmm(24小时制) 
|20 | 120 | yyyy-mm-dd hh:mi:ss(24小时制) 
|21 | 121 | yyyy-mm-dd hh:mi:ss:mmm(24小时制)
|22 | 120 | mm/dd/yy hh:mi:ss(24小时制) 
|23 | -   | yyyy-mm-dd AM(或PM)
|24 | -   | hh:mi:ss(24小时制)
|25 | -   | yyyy-mm-dd hh:mi:ss:mmm(24小时制)
|-  | 126 | yyyy-mm-ddThh:mi:ss:mmm(24小时制)

