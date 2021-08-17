# 多线程

## 线程

### Threading

>ThreadStart

>ParameterizedThreadStart


## 线程同步

### Lock

Lock对象需要是一个引用对象，引用类型是存储在堆中，对象描述中会有一个同步块索引，这个就是用来做同步操作的一个标识，标识是否已经进入了同步操作，`GetHashCode`方法也是和同步索引有关的

而值对象是存储在栈中，没有同步块索引的描述，所以无法进行lock操作

### ReaderWriterLock和ReaderWriterLockSlim

读写锁，能多个线程同时获取读锁，只能有一个线程获取写锁，多个读锁可以同时进入同步块，读锁和写锁不能同时进入同步块

使用场景：多读少写的情况

例子：文件读取和修改

读取：
```csharp
rwlock.EnterReadLock();//获取读锁，
try
{
    using(FileStream stream = new FileStream(path, FileMode.Open,FileAccess.Read, FileShare.Read))
    {

    }
}
finally {
    rwlock.ExitReadLock();
}

```
修改：
```csharp
rwlock.EnterWriteLock();//获取读锁，
try
{
    using(FileStream stream = new FileStream(path, FileMode.Create,FileAccess.Write, FileShare.Read))
    {
    }
}
finally {
    rwlock.ExitWriteLock();
}

```

多线程读取文件的时候，需要设置`FileShare.Read`,否则会发生文件占用异常，当线程获取写锁时，其他线程将会阻塞，无法获取到读锁和写锁，保证写入的安全。

`EnterUpgradeableReadLock`进入可升级读锁，可升级读锁能在里面可以使用`EnterWriteLock`方法来升级读锁为写锁，这个操作是原子级别的操作，也就是当多个读锁升级为写锁时只有一个成功

可升级锁比较常用的场景为如果不能存在则新增

```csharp
rwl.EnterUpgradeableReadLock();
try
{
    JobEventInfo eventinfo = NiagaraConfigContext.JobEventInfos.OrderByDescending(m => m.Time).FirstOrDefault();
    if (eventinfo == null)
    {
        // 如果不存在时，需要进行写入操作
        // 原子操作升级读锁为写锁
        rwl.EnterWriteLock();
        try
        {
            // 可能会多个线程请求升级锁为写锁，这时候只有一个线程获取到写锁，在写入后，其他等待的线程会获取到写锁，这时候需要重新判断JobEventInfos表是否有数据
            eventinfo = NiagaraConfigContext.JobEventInfos.OrderByDescending(m => m.Time).FirstOrDefault();
            if (eventinfo == null) {
                eventinfo = new JobEventInfo()
                {
                    Time = DateTime.Now,
                };
                NiagaraConfigContext.JobEventInfos.Add(eventinfo);
                await NiagaraConfigContext.SaveChangesAsync();
            }
        }
        finally
        {
            rwl.ExitWriteLock();
        }

    }
    return eventinfo;
}
finally
{
    rwl.ExitUpgradeableReadLock();
}

```

### Monitor 

lock 操作会编译为Monitor，直接使用Monitor会更灵活一点，包括有超时操作等，



### WaitHandles、AutoResetEvent和ManualResetEventSlim

锁的一个重要特性是它提供了公平性.换句话说,保证争夺锁的线程能最终获取锁，Monitor类提供了这样的保证,由CLR中的等待队列实现.而Mutex和Semaphore操作系统提供了这种保证

WaitHandles 不提供这样的保证,所以不应该用来实现锁类似的功能,否则可能同一个线程可以反复获取它,其他线程可以永远饿死.

例子：使用AutoResetEvent实现两个线程输出按顺序输出奇数和偶数


### Semaphore 信号量

使用场景：

### Barrier 栅栏

使用场景： 要保持多个线程的多个阶段的进度一致

例子：有四个线程，四个线程全部打印完1之后，打印2，四个线程全部打印完2后，打印3
```csharp
static Barrier barrier = new Barrier(4);
static void BarrierHandle() {
    for (int i = 0; i < 4; i++) {
        Thread thread = new Thread(new ThreadStart(ConsoleValue));
        thread.Start();
    }
}

static void ConsoleValue() {
    Console.WriteLine("线程{0} 打印{1}",Thread.CurrentThread.ManagedThreadId, 1);
    barrier.SignalAndWait();
    Console.WriteLine("线程{0} 打印{1}", Thread.CurrentThread.ManagedThreadId, 2);
    barrier.SignalAndWait();
    Console.WriteLine("线程{0} 打印{1}", Thread.CurrentThread.ManagedThreadId, 3);
}
```


结果

```
线程11 打印1
线程12 打印1
线程13 打印1
线程14 打印1
线程12 打印2
线程13 打印2
线程11 打印2
线程14 打印2
线程14 打印3
线程13 打印3
线程11 打印3
线程12 打印3
```



### Mutex

Mutex是内核态线程同步对象

使用Mutex来判断程序是否已经有实例在运行

```csharp

string ptitle = Assembly.GetExecutingAssembly().GetName().Name;
bool canCreateNew = false;
Mutex mutex = new Mutex(true, @"Global\"+ptitle, out canCreateNew);
```
第二个参数需要使用`Global\`来指定域是全局域，否则只能进行用户态的判断。

当canCreateNew为false，说明已经有一个实例在运行了，`Mutex`对象的生命周期需要和程序运行周期一样，否则`Mutex`被垃圾回收器回收后就失效了



### Interlocked

`Interlocked`是一个原子操作类

原子操作就是不可再拆分的操作，当需要修改一个值时(i = i + 1)，需要从内存中把值读取到缓存区，再从缓存区读取到cpu进行计算，计算完成后写入缓存区中，再写入到内存中，一个累加计算就需要这么多步骤，
当进行多线程操作时候，可能两个线程陆续从cpu中读取到相同的i = 5，进行累计后陆续把i = 6写入到内存，这样就造成了i = 5两次累加，最终值却只是6，而不是预计的7，线程不安全
而原子操作类的累加操作(`Interlocked.Add`)是把这些步骤都合并成一个不可拆分的操作，一步完成，这样的好处就是线程安全的

```csharp
public static class Interlocked
{
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static long Add(ref long location1, long value);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static int Add(ref int location1, int value);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SecuritySafeCritical]
    public static object CompareExchange(ref object location1, object value, object comparand);
    [SecuritySafeCritical]
    public static double CompareExchange(ref double location1, double value, double comparand);
    [SecuritySafeCritical]
    public static float CompareExchange(ref float location1, float value, float comparand);
    [SecuritySafeCritical]
    public static long CompareExchange(ref long location1, long value, long comparand);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SecuritySafeCritical]
    public static IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SecuritySafeCritical]
    public static int CompareExchange(ref int location1, int value, int comparand);
    [ComVisible(false)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SecuritySafeCritical]
    public static T CompareExchange<T>(ref T location1, T value, T comparand) where T : class;
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static int Decrement(ref int location);
    public static long Decrement(ref long location);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SecuritySafeCritical]
    public static object Exchange(ref object location1, object value);
    [SecuritySafeCritical]
    public static double Exchange(ref double location1, double value);
    [SecuritySafeCritical]
    public static float Exchange(ref float location1, float value);
    [SecuritySafeCritical]
    public static long Exchange(ref long location1, long value);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SecuritySafeCritical]
    public static int Exchange(ref int location1, int value);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SecuritySafeCritical]
    public static IntPtr Exchange(ref IntPtr location1, IntPtr value);
    [ComVisible(false)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SecuritySafeCritical]
    public static T Exchange<T>(ref T location1, T value) where T : class;
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static int Increment(ref int location);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static long Increment(ref long location);
    public static void MemoryBarrier();
    public static long Read(ref long location);
}
```
### 线程同步类

在命名空间`System.Collections.Concurrent;`下

### 其他

>SpinWait