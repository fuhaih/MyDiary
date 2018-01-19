# Mutex
    互斥体，是.NET中的一个进程互斥锁对象，该对象是整个操作系统可见的

## 常用构成方法
构造函数：
``` csharp
public Mutex(bool initiallyOwned, string name, out bool createdNew);
```
调用：
```csharp
bool createdNew;
Mutex mutex = new Mutex(true, @"Global\MutexSampleApp", out createdNew);
```
在系统范围内，一个名字只能创建一个Mutex对象，如果系统内已经存在一个name 为”Global\MutexSampleApp“的Mutex对象，当尝试创建同名的Mutex对象时，createdNew会返回false

## 用途

* **判断程序是否已经有实例在运行**

    这个用途并没有用到Mutex的互斥性，而是用到了其唯一性，当一个程序实例运行时，创建一个Mutex对象，当第二个程序实例运行时，创建一个同名Mutex对象，createdNew会赋值false，我们就知道程序已经有实例在运行，退出当前运行的实例。
* **进程间的资源互斥**

    这个暂时还没用过

## 注意事项
* **Global和Local前缀**

    例子中在给Mutex命名的字符串里给出了一个“Global\”的前缀。这是因为在运行终端服务（或者远程桌面）的服务器上，已命名的全局 mutex 有两种可见性。如果名称以前缀“Global\”开头，则 mutex 在所有终端服务器会话中均为可见。如果名称以前缀“Local\”开头，则 mutex 仅在创建它的终端服务器会话中可见，在这种情况下，服务器上各个其他终端服务器会话中都可以拥有一个名称相同的独立 mutex。如果创建已命名 mutex 时不指定前缀，则它将采用前缀“Local\”。在终端服务器会话中，只是名称前缀不同的两个 mutex 是独立的 mutex，这两个 mutex 对于终端服务器会话中的所有进程均为可见。即：前缀名称“Global\”和“Local\”仅用来说明 mutex 名称相对于终端服务器会话（而并非相对于进程）的范围。最后需要注意“Global\”和“Local\”是大小写敏感的。
* **Mutex对象**

    Mutex对象最好是一个静态对象，确保其存在于整个程序的生命周期中