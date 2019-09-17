# 一些刁钻的用法

[相关博客](http://www.datatables.club/blog/)
>重新绑定数据
```javascript
if ($('#table_id_example').hasClass('dataTable')) {
    dttable = $('#table_id_example').dataTable();
    dttable.fnClearTable(); //清空一下table
    dttable.fnDestroy(); //还原初始化了的datatable
}
$("#table_id_example").find("thead").html(thead);
$("#table_id_example").find("tbody").html(html);
$("#table_id_example").DataTable()
```
>汉化
```javascript
(function (DataTable){
$.extend(true, DataTable.defaults, {
    oLanguage: {
        oPaginate: {
            sFirst: "首页",
            sLast: "尾页",
            sNext: "下一页",
            sPrevious: "上一页"
        },
        sEmptyTable: "无数据",
        sInfo: "显示第_START_到第_END_行数据（共_TOTAL_行）",
        sInfoEmpty: "显示第0到第0行数据（共0行）",
        sLengthMenu: "显示_MENU_行",
        sLoadingRecords: "数据加载中.......",
        sProcessing: "正在处理数据......",
        sSearch: "搜索"
    }
});
})(jQuery.fn.dataTable)

```

>页码和检索
```javascript
//获取页码
var pageinfo = $("#table_id_example").DataTable().page.info();
var page=pageinfo.page;
//获取检索关键字
var keyword= $("#table_id_example_filter").find("input").val();
//初始化时进行检索和页码跳转
var oTable = $("#table_id_example").DataTable(); 
oTable.search(keyword||"").page(page || 0).draw(false)
```