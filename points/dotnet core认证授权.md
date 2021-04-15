## ClaimsPrincipal

在dotnet core中，无论哪种认证，都会解析为`ClaimsPrincipal`对象，然后赋值给HttpContext.User

>cookie 

cookie认证也是构建一个`ClaimsPrincipal`对象，然后使用HttpContext.SignInAsync方法来进行登录操作，HttpContext.SignOutAsync进行登出操作

```csharp
/// <summary>
/// Signin
/// </summary>
/// <param name="parameter"></param>
/// <returns></returns>
public async Task<IActionResult> Signin(LoginParameter parameter)
{
    var userClaims = new List<Claim>(){
      new Claim(ClaimTypes.Name, parameter.User),
      new Claim(ClaimTypes.Email, "anet@test.com"),
      new Claim(ClaimTypes.Role, "admin"),
    };
    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
    await HttpContext.SignInAsync(userPrincipal);
    return Redirect(parameter.ReturnUrl);
}
```

>jwt

jwt生成

这里使用到了`IdentityModel`类库，封装了jwt生成的方法

```csharp
[HttpPost]
[Route("signin")]
[AllowAnonymous]
public async Task<AjaxResult<Token>> Signin([FromForm] UserInfo user)
{
    
    await Task.Yield();
    string pwd = user.Pwd.ToMD5().HexEecode().ToUpper();
    bool login = await dataContent.LoginAsync(user.Name, pwd);
    if(!login) return AjaxResult<Token>.Failt();
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

    return AjaxResult<Token>.Success(tokenResult);
}
```

**下面这句暂时保留意见** 

解析的时候会直接解析出jwt的payload信息，jwt的payload信息其实也是类似于`Claim`的，所以可以直接用payload信息构建出`ClaimsPrincipal`对象

>授权

使用Authorize特性进行认证授权，会匹配ClaimsPrincipal对象的名称为Role的Claim对象，符合Authorize中的Roles则能够访问，Policy类似于Roles的组合，把某几个Roles组合为一个Policy，这样就能方便授权

```csharp
[Authorize(Roles = "admin",Policy ="")]
public class AccountController : ControllerBase
{

}
```

>配置

```csharp

var jwtconfigSection = Configuration.GetSection("JwtSetting");
JwtSetting setting = jwtconfigSection.Get<JwtSetting>();
// cookie认证
services.AddAuthentication("Cookies")
    .AddCookie("Cookies",options=> { 
    });
//jwt认证
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = setting.Issuer,
            ValidAudience = setting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecurityKey)),
            ClockSkew = TimeSpan.Zero
        };
});
services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("admin", "teacher").Build());
});
```

`AddAuthentication` 是添加身份认证，也就是认证身份是否合法

`AddAuthorization` 是添加授权信息，也就是判断身份是否有权限

`AddAuthentication` 方法会设置defaultScheme，然后从根据Scheme查找对应的`IAuthenticationHandler`,进行身份认证

AddAuthentication配置defaultScheme时，不能同时设置两种认证方式，只有后面的认证有效，如果想要使用两种认证方式，也可以使用特性来指定Scheme


```csharp
[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
public ActionResult Index()
{
    return Ok();
}
```

## 自定义认证

IAuthenticationHandler，IAuthorizationService