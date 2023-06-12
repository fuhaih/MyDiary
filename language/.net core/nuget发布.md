## 登录nuget平台

登录[Nuget](https://www.nuget.org/)

申请一个API KEY，Glob pattern配置为*

## nuget.exe

从官网中下载nuget.exe工具，放在c盘某个目录下，并且把锁定去掉(属性那里会有锁定)，把工具的路径配置到环境变量的path中，然后重启vs

vs中调出命令行

```
nuget setApiKey <申请的apikey>
```

命令行上方的小工具栏中项目选中要发布的项目

```
nuget spec
```

生成一个配置文件,格式如下，自动生成的配置项可能较少，需要的其他配置安装下列模板配上，[模板地址](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package#new-file-with-default-values)

```xml
<?xml version="1.0"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
    <metadata>
        <!-- Identifier that must be unique within the hosting gallery -->
        <id>Contoso.Utility.UsefulStuff</id>

        <!-- Package version number that is used when resolving dependencies -->
        <version>1.8.3</version>

        <!-- Authors contain text that appears directly on the gallery -->
        <authors>Dejana Tesic, Rajeev Dey</authors>

        <!-- 
            Owners are typically nuget.org identities that allow gallery
            users to easily find other packages by the same owners.  
        -->
        <owners>dejanatc, rjdey</owners>
        
         <!-- Project URL provides a link for the gallery -->
        <projectUrl>http://github.com/contoso/UsefulStuff</projectUrl>

         <!-- License information is displayed on the gallery -->
        <license type="expression">Apache-2.0</license>
        

        <!-- Icon is used in Visual Studio's package manager UI -->
        <icon>icon.png</icon>

        <!-- 
            If true, this value prompts the user to accept the license when
            installing the package. 
        -->
        <requireLicenseAcceptance>false</requireLicenseAcceptance>

        <!-- Any details about this particular release -->
        <releaseNotes>Bug fixes and performance improvements</releaseNotes>

        <!-- 
            The description can be used in package manager UI. Note that the
            nuget.org gallery uses information you add in the portal. 
        -->
        <description>Core utility functions for web applications</description>

        <!-- Copyright information -->
        <copyright>Copyright ©2016 Contoso Corporation</copyright>

        <!-- Tags appear in the gallery and can be used for tag searches -->
        <tags>web utility http json url parsing</tags>

        <!-- Dependencies are automatically installed when the package is installed -->
        <dependencies>
            <dependency id="Newtonsoft.Json" version="9.0" />
        </dependencies>
    </metadata>

    <!-- A readme.txt to display when the package is installed -->
    <files>
        <file src="readme.txt" target="" />
        <file src="icon.png" target="" />
    </files>
</package>
```

项目实际配置

```xml
<?xml version="1.0" encoding="utf-8"?>
<package >
  <metadata>
    <id>DynamicExpression.Core</id>
    <version>1.0.2</version>
    <title>DynamicExpression.Core</title>
    <authors>fuhaih</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <license type="expression">MIT</license>
    <!-- <icon>icon.png</icon> -->
    <projectUrl>https://github.com/fuhaih/DynamicExpression</projectUrl>
    <description>动态表达式,可以执行表达式字符串，可以合并表达式内容</description>
    <releaseNotes>发行说明，可以是修改了哪些问题</releaseNotes>
    <copyright>$copyright$</copyright>
    <tags>DynamicExpression Expression Lampda Dynamic eval linq</tags>
    <dependencies>
	    <group>
		    <dependency id="Microsoft.CodeAnalysis.CSharp" version="4.2.0" />
	    </group>
    </dependencies>
  </metadata>
</package>
```

**注意**：tags是和搜索相关的，nuget中的搜索会涉及到id、title、tag等

如果项目中引用有其他包，需要配置dependencies

打包
```
nuget pack DynamicExpression.csproj -Prop Configuration=Release -IncludeReferencedProjects
```

Configuration=Release是配置为Release，需要在vs工具栏中把debug更改为release，再重新生成项目，再执行打包操作

IncludeReferencedProjects：如果项目中通过项目引用有其他项目，这个配置会把其他项目一起打包到包里，如果引用的是nuget，在上面的配置文件配置dependencies就行了。

发布
```
nuget push DynamicExpression.Core.1.0.2.nupkg -source nuget.org
```
发布打包好的包

发布的时候需要到官网先查一下包的名称id是否冲突，如果冲突了是会发布失败的。

携带apikey发布

```
nuget push SR.Module.Dashboard.0.0.1.nupkg -source http://nuget.senruisoft.com/ -apikey srnugetpublishkey
```