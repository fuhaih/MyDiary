# Demo
```csharp
//实例化一个发送邮件类。
MailMessage mailMessage = new MailMessage();
//发件人邮箱地址，方法重载不同，可以根据需求自行选择。
mailMessage.From = new MailAddress("2973116040@qq.com");
//收件人邮箱地址。
mailMessage.To.Add(new MailAddress("1154590036@qq.com"));
//邮件标题。
mailMessage.Subject = "发送邮件测试";
//邮件内容。
mailMessage.Body = "<h1>这是我给你发送的第一份邮件哦！</h1>";
//邮件内容是否是html
mailMessage.IsBodyHtml = true;
//实例化一个SmtpClient类。
SmtpClient client = new SmtpClient("smtp.qq.com",587);
//在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
//client.Host = "smtp.qq.com";
//使用安全加密连接。
client.EnableSsl = true;
//不和请求一块发送。
client.UseDefaultCredentials = false;
//验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
client.Credentials = new NetworkCredential("2973116040@qq.com", "awttmyshmkokddci");
//发送
client.Send(mailMessage);
```

# 常见异常
## mail from address must be same as authorization user
    一般出这种情况的原因有两种：

* POP3/IMAP/SMTP未开启。

    这个需要登录邮箱，在设置中开启服务

* 授权码错误。

    这个一般是授权码错误或者是邮箱错误，如上面的Demo，mailMessage.From中的邮箱和client.Credentials中的邮箱需要保持一致，而client.Credentials中的邮箱密码填写的是授权码而不是邮箱的登录密码
## Error: need EHLO and AUTH first !
    一般情况系出现这种错误原因是SmtpClient类的EnableSsl、UseDefaultCredentials属性放到了Credentials属性之下。
    解决办法：将SmtpClient类的EnableSsl、UseDefaultCredentials属性放到了Credentials属性之上。

## 服务器不支持安全连接.
    这个的解决办法是在生成SmtpClient对象的时候添加一个端口号。

# 关于邮件发送上限
[关于edm服务器ip信誉度及发信额度的科普](http://www.youjianqunfa8888.com/youjianqunfa/184.html)

    比如说腾讯的qq邮箱，刚开始用的时候，腾讯分配给给个edm服务器ip（也就是发布邮件发送服务的服务器的ip）的额度一般为100-300每天，如果信誉良好，额度会持续提升，最多有几万条每条，相反，如果经常遭到举报，信誉变差，额度会持续减少。