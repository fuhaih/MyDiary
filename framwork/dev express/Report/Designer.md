>表格边框

表格比较常见的需求是表格边框，如果每个表格都设置上下边框，那么边框会重合

解决方案：

```
GroupHeader
    ----table_header
Detail
    ----table
GroupFooter
    ----table_footer
```

table里只设置左右边框，上边框在GroupHeader里设置，table_header也设置在GroupHeader中，GroupUnion设置为With First Detail，然后设置Repeat Every Page，确保分页的时候每页都有，下边框同理，使用GroupFooter设置。GroupHeader和GroupFooter都可以设置多个，按照需求使用。

>表格标题放在表格左边

这个可以使用GroupHeader设置，GroupHeader配置为Print Across Bands 

例子:
[Product List](https://demos.devexpress.com/ASPNetCore/Demo/Reporting/ProductListReport/NetCore/Light/)
