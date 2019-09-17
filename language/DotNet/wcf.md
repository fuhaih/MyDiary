# wcf基础
## 终结点（Endpoint）三要素
### 地址
### 契约

>服务契约 
```csharp
[ServiceContract(SessionMode = SessionMode.Required)]
[OperationContract]
```
>数据契约 
```csharp
[DataContract]
[DataMember]
```

>消息契约

>错误契约
### 绑定

>protocolMapping

这个是绑定的默认配置，有时候绑定配置没有用是跟这个有关

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

## 单个客户端在wcf中共享变量

**缺点**:通讯是比较慢
```csharp
//服务行为
[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
//服务契约
[ServiceContract(SessionMode = SessionMode.Required)]
//绑定需要支持session的绑定
netHttpBinding
```

#一个wcf配置实例

```xml
  <system.serviceModel>
    <services>
      <service name="DeclareConfig.Service.DeclareService">
        <endpoint address="" binding="basicHttpBinding" bindingConfiguration="NewBinding0"
          contract="DeclareConfig.Service.IDeclareService" />
      </service>
      <service name="DeclareConfig.Service.Test">
        <endpoint address="" binding="netHttpBinding" bindingConfiguration="NewBinding1"
          contract="DeclareConfig.Service.ITest" />
      </service>
      <service name="DeclareConfig.Service.FactoryService">
        <endpoint address="" binding="basicHttpBinding" bindingConfiguration="NewBinding0"
          contract="DeclareConfig.Service.IFactoryService" />
      </service>
    </services>
    <behaviors>
      <serviceBehaviors>
        <behavior name="">
          <serviceMetadata httpGetEnabled="true" httpsGetEnabled="true" />
          <serviceDebug includeExceptionDetailInFaults="true" />
        </behavior>
      </serviceBehaviors>
    </behaviors>
    <bindings>
      <basicHttpBinding>
        <binding name="NewBinding0" receiveTimeout="01:00:00" sendTimeout="00:10:00"
          maxBufferPoolSize="2147483647" maxBufferSize="2147483647" maxReceivedMessageSize="2147483647" />
      </basicHttpBinding>
      <netHttpBinding>
        <binding name="NewBinding1" receiveTimeout="01:00:00" sendTimeout="00:10:00"
          maxBufferPoolSize="2147483647" maxBufferSize="2147483647" maxReceivedMessageSize="2147483647">
          <reliableSession enabled="true" />
        </binding>
      </netHttpBinding>
    </bindings>
    <protocolMapping>
      <remove scheme="http" />
      <add scheme="http" binding="netHttpBinding" bindingConfiguration="NewBinding1" />
    </protocolMapping>
    <serviceHostingEnvironment aspNetCompatibilityEnabled="true" multipleSiteBindingsEnabled="true"/>
  </system.serviceModel>
```