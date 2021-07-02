# 使用

>join

ef中通常是每个context对应一个数据库，也是对应着一个数据库连接，所以两个context，也就是两个数据库连接，不能进行jion操作

join操作需要在同一个连接里进行同一个连接的情况下，没办法查询其他库的数据，也就没办法垮库查询，

解决方案：

* 1、使用别名操作

例如：能耗库EnergyContext需要和用户库中某个表User进行连接，那么可以给User表监理别名 EnergyUser
然后在EnergyContext中`Entity<EnergyUser>().ToTable("dbo.EnergyUser")`
这样就可以在EnergyContext中访问User库的User表了，也就能进行连接操作了

* 2、sql查询

另外一种方法就是写sql的方式进行查询，`Set<EnergyValue>().SqlQuery(command)`

**建议**

相同业务尽量不要进行分库

# 异常

>事务(进程 ID 81)与另一个进程被死锁在 锁 资源上，并且已被选作死锁牺牲品。请重新运行该事务。

数据查询发生死锁，这个在使用`SqlQuery`进行sql操作的时候发生的异常，主要原因是查询的表是经常更新的表，所以在查询数据的时候会发生异常。

根据不同需求使用不同解决方法

解决方法

* NOLOCK

`SELECT COUNT(*) FROM Example WITH(NOLOCK)`

查询的时候不加锁，这个表的数据会经常更新插入新的数据，但是在查询的时候是不需要一定要更新完再获取到的，所以可以使用NOLOCK来获取数据，缺点是可能未提交的事务也会被读取到，就会导致脏读

* READPAST

`SELECT COUNT(*)FROM Example WITH(READPAST)`

在读取数据时不会返回正在锁定的行和页


