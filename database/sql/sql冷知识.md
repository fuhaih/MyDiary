>OUTER APPLY

用在一行拆多行的时候

>ROW_NUMBER

用在分组排序，会根据PARTITION BY的信息进行分组，再根据ORDER BY给每组进行排序，然后给每组排序进行编号。

可以通过这个编号来获取每组的前几条

```sql
ROW_NUMBER() OVER (PARTITION BY A.BED_ID ORDER BY Isnull(A.UP_TIME,'12/31/9999')  asc,A.CREATE_AT ASC) AS RANKS
```