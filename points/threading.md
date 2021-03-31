> 什么是线程安全

累加器例子

ConcurrentDictionary GetOrAdd 方法

```
public TValue GetOrAdd (TKey key, Func<TKey,TValue> valueFactory);
```

GetOrAdd 中的回调函数valueFactory是不在锁内执行的，所以当多个线程使用这个方法的时候，可能会执行多次，但是最终新增到字典中的值只有一个，所以多个线程获取到的对象是同一个对象。这种情况下也是线程安全的，获取到的TValue是预期结果，每个线程获取到的都是同一个对象