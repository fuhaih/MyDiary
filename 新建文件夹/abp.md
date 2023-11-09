>本地化

```csharp
/// <summary>
/// 配置国际化
/// </summary>
private void ConfigureLocalization()
{
    Configure<AbpLocalizationOptions>(options =>
    {
        options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
        options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
        options.Languages.Add(new LanguageInfo("en", "en", "English"));
        options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
        options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
        options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
        options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
        options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
        options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
        options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
        options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
        options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
        options.Languages.Add(new LanguageInfo("es", "es", "Español", "es"));
    });
}
```
`SR.SHRDS.Domain.Shared.SHRDSDomainSharedModule`

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpVirtualFileSystemOptions>(options =>
    {
        options.FileSets.AddEmbedded<SHRDSDomainSharedModule>();
    });

    Configure<AbpLocalizationOptions>(options =>
    {
        options.Resources
            .Add<SHRDSResource>("zh-Hans")
            .AddBaseTypes(typeof(AbpValidationResource))
            .AddVirtualJson("/Localization/SHRDS");

        options.DefaultResourceType = typeof(SHRDSResource);
    });

    Configure<AbpExceptionLocalizationOptions>(options =>
    {
        options.MapCodeNamespace("SHRDS", typeof(SHRDSResource));
    });
}
```

配置本地化，然后通过请求头指定

>路由配置

质控项目的路由配置实在`SR.SHRDS.Domain.Shared.SHRDSRouterConfig`里，然后在路由中进行配置

```csharp
/// <summary>
/// 配置控制器生成
/// </summary>
private void ConfigureConventionalControllers()
{     
    Configure<AbpAspNetCoreMvcOptions>(options =>
    {
        options.ConventionalControllers.Create(typeof(SHRDSApplicationModule).Assembly, opts =>
        {
            opts.UrlControllerNameNormalizer = name =>
            {
                //根据控制器名称重新配置路由
                var result = SHRDSRouterConfig.RouterConfig.FirstOrDefault(c => c.Key == name.ControllerName);
                if (!result.Value.IsNullOrWhiteSpace())
                {
                    return result.Value;
                }
                return name.ControllerName;
            };
        });
    });
}
```

>claim信息扩展


```csharp
/// <summary>
/// 拓展Claim信息
/// </summary>
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(AbpUserClaimsPrincipalFactory))] // 替换旧的AbpUserClaimsPrincipalFactory
public class SHRDSClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory, IScopedDependency
{
    private readonly ICenterUserRepository _centerUserRepository;

    public SHRDSClaimsPrincipalFactory(
        UserManager<Volo.Abp.Identity.IdentityUser> userManager,
        RoleManager<Volo.Abp.Identity.IdentityRole> roleManager,
        IOptions<IdentityOptions> options,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory,
        ICenterUserRepository centerUserRepository) :
        base(userManager, roleManager, options, currentPrincipalAccessor, abpClaimsPrincipalFactory)
    {
        _centerUserRepository = centerUserRepository;
    }

    public override async Task<ClaimsPrincipal> CreateAsync(Volo.Abp.Identity.IdentityUser user)
    {
        var principal = await base.CreateAsync(user);
        var identityPrincipal = principal.Identities.First();

        var hospitalRole = await RoleManager.FindByNameAsync(UserRole.医院人员.ToString());

        var userData = await UserManager.FindByIdAsync(user.Id.ToString());
        identityPrincipal.AddClaim(new Claim("user_id", user.ExtraProperties["UserId"].ToString()));

        // 如果是医院人员，加入中心Claim
        if (userData.IsInRole(hospitalRole.Id))
        {
            var centerId = await _centerUserRepository.GetUserCenterIdAsync(user.Id);
            var hopsitalId = await _centerUserRepository.GetUserHospitalIdAsync(user.Id);
            identityPrincipal.AddClaim(new Claim("center_id", centerId.ToString()));
            identityPrincipal.AddClaim(new Claim("hospital_id", hopsitalId.ToString()));
        }
        return principal;
    }
}
```


>质控中心账户添加


>权限相关

这个是abp内置的

* IdentityUserManager 

Abp账户管理，涉及到的表有`AbpUsers`和`AbpUserRoles`

* IdentityRoleManager -> IdentityRole

角色管理，在种子文件中可以通过`IdentityRoleManager`事先配置好角色信息


* PermissionManager 

把权限信息写入`AbpPermissionGrants`表中,包括角色的权限和账户的权限

|name|PrividerName|ProviderKey|
|----|----|----|
|权限名称|R 指角色，U指用户| PrividerName:R 这里就是角色名 PrividerName:U 这里就是账户id

在种子文件中通过PermissionManager事先配置好各个角色的权限信息

* SHRDSPermissionDefinitionProvider

权限配置页面会罗列`SHRDSPermissionDefinitionProvider`中配置好的权限，权限信息列表不会存储在数据库中

* 大致流程

用户登录后，根据jwt获取账户id和角色信息，根据这两个信息到`AbpPermissionGrants`表中获取到权限的名称，和[Authorize()]中配置的权限名称进行对比,判断权限


>IUserRoleStore

>初始化密码

找一个初始密码的账户，把AbpUsers里的PasswordHash 和 SecurityStamp都替换过去
下面是密码例子

AQAAAAEAACcQAAAAELtgftswPhdlagAftm6nT2SwEI/cKCDtQm3O0tiUqrCTDvWwK9alvtbJns+IyDC++Q==

U7MGKDQYUPZQOY3Y2D764YV2A23YS2PV

>IRepository

有使用使用IRepository<,>,发现无法获取到仓储，是因为没有在DBContext中写对应模型类的DBSet<>,添加上后就正常了，但是发现有些模型不用写也可以用，这个有待考察

>防XSRF请求

在跨域访问接口的情况下，这个功能无效

但是在非跨域请求的情况下，请求会自动携带cookie,cookie中会包含有`XSRF-TOKEN`字段，需要获取改token信息，放入到`RequestVerificationToken`请求头中
