## JWT

nuget安装`IdentityModel`和`Microsoft.AspNetCore.Authentication.JwtBearer`

>添加配置

```json
"JwtSetting": {
  "SecurityKey": "d0ecd23c-dfdb-4005-a2ea-0fea210c858a", // 密钥
  "Issuer": "jwtIssuertest", // 颁发者
  "Audience": "jwtAudiencetest", // 接收者
  "ExpireSeconds": 200000 // 过期时间（200000s）
}
```

> 修改Startup

```csharp
var jwtconfigSection = Configuration.GetSection("JwtSetting");
JwtSetting setting = jwtconfigSection.Get<JwtSetting>();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = setting.Issuer,
            ValidAudience = setting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecurityKey)),
            ClockSkew = TimeSpan.Zero//过期缓冲时间（token真正过期时间是token有效时间+过期缓冲时间）
        };
    });

```

>登录接口
```csharp
[HttpPost]
[AllowAnonymous]
public async Task<Token> Login([FromForm]UserInfo user)
{
    await Task.Yield();
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
    return new Token {
        access_token = jwtToken,
        expires_in = _jwtSetting.ExpireSeconds * 60,
        token_type = "Bearer"
    };
}
```

```csharp
app.UseAuthentication();//认证 

app.UseAuthorization();//授权
```

>认证

```csharp
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "admin")]
//[Authorize(Policy = "admin")]
//[Authorize(Roles ="admin,teacher")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var user = User;
        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
```
通过`Authorize`特性来认证授权，`Authorize(Roles = "admin")`会判断Claims中的role是否包含admin权限。

在进行请求时，加上Authorization请求头就行了     
Authorization: Bearer [access_token]


>批量授权
* `[Authorize(Roles = "admin,teacher")]`

* 使用Policy

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("admin_policy", policy => policy.RequireRole("admin","teacher").Build());
});
```

然后使用`[Authorize(Policy = "admin_policy")]`

>大致流程

浏览器访问login方法获取access_token,后续访问接口时在请求头Authorization中带入access_token，后台解析token获取用户信息(包括role等)，判断access_token是否过期，再通过用户信息来判断认证和权限。jwt和cookie不一样，access_token能解析出用户信息，无需后端存储用户信息，所以即便是后台重启过之后，access_token也不会失效，只有它过期之后才会失效。

## Cookie

```csharp
var userClaims = new List<Claim>()
{
  new Claim(ClaimTypes.Name, user.UserName),
  new Claim(ClaimTypes.Email, "anet@test.com"),
};

var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
HttpContext.SignInAsync(userPrincipal);
```

`SignInManager<IdentityUser>`   
`HttpContext.SignInAsync`

## OAuth2

>QQ 微信

## 单点登录
>IdentityServer4
