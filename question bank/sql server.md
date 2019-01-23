# 1、基本知识点

>sql server 编译、重编译、执行计划

>基本数据类型

>索引机制、几种索引和区别

>引擎

>锁机制

>慢sql定位和优化

>索引的原理

>索引重建

>union 和union all区别，lef join，not exists

# 2、锁

## 2.1、关于锁的几个概念
>粒度

锁是用来锁定资源，而资源是包括很多种的，而这些不同的资源代表着不同的粒度

|资源|说明|
|--|--|
|RID|用于锁定堆中的单个行的行标识符。
|KEY|索引中用于保护可序列化事务中的键范围的行锁。
|PAGE|数据库中的 8 KB 页，例如数据页或索引页。
|EXTENT|一组连续的八页，例如数据页或索引页。
|HoBT|堆或 B 树。 用于保护没有聚集索引的表中的 B 树（索引）或堆数据页的锁。
|TABLE|包括所有数据和索引的整个表。
|FILE|数据库文件。
|APPLICATION|应用程序专用的资源。
|METADATA|元数据锁。
|ALLOCATION_UNIT|分配单元。
|DATABASE|整个数据库。
|OBJECT|代表一个数据库对象，包括数据库表、视图、存储过程或者任何包含Object ID的对象。

>层次结构

    表、分区、页、行、键
>锁的类型

    共享锁(S)、更新锁(U)、排他锁(X)和架构锁(Sch)
>锁的兼容性

    不同类型的锁，有些是互斥的，有些是兼容的。如共享锁与其它类型的锁相互兼容，排他锁与其它的锁类型互斥。
>意向锁(I)

    在分配锁时，上级的资源会被分配意向锁(I)，用来表示这个资源的下级某个资源已经被锁定了。意向锁也可以分为IS,IX,IU等类型。例如，更新表中某一行，需要在在行上分配X锁，而在行所属的数据页中分配意向锁IX，数据页所属的表上分配IX锁。
>SIX锁(意向排他共享锁)

    如果一个会话的事务当前持有了某个表或者数据页的S锁，而它接下来又要去修改表中的某一个行。这种情况下，事务需要获取行上的X锁和表或数据页上的IX锁，但是SQL Server只允许一个会话在一个资源上获取一个锁。也就是说没有办法在已经获得表或者页级别的S锁之后又分配IX给它。为了解决这个问题，于是就出现了两者的结合体：S+IX=SIX。 同理，如果先持有IX，再去获取S，也会得到SIX。
    另外SQL Server中还有类似的锁类型UIX(U+IX),SIU(S+IU),机理也是一样的。这三种锁被称为转换锁。
>锁分区

>NOLOCK

    使用：select * from tablename with(nolock)
    优点：提高查询速率，由于查找没有上锁，所以不会发生死锁。
    缺点：可能会有脏数据
    使用场景:在查询数据时，如果可以忽略脏数据，可以使用with(nolock)来提高查找速率，以及避免死锁

>READPATH

    使用：select * from tablename with(READPATH)
    优点:不会加锁，所以不会发生死锁，在读取数据时会跳过添加了排他锁的数据，所以不会有脏数据
    缺点：由于跳过了排他锁的数据，所以会导致我们有些需要的结果是没有在查询结果列表里的。


## 2.2、数据库中快速查找死锁

    1、打开 管理 > 扩展事件 > 会话 > system_health > package0.event_file 双击打开

    2、找到死锁发生时间的xml_deadlock_report行

    3、将死锁xml信息文本保存为后缀是.xdl的文件。 

    4、将.xdl文件打开或者直接看xml信息也能了解死锁的产生原因

## 2.3、查看某条语句会加那些锁
```sql
BEGIN TRANSACTION

SELECT * FROM Test WITH (HOLDLOCK)
WHERE ID = 5000

SELECT * FROM sys.dm_tran_locks
WHERE request_session_id = @@SPID

ROLLBACK
GO
```
WITH (HOLDLOCK)表示锁贯穿整个事务，直到事务结束才释放锁，所以查询语句的锁会保持到事务结束，这样方便我们查询锁信息。`@@SPID`是当前线程ip，也就是筛选出当前线程加的锁。

```sql
ALTER INDEX idx_ci ON Foo REBUILD
WITH (ALLOW_ROW_LOCKS = OFF)
GO
```
在重建索引时可以使用ALLOW_ROW_LOCKS 和ALLOW_PAGE_LOCKS来设置是否停用行层级锁和页层级锁
# 3、数据库事务



## 3.1、数据库事务ACID特性

>原子性（Atomicity）

>一致性（Consistency）

>隔离性（Isolation）

>持久性（Durability）

## 3.2、事务的隔离级别

>READ_UNCOMMITTED

>READ_COMMITED

>REPEATABLE_READ

>SERLALIZABLE

# 4、数据库索引

# 5、数据库设计

## 5.1、范式