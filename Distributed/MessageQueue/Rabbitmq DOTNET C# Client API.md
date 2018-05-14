
## 创建连接和通道
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
//创建通道
IModel channel = conn.CreateModel();
//关闭通道
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
noAck 设置为false指消息消费完成之后需要Ack确认，消息队列才会删除该消息

## 消费消息（自动）
```csharp
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

