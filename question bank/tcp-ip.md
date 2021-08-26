# tcp

>tcp全程

    tramsmission control protocol

>一点疑惑

    tcp连接不需要一问一答，在发送报文量多的时候，会影响效率，http协议就是一问一答的。

>tcp连接四元组

    源ip地址、源端口号、目的ip地址、目的端口号

>TCP控制字符

    SYN 表示建立连接
    FIN 表示关闭连接
    ACK 表示响应
    PSH 表示有DATA数据传输
    RST 表示重置连接
    URG    

    其中,ACK是可能与SYN、FIN等同时使用的，比如说SYN和ACK可能同时为1，它表示的就是请求建立连接后的响应  
    如果只是单个的一个SYN，它表示的是请求建立连接.

>连接三次握手

    1、主动开启者发送一个SYN报文段,并指明自己想要连接的端口号和它的客户端序列号(记为ISN(c))。通常，客户端还会借此发送一个或者多个选项。
    2、服务器在接收到请求后，会把ISN(c)数值加1作为序列号，往客户端发送SYN报文段，并且标记ACK，表示对请求连接的响应。
    3、客户端接收到服务器响应后，再把ISN(c)数值加1作为序列号，往服务端发送ACK报文段。

>关闭四次握手

>半连接状态

>tcp的状态转换

>TIME_WAIT状态

>FIN_WAIT_2状态

>静默时间

>终止一条连接

1、发送FIN信号（有序释放）

FIN是在之前所有排队数据都已经发送后才被发送出去的，而且在FIN_WAIT_2状态可能会处于无限等待，在接收到被动关闭方的ACK后，还会进行WAIT_TIME状态，等待两个MSL时间来确保ACK信息成功发送到另一端。

2、发送重置报文段（终止释放）

可以通过将“逗留于关闭”套接字选项（SO_LINGER）的数值设置为0来实现。这意味着“不会在终止之前为了确定数据是否到达另一端而逗留任何时间”。需要注意的是，重置报文段不会令通讯的另一端做任务的相应，它不会被确认，通讯另一端会造成“连接被另一端重置”的错误提示或者类似的信息。


>tcp/ip五元组

    五元组：源IP地址、目的IP地址、协议号、源端口、目的端口

>发送队列和接受队列

>socket阻塞发送和接收(SocketFlags)

    发送数据时，如果发送队列满，发送方法会阻塞；如果服务端接收队列满，tcp底层不会把发送队列中的数据发送过去。
    接收数据时，如果用户buffer大于接收队列，如果设置flags为MSG_WAITALL（.NET socket估计是默认设置为MSG_WAITALL，因为在SocketFlags中没有看到WAITALL状态。），则会把用户buffer填满，接收方法才会返回。
    
>socket的Close()操作的行为（LingerOption）

|enable|seconds|行为
|--|--|--|
|false （禁用），默认值|不适用，超时值 （默认值）|尝试发送挂起数据的面向连接的套接字 (例如 TCP) 直到默认 IP 协议超时过期。
|true （已启用）|非零值超时|尝试发送挂起的数据，直到指定的超时时间已到，如果该尝试失败，然后 Winsock 重置连接。
|true （已启用）|零个超时时间。|将放弃所有挂起的数据。 对于面向连接的套接字 (例如 TCP)，Winsock 重置连接。

当LingerTime属性存储在LingerState属性设置为大于默认 IP 协议超时时间，则默认 IP 协议超时时间仍将应用，并替代

>tcp滑动窗口

>Nagle算法。


>负载均衡算法
# .net socket

>多路复用(iocp)和完成端口（SocketAsyncEventArgs）

>NetworkStream

>心跳包和Socket.Poll方法



>SendFile方法

    .Net Socket中的SendFile方法在底层使用了TransmitFile API进行文件的高效传输，它允许在套接字连接上发送一个打开的文件，不用把文件信息读入内存再进行传输。不过需要处理好粘包问题。

>SafeFileHandle

# ip地址分类

![ABCDE类ip地址](images/ABCDE类ip地址.png)

# ip地址详细分类

|Address Block                    |Name                              |RFC                       
|--|--|--|
|0.0.0.0/8                        |"This host on this network"       |[RFC1122], section 3.2.1.3
|10.0.0.0/8                       |Private-Use                       |[RFC1918]                 
|100.64.0.0/10                    |Shared Address Space              |[RFC6598]                 
|127.0.0.0/8                      |Loopback                          |[RFC1122], section 3.2.1.3
|169.254.0.0/16                   |Link Local                        |[RFC3927]                 
|172.16.0.0/12                    |Private-Use                       |[RFC1918]                 
|192.0.0.0/24[2]                  |IETF Protocol Assignments         |[RFC6890], section 2.1    
|192.0.0.0/29                     |IPv4 Service Continuity Prefix    |[RFC7335]                 
|192.0.0.8/32                     |IPv4 dummy address                |[RFC7600]                 
|192.0.0.9/32                     |Port Control Protocol Anycast     |[RFC-ietf-pcp-anycast-08] 
|192.0.0.170/32, 192.0.0.171/32   |NAT64/DNS64 Discovery             |[RFC7050], section 2.2    
|192.0.2.0/24                     |Documentation (TEST-NET-1)        |[RFC5737]                 
|192.31.196.0/24                  |AS112-v4                          |[RFC7535]                 
|192.52.193.0/24                  |AMT                               |[RFC7450]                 
|192.88.99.0/24                   |Deprecated (6to4 Relay Anycast)   |[RFC7526]                 
|192.168.0.0/16                   |Private-Use                       |[RFC1918]                 
|192.175.48.0/24                  |Direct Delegation AS112 Service   |[RFC7534]                 
|198.18.0.0/15                    |Benchmarking                      |[RFC2544]                 
|198.51.100.0/24                  |Documentation (TEST-NET-2)        |[RFC5737]                 
|203.0.113.0/24                   |Documentation (TEST-NET-3)        |[RFC5737]                 
|240.0.0.0/4                      |Reserved                          |[RFC1112], section 4      
|255.255.255.255/32               |Limited Broadcast                 |[RFC919], section 7 

# 子网掩码表示

# 可变长度子网掩码




