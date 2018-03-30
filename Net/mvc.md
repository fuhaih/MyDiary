# Bundle
.net mvc中可以用Bundle来进行静态文件的管理，比如说JavaScript和css文件的压缩等
## 启用Bundle的压缩功能
```csharp
  <system.web>
    <compilation debug="false" targetFramework="4.5.1" />
  </system.web>
```
在调试状态下，JavaScript和css是不会被压缩处理的，在发布的时候，可以把debug设置为诶false，这样mvc网站中的JavaScript和css会经过压缩传递到前台。
## Bundle压缩会出现的问题
在进行压缩之后，css和JavaScript中的文件相对路径会出现异常，Bundle会直接在相对路径前面添加host前缀，所以应该把相对路径都更改为相对于整个项目的路径
```csharp
/Content/.....
```

## TempleData
TempleData可以用作mvc后台跳转传值，原理是利用session存储的值，在跳转后的控制器中获取，TempleData的值只能传递一次，也就是在使用过后就会删除掉。

## mvc的身份验证IPrincipal

# 注意
* webapi的路由不能包含action，所以在设置api方法参数的时候也不要用action作为参数名，否则会出错，无法找到路径