# 数据绑定
## 简单绑定

```csharp
DataTable value;
gridControlAccount.DataSource =value;
```
## 主从表
* 定义主从表视图    
主表视图就用默认的GridView
从表视图是添加一个Level，在Level中新建一个视图
![Master-Detail](Master-Detail.jpg)
* 数据绑定(通过DataRelation)

```csharp
DataSet dsAB ;
DataRelation dRelation = new DataRelation("AccountRelation", dsAB.Tables[0].Columns["F_AccountNumber"], dsAB.Tables[1].Columns["F_AccountNumber"]);
dsAB.Tables[0].ChildRelations.Add(dRelation);
gridControlAccount.DataSource = dsAB.Tables[0];
//给gridViewRelate创建列，以便能自定义各个列
//gridViewRelate是从表视图，主表视图在数据绑定的时候已经创建好列了
gridViewRelate.PopulateColumns(dsAB.Tables[1]);
```
**注意：**在数据绑定的时候，DataRelation的名称要和新建的Level名称一样
## 列合并

## 行下表
## 分组

# 常用属性配置
