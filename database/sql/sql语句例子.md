数据库表StudentGrade

| stuId | subId| grade|
|:------:|:--: |:---: |
|001       	|1	|97
|001       	|2	|97
|001       	|3	|70
|002       	|1	|97
|002       	|2	|80
|002       	|3	|30
|003       	|1	|97
|003       	|2	|95
|003       	|3	|85
|004       	|1	|73
|004       	|2	|78
|004       	|3	|87

```sql
CREATE TABLE StudentGrade(
stuId CHAR(4),    --学号
subId INT,        --课程号
grade INT,        --成绩
PRIMARY KEY (stuId,subId)
)
GO
--表中数据如下
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('001',1,97);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('001',2,50);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('001',3,70);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('002',1,92);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('002',2,80);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('002',3,30);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('003',1,93);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('003',2,95);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('003',3,85);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('004',1,73);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('004',2,78);
INSERT INTO StudentGrade(stuId,subId,grade) VALUES('004',3,87);
GO
```

# 获取每个学生成绩最高的科目
```sql
  --包含重复项
  SELECT [stuId]
      ,[subId]
      ,[grade]
  FROM [Test].[dbo].[StudentGrade] a
  where exists(
  select 1 from(
	  SELECT [stuId]
		  ,max([grade]) maxgrade
	  FROM [Test].[dbo].[StudentGrade]
	  group by stuId 
	) b
	where a.stuId=b.stuId and a.grade=b.maxgrade
  )

  --根据学号分组，获取每个学生成绩最高的一条信息（只取每组一条）
  SELECT [stuId]
      ,[subId]
      ,[grade]
  FROM [Test].[dbo].[StudentGrade] a
  where subId=(
    select top 1 subId from [Test].[dbo].[StudentGrade] 
	where a.stuId=stuId
	order by grade desc
  )
```

```sql
--使用RowNum

```
# 获取最高分数对应的学科和学生id
```sql
SELECT [stuId]
      ,[subId]
      ,[grade]
  FROM [Test].[dbo].[StudentGrade] a
  where not exists(
	SELECT 1 
	FROM [Test].[dbo].[StudentGrade] b
    where a.grade<b.grade
  )
```
此表中的最高分是97分，如果只有一个97分，可以用order by来代替not exists来实现该需求，但是如果有多个97分，那么order by是满足不了需求的
# 获取每个科目前两名
## 情况1
    排名并列，即存在多个第一第二
```sql
  --方法1
  --exists
  SELECT [stuId]
      ,[subId]
      ,[grade]
  FROM [Test].[dbo].[StudentGrade] t1
  where exists(
	select 1 from 
	(
		SELECT count(DISTINCT t2.grade) counNum
		FROM [Test].[dbo].[StudentGrade] t2
		where t1.subId=t2.subId and t1.grade<=t2.grade
	) t3
	where t3.counNum<=2
  )
  order by t1.subId,t1.grade desc

  --方法2
  select * from [Test].[dbo].[StudentGrade] t1
  where (
	select count(DISTINCT t2.grade) from [Test].[dbo].[StudentGrade] 
	where subId=t1.subId and grade>=t1.grade
  )<=2
  order by subid,grade desc
```
    两个方法的查询速度还有待研究
## 情况2
    排名不并列，这个还是写程序去吧

