# SocketAsyncEventArgs
SocketAsyncEventArgs类是实现socket异步必须的
# SendAsync方法
SendAsync方法会用socket异步发送数据，但是如果连续使用该方法，方法会把几次调用的数据进行拆分发送
比如：

    //客户端发送
    byte[] value1=Encoding.UTF8.GetBytes("123");
    byte[] value2=Encoding.UTF8.GetBytes("456");
    byte[] value3=Encoding.UTF8.GetBytes("789");
    SendAsync(value1);
    SendAsync(value2);
    SendAsync(value3);

    //服务端接收
    value1="12";
    value2="34567";
    value3="89";

这种情况下虽然数据被拆分了，但是数据的顺序没有变化