>除非另外还指定了 TOP、OFFSET 或 FOR XML，否则，ORDER BY 子句在视图、内联函数、派生表、子查询和公用表表达式中无效。

嵌套查询的时候不能在内联函数中使用order by，当使用top等关键字的时候才会使用order by