# wcf基础
## 终结点（Endpoint）三要素
### 地址
### 契约

>服务契约 

>数据契约 

>消息契约

>错误契约
### 绑定

# wcf编程
## wcf 服务实例模式

* 单调服务（Per-Call Service）：每次的客户端请求分配一个新的服务实例。类似于Net Remoting的SingleCall模式；

* 会话服务（Sessionful Service）：则为每次客户端连接分配一个服务实例。类似于Net Remoting的客户端激活模式；

* 单例服务（Singleton Service）：所有的客户端会为所有的连接和激活对象共享一个相同的服务实例。类似于Net Remoting的SingleTon模式。

## wcf 服务并发模式

* Single：一个单一的请求线程能够在某一特定时间访问服务对象

* Reentrant（重入）：一个单一的请求线程能够访问这些服务对象，但线程可以退出这项服务，并重新输入而不会死锁。
当一个服务是回调给客户的时候，重入模式是必要的，除非这个回调的是一个单向操作。因为，客户调用服务并等待响应。客户在其调用线程中受阻，直到调用返回。服务发送一个回调给客户并等待响应。这肯定会发生死锁。设置重入模式，可以避免这种问题，从服务到客户端的调用会返回给服务实例而不会引起死锁。

* Multiple：多个请求线程能够访问这些服务对象和共享资源。

## wcf默认配置

    maxConcurrentCalls ：最大并发数，默认为16 
    maxConcurrentSessions ：最大的会话数，主要针对于PerSession的情况，默认为10 
    maxConcurrentInstances：最大实例数，默认为26
    服务实例模式：默认是会话模式，为每个会话创建一个实例
    服务并发模式：默认Single模式，某一特定时间能治只能执行一个请求，其他请求在请求队列中等待

## 在ServiceBehaviorAttribute中进行配置
```csharp
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,      ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SHEnergyService : ISHEnergyService
    {
        public string DoWork()
        {
            return "test";
        }
    }
```