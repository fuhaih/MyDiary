# [官方文档](http://www.rabbitmq.com/dotnet-api-guide.html#consumer-callbacks-and-ordering)
## 创建连接和信道
```csharp
ConnectionFactory factory = new ConnectionFactory();
// "guest"/"guest" by default, limited to localhost connections
//创建连接方式1
factory.UserName = user;
factory.Password = pass;
factory.VirtualHost = vhost;
factory.HostName = hostName;
IConnection conn = factory.CreateConnection();

//创建连接方式2
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = "amqp://user:pass@hostName:port/vhost";
IConnection conn = factory.CreateConnection();
//创建信道
IModel channel = conn.CreateModel();

//关闭信道
channel.Close();
//关闭连接
conn.Close();
```

## 使用交换机和队列
```csharp
//定义交换机
model.ExchangeDeclare(exchangeName, "direct");
//定义队列
model.QueueDeclare(queueName, false, false, false, null);
//通过routingkey绑定交换机和队列
model.QueueBind(queueName, exchangeName, routingKey, null);
```

如果需要队列持久化，队列的第二个参数设置为true，需要注意的是，如果已存在该队列，而且队列配置为不持久化，是不能把其变更为持久化的，这样会报错。

## 发布消息

```csharp
byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
model.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
```
消息发布到交换机中，交换机会找到与routingKey匹配的队列（在交互机与队列绑定时有个routingKey）
## 定义消息
```csharp
byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Hello, world!");

IBasicProperties props = model.CreateBasicProperties();
//定义数据类型
props.ContentType = "text/plain";
//设置消息是否持久化，DeliveryMode = 2会消息持久化
props.DeliveryMode = 2;
//定义消息过期时间
props.Expiration = "36000000"
//定义消息头
props.Headers = new Dictionary<string, object>();
props.Headers.Add("latitude",  51.5252949);
props.Headers.Add("longitude", -0.0905493);

model.BasicPublish(exchangeName,
                   routingKey, props,
                   messageBodyBytes);
```

## 消费消息（手动）
```csharp
bool noAck = false;
BasicGetResult result = channel.BasicGet(queueName, noAck);
if (result == null) {
    // No message available at this time.
} else {
    IBasicProperties props = result.BasicProperties;
    byte[] body = result.Body;
    ...
    ...
    // acknowledge receipt of the message
    channel.BasicAck(result.DeliveryTag, false);
}
```
noAck 设置为false指消息消费完成之后需要Ack确认，消息队列才会删除该消息。如果设置为true，一旦消费者(consumer)获取到消息，那么这个message就会立刻从队列中移除，不管消息有没有成功消费。noAck设置为true会引发一个问题，如果消费者获取到消息之后，在消息消费过程中挂了，那么这个消息没有被成功消费，同时消息队列里也已经没有该消息，该消息就会丢失。

## 消费消息（自动）
```csharp
//公平分发，同一时间只处理一个消息
//在上个消息未交付（ack确认）之前不会再分发消息
channel.BasicQos(0, 1, false);
bool noAck = false;
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (ch, ea) =>
                {
                    var body = ea.Body;
                    // ... process the message
                    channel.BasicAck(ea.DeliveryTag, false);
                };
String consumerTag = channel.BasicConsume(queueName, noAck, consumer);
//取消订阅
channel.BasicCancel(consumerTag);
```
该方式实现消息订阅，消息队列在有消息的时候会自动推送过来，然后通过Received回调函数进行消息消费主体操作。

