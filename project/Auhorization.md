# cookie-session

这个是最常用的授权方式，当设置好session后，后台会返回带有sessionid的Set-Cookie头，前端浏览器就根据Set-Cookie来设置cookie，当再次请求时，就会把cookie带上，后台根据cookie里的sessionid获取到session，得到登录信息。

.net core使用cookie-session登录示例

>登录

```csharp
var userClaims = new List<Claim>(){
              new Claim(ClaimTypes.Name, parameter.User),
              new Claim(ClaimTypes.Email, "anet@test.com"),
              new Claim(ClaimTypes.Role, "admin"),
            };
var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
await HttpContext.SignInAsync(userPrincipal);
```

验证方式：在控制器或者方法上添加`[Authorize(Roles ="admin")]`注解


>优点

相对安全，因为sessionid存储在cookie里，设置cookie为httponly，可以有效的防止一些XSS攻击。

设置cookie的Secure为true，则cookie只会在https访问时携带。

>缺点

安全性高的同时，灵活性相对较低，不过对于外网访问的站点，推荐用cookie-session




# Bearer

## jwt

常用的是jwt，全称JavaScript Web Token

.net core使用例子

>引用

```
IdentityModel
Microsoft.AspNetCore.Authentication.JwtBearer
```

>配置

配置文件
```json
  "JwtSetting": {
    "SecurityKey": "d0ecd23c-dfdb-4005-a2ea-0fea210c858a", // 密钥
    "Issuer": "jwtIssuertest", // 颁发者
    "Audience": "jwtAudiencetest", // 接收者
    "ExpireSeconds": 100000 // 过期时间（s）
  }
```

添加jwt认证到服务中
```csharp

var jwtconfigSection = Configuration.GetSection("JwtSetting");
JwtSetting setting = jwtconfigSection.Get<JwtSetting>();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = setting.Issuer,
            ValidAudience = setting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecurityKey)),
            ClockSkew = TimeSpan.Zero
        };
});
app.UseAuthentication();//认证 
```

>登录

登录时候先验证，验证登录信息正确后，创建jwt token返回到前端

```csharp
var jwtSection = _config.GetSection("JwtSetting");
JwtSetting _jwtSetting = jwtSection.Get<JwtSetting>();
//创建用户身份标识，可按需要添加更多信息
var claims = new Claim[]
{
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //new Claim("id", user.Id.ToString(), ClaimValueTypes.Integer32), // 用户id
    new Claim(JwtClaimTypes.Name, user.Name), // 用户名
    new Claim(JwtClaimTypes.Role, "admin") // 是否是管理员
};
byte[] key = Encoding.UTF8.GetBytes(_jwtSetting.SecurityKey);
//创建令牌
var token = new JwtSecurityToken(
  issuer: _jwtSetting.Issuer,
  audience: _jwtSetting.Audience,
  signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
  claims: claims,
  notBefore: DateTime.Now,
  expires: DateTime.Now.AddSeconds(_jwtSetting.ExpireSeconds)
);

string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

Token tokenResult = new Token
{
    access_token = jwtToken,
    expires_in = _jwtSetting.ExpireSeconds * 60,
    token_type = "Bearer"
};

```
>前端
添加请求头
`Authorization: Bearer token`

>认证

1、通过Role来认证权限

在控制器上添加`[Authorize(Roles = "admin")]`注解，那么Roles为admin的用户可以访问该控制器下api。
多个权限可以用逗号隔开，`[Authorize(Roles = "admin,teacher")]`

2、通过Policy来认证权限

配置policy
```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("teacher", policy => policy.RequireRole("admin","teacher").Build());
});
```

在控制器上添加`[Authorize(Policy = "teacher")]`注解，那么Roles为admin或者teacher的用户都可以访问该控制器下api，相当于用户组。

>优点

1、无状态，可以多端使用，有些客户端不方便使用cookie-session。
2、方便跨域，cookie-session跨域相对比较烦

>缺点

1、一般存储在localstore 里，容易受到XSS攻击，如果放在cookie里，设置httponly，那就没办法使用`Authorization`请求头来传送token，就改成了使用cookie来传送token了，那这样的方式也就跟直接使用cookie-session没什么区别了。

2、过期不好处理，因为无状态，没办法直接使得token失效。

>最佳使用场景

登录认证这样要保持登录状态的场景不太适合jwt的使用，还是建议使用cookie-session。下面场景比较适合jwt

1、邮箱验证

用户注册时发送邮件，点击邮件链接时携带上jwt token，jwt过期时，链接也就失效。

# Basic

>basic认证

basic认证是使用用户名和密码进行认证，使用用户名和密码构造成`username:password`格式的字符串，然后转换为base64字符串作为token

>使用

创建token
```csharp
byte[] authBytes = Encoding.UTF8.GetBytes($"{Username}:{Password}");
string authToken = Convert.ToBase64String(authBytes);
```

请求上添加`Authorization: Basic token`

>优点

比较简单

>缺点

需要前端记住用户名密码，token是base64字符串，可逆，这样用户名密码容易暴露，比较适用的场景是后台api调用，比如说rabbitmq management调用http api时就是用的该认证方式。

# OAuth 2

第三方登录就是使用的OAuth认证

# 单点登录

>优点

可以解决多个站点重复登录问题

>缺点

不适用于前后端分离项目，认证过期不好处理。