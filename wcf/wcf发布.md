# 发布异常
## HTTP 错误 404.3 - Not Found
    在iis中添加.svc类型的MIME映射，扩展名“.svc”，MIME类型 “application/octet-stream”

##  未进行wcf服务的http激活
出现的异常现象是：用浏览器打开服务的时候（[http://localhost/Service1.svc](http://localhost/Service1.svc)），会出现Service1.svc文件的下载提示。

    控制面板->程序和功能->启用或关闭windows功能->.Net Framework 4.0(wcf程序版本或更高版本)->WCF服务->HTTP激活，勾选了http激活之后就确认安装。

## 终节点异常
异常信息：如果在配置中将“system.serviceModel/serviceHostingEnvironment/multipleSiteBindingsEnabled”设置为 true，则需要终结点指定相对地址。如果在终结点上指定相对侦听 URI，则该地址可以是绝对地址。若要解决此问题，请为终结点"http://localhost/Service1.svc"指定相对 URI。

解决方案：配置文件如下，其中endpoint的address不能指定路径
```xml
<system.serviceModel>
    <services>
      <service name="LandMark.Wcf.LandMarkService">
        <endpoint address=""
          binding="basicHttpBinding" bindingConfiguration="" contract="LandMark.Wcf.ILandMarkService" />
      </service>
    </services>
</system.serviceModel>  
```

## 当前已禁用此服务的元数据发布
配置文件
```xml
 <system.serviceModel>
    <behaviors>
      <serviceBehaviors>
        <behavior name="">
          <serviceMetadata httpGetEnabled="true" />
          <serviceDebug includeExceptionDetailInFaults="false" />
        </behavior>
      </serviceBehaviors>
 </behaviors>
  </system.serviceModel>  
```