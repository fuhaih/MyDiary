# RPC(Remote Procedure Call Protocol)

RPC是远程过程调用，概念比较宽泛，也就是调用远程电脑上的应用的方法

这么解释的话其实http协议的一些应用(webapi、webservice)也算是rpc

RPC是一个宽泛的概念，具体实现的话需要有两个部分，一个是通讯方式(tcp、http1.1 http2、udp、webservice)，一个是数据结构(json、xml、protobuf、TCompactProtocol(这个是Thrift框架用的数据协议)、Msgpack )

下面是一些常见的rpc实现

## grpc

使用的是protobuf格式来序列化数据，相比json和xml等格式，体积更小，更有利于传输，能减少网络传输时间

通讯用的是http2协议或者是tcp协议，http2相比于http1.1和webservice等协议，也是体积更小，直接使用tcp就更不用说，http1.1、http2、webservice(基于http)都是基于tcp传输的，grpc直接用tcp传输会更加轻量。

所以grpc无论是从通讯协议上，还是数据结构上，都是小体积的，方便传输