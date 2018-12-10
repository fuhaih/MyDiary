# vhost(virtualhost)
rabbitmq虚拟主机，主要是用来消息隔离的，每个虚拟主机可以创建自己的exchange和queue

# channel
# Exchange（交换机）
## Binding(绑定)
Binding包含了三个元素(ExchangeName,RoutingKey,QueueName)    
RabbitMQ的数据是发送到交换机(Exchange)中的，然后再由交换机发送到队列(Queue)     
Binding就是交换机和队列的关联关系
## 四种交换机类型
>Direct Exchange

将消息中的Routing key与该Exchange关联的所有Binding中的Routing key进行比较，如果相等，则发送到该Binding对应的Queue中。
>Topic Exchange

该类型的Binding的RoutingKey是一个表达式
将消息中的Routing key与该Exchange关联的所有Binding中的Routing key进行对比，如果匹配上了，则发送到该Binding对应的Queue中。
>Fanout Exchange

直接将消息转发到所有binding的对应queue中，这种exchange在路由转发的时候，忽略Routing key。
>Headers Exchange

将消息中的headers与该Exchange相关联的所有Binging中的参数进行匹配，如果匹配上了，则发送到该Binding对应的Queue中。

