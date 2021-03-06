>Lock Convoy

Lock Convoy锁护送

产生原因：

```csharp
WaitForSingleObject()//step 1
....dosomething
WaitForSingleObject()//step 2
....dosomething
WaitForSingleObject()//step 3
.....dosomething
```

`WaitForSingleObject`方法内部会上锁

该代码段多次使用`WaitForSingleObject`方法，将会多次请求锁

假设有两个相同优先级的线程(线程A，线程B)执行该代码段，线程A在step1获取到锁，此时线程A时间片结束，线程A还没释放锁，时间片交由线程B执行，线程B执行到step 1时就会等待锁，直到时间片结束，再由线程A执行，线程A完成step 1，释放锁，时间片结束，到线程B执行，线程B在step 1获取到锁，执行step 1，还没完成step 1，时间片结束了，再交由线程A执行，线程A执行到step 2，调用的相同方法，请求的是相同的锁，现在锁还是线程B持有，所以线程A只能等待线程B释放锁。

由上面的描述可以知道，锁的存在是会限制并发操作的，当只有一个锁的时候，影响不是太大，但是当代码段多次请求同一个锁的时候，代码的并发将被锁限制。

锁护送产生条件：

* 同等级线程
* 频繁竞争同一个锁
* 线程调度策略为FIFO调度


锁本来就是会影响到并发性能的，原本多线程并发操作由于锁的存在，其他线程会阻塞，只能一个一个线程竞争锁然后执行，而锁护送就更加恶劣，同一个线程多次竞争同一个锁，

解决方案：批量操作

既然是竞争的同一个锁，那么操作的资源是相同的，这时候可以把锁放在外面，再批量操作资源，当然最好要控制好批量操作资源的时间，锁占用的时间不宜过长。



