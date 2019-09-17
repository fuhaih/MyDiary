# 查看 .net core已安装版本

```sh
dotnet --list-sdks
```
# 需要注意的地方

.net core版本需要注意的有两个地方，   

## **第一是项目指定的 .net core版本。**

可以在项目csproj中指定。    
`<TargetFramework>netcoreapp2.1</TargetFramework>`    
需要注意的是，不同版本的csproj配置不太一样，2.1中需要PackageReference来引用`Microsoft.AspNetCore.App`包，而3.0中不用，而且不同版本的Startup和Program也有差异，所以升级或者降级 .net core项目版本还是比较麻烦的。

## **第二是 .net core运行环境版本(也就是说dotnet指令用哪个版本)。**

这个是通过[global.json](https://docs.microsoft.com/zh-cn/dotnet/core/tools/global-json)文件来进行配置   
比如说需要创建一个 .net core 2.1版本的项目，但是还安装有3.0的 .net core sdk，那么运行dotnet命令时默认是使用3.0版本的sdk，这时候可以查看.net core 已安装版本，找到对应版本创建global.json文件

```powershell
PS D:\fuhai\demo\testcore> dotnet --version
# dotnet 指令默认使用最新版本
3.0.100-preview8-013656
PS D:\fuhai\demo\testcore> dotnet --list-sdks
1.0.4 [C:\Program Files\dotnet\sdk]
2.1.202 [C:\Program Files\dotnet\sdk]
2.1.508 [C:\Program Files\dotnet\sdk]
3.0.100-preview8-013656 [C:\Program Files\dotnet\sdk]
```

然后创建2.1版本的global.json文件

```powershell
dotnet new globaljson --sdk-version 2.1.508
The template "global.json file" was created successfully.
```

这时候当前目录下生成一个global.json文件
```json
{
  "sdk": {
    "version": "2.1.508"
  }
}
```
再查看dotnet版本
```powershell
PS D:\fuhai\demo\testcore> dotnet --version
2.1.508
```
这时候`dotnet`指令就是使用的`2.1.508`版本
使用`dotnet`指令生成的项目就是2.1版本
```powershell
PS D:\fuhai\demo\testcore> dotnet new webapi -o coredemo
```
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp2.1</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Folder Include="wwwroot\" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.App" />
    <PackageReference Include="Microsoft.AspNetCore.Razor.Design" Version="2.1.2" PrivateAssets="All" />
  </ItemGroup>

</Project>
```

# 总结
不同的 .net core项目下，最好都要创建一个global.json文件来指定dotnet指令的运行版本，避免多个 .net core版本的项目出现异常。

# 版本升级

>查看版本

`dotnet --list-sdks`


>创建global.json文件 (dotnet命令版本号问题)

`dotnet new globaljson --sdk-version 版本号`

>修改csproj文件 (dll引用问题)

主要修改版本号。可以创建一个空的目标版本的项目对应着修改

>修改Program.cs和Startup.cs (项目架构问题)

这个也是创建一个空的目标版本的项目来对照着修改

进行上面四个步骤的修改后，应该就能以新版本的net core框架来运行项目了。

