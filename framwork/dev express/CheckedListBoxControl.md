## 数据绑定


>常用方式
```csharp
//DataSource可以是DataTable或者是IEnumerable<T>
checkedListBoxControl.DataSource = sourcelist;
checkedListBoxControl.DisplayMember = "name";
checkedListBoxControl.ValueMember = "id";

```

>用items绑定

```csharp
box.Items.Clear();
foreach (var item in corpus)
{
    CheckState itemChecked = CheckState.Unchecked;
    if(CorpusID.Contains(item._id))  
    {
        itemChecked = CheckState.Checked;
    }
    //这里第二个参数是displayName，需要指定
    box.Items.Add(item,item.Name,itemChecked,true);
}
//使用Items来添加数据源时，这两个参数好像没有作用。
box.DisplayMember = "Name";
box.ValueMember = "_id";
```