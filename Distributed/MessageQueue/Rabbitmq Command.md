# 用户管理
rabbitmqctl 只能管理rabbitmq内部的用户数据库，对于其他的后端身份验证的用户都是不可见的。

**add_user**    
rabbitmqctl add_user {username} {password}
**delete_user** 
rabbitmqctl delete_user {username}
