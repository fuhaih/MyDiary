# wcf

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

## wcf中实例的生命周期
* PreCall方式

    1）客户端创建代理对象（Proxy）

    2）客户端调用代理对象的一个契约操作，代理对象将其传递给宿主程序

    3）服务应用程序创建一个新的服务契约对象，并执行请求的操作

    4）在执行完请求的操作后，如果要求有应答，那么服务契约会给代理对象一个应答，然后销毁自己（如果实现了IDisposable，则调用Disposs函数）

* PreSession方式

    1）客户端创建代理对象（Proxy）

    2）客户端第一次调用代理对象的一个契约操作，代理对象将其传递给宿主程序

    3）宿主程序创建新的服务对象，并执行请求操作，如果有必要，返回客户端应答。

    4）客户端再次发出调用服操作的请求，宿主会先判断是否已有建立好的会话，如果存在，则不需要再创建新的服务对象，直接使用老对象即可

    5）在时间达到指定要求或者因一些特殊原因，会话会过期，此时服务对象销毁

* Single方式

    1）服务端启动，同时创建服务对象

    2）客户端通过代理调用契约操作

    3）第一步中创建的服务对象接受请求，并执行操作，进行必要的应答

    4）第一步创建的服务对象将一直保留

    5）服务关闭，第一步创建的对象销毁

    **注意**：Single方式下，如果服务是寄宿在iis服务器上，那么服务长时间没有被访问时，iis会回收服务对象，这个可以在iis中设置。

## 关于wcf单例模式
在iis中寄宿wcf的时候，服务对象会在服务启动的时候就创建，如果wcf长期没有被访问，iis就会回收wcf服务对象，在下次启用的时候才重新创建，所以如果wcf采用的是单例模式，可以考虑让iis不回收服务对象，这个可以在应用程序池中设置，选中应用程序池->高级设置->进程模型中的闲置超时和回收中的固定时间间隔都设置为0
