## sql server关键字

use,declare,select,top,from,where,group,having,order by ,aes,dec，case when xxx then xxx when xxx then xxx else xxx end,and,or,
between and,Distinct,Join ... on ,INNER JOIN , LEFT JOIN,RIGHT JOIN,as (Alias),Insert Into,Update,Delete,Create Table,Alter Table,Drop Table,

## 查询子句

```sql
SELECT select_list 
[ INTO new_table ] 
FROM table_source 
[ WHERE search_condition ] 
[ GROUP BY group_by_expression ] 
[ HAVING search_condition ] 
[ ORDER BY order_expression [ ASC | DESC ] ] 
```

table_source 可以是表名，也可以是子查询,

## 拆解

### condition

条件，表达式 


## 一些例子

```csharp
public class SqlGrammar : Irony.Parsing.Grammar
{
    public SqlGrammar() : base(false)
    {
        var number = new NumberLiteral("number");
        var id = new IdentifierTerminal("id");

        var selectStatement = new NonTerminal("SelectStatement");
        var fromClause = new NonTerminal("FromClause");
        var whereClause = new NonTerminal("WhereClause");

        selectStatement.Rule = "SELECT" + id + "FROM" + fromClause + whereClause;
        fromClause.Rule = "FROM" + id;
        whereClause.Rule = Empty | "WHERE" + id + "=" + number;

        this.Root = selectStatement;
    }
}
```

```csharp
static void Main(string[] args)
{
    Parser parser = new Parser();
    string input = "SELECT name FROM products WHERE price = 10";
    ParseTree tree = parser.Parse(input);

    if (tree.Status == ParseTreeStatus.Error)
    {
        Console.WriteLine("语法错误：{0}", tree.ParserMessages[0].Message);
    }
    else
    {
        // 处理抽象语法树（AST）...
    }
}

```


```csharp
var joinType = new NonTerminal("JoinType");
var joinClause = new NonTerminal("JoinClause");
var onClause = new NonTerminal("OnClause");

joinType.Rule = ToTerm("INNER") | ToTerm("LEFT") | ToTerm("RIGHT") | ToTerm("FULL") + Optional(ToTerm("OUTER"));
joinClause.Rule = joinType + "JOIN" + id + "ON" + onClause;
onClause.Rule = id + "=" + id;

selectStatement.Rule = "SELECT" + id + "FROM" + fromClause + Optional(joinClause) + Optional(whereClause);

```

```csharp
var caseExpr = new NonTerminal("CaseExpr");
var whenClause = new NonTerminal("WhenClause");
var elseClause = new NonTerminal("ElseClause");

caseExpr.Rule = "CASE" + whenClause + Optional(elseClause) + "END";
whenClause.Rule = MakePlusRule(whenClause, null, 
    ToTerm("WHEN") + id + "THEN" + number);
elseClause.Rule = "ELSE" + number;

selectStatement.Rule = "SELECT" + caseExpr + id + "FROM" + fromClause + Optional(joinClause) + Optional(whereClause);

```

```csharp
var subQuery = new NonTerminal("SubQuery");
subQuery.Rule = "(" + selectStatement + ")";

fromClause.Rule = "FROM" + (id | subQuery);

selectStatement.Rule = "SELECT" + id + "FROM" + fromClause + Optional(joinClause) + Optional(whereClause);

```

```csharp
var rowNumberExpr = new NonTerminal("RowNumberExpr");
rowNumberExpr.Rule = "ROW_NUMBER()" + "OVER" + "(" + orderByClause + ")";

selectStatement.Rule = "SELECT" + rowNumberExpr + "," + id + "FROM" + fromClause + Optional(joinClause) + Optional(whereClause);

```

```csharp
using Irony.Parsing;

public class SqlGrammar : Grammar
{
    public SqlGrammar()
    {
        // 定义终结符和非终结
        var selectStmt = new NonTerminal("SelectStmt");
        var selectKeyword = ToTerm("SELECT");
        var fromKeyword = ToTerm("FROM");
        var identifier = new IdentifierTerminal("Identifier");

        // 定义语法规则
        selectStmt.Rule = selectKeyword + identifier + fromKeyword + identifier;

        设置根规则
        this.Root = selectStmt;
    }
}

```

```csharp
csharp
using Irony.Parsing;

public class SqlGrammar : Grammar
{
    public SqlGrammar()
    {
        // 定义终结符和非终结符
        var selectStmt = new NonTerminal("SelectStmt");
        var selectKeyword = ToTerm("SELECT");
        var fromKeyword = ToTerm("FROM");
        var identifier = new IdentifierTerminal("Identifier");

        // 新增嵌套查询规则
        var subquery = new NonTerminal("Subquery");
        var leftParenthesis = ToTerm("(");
        var rightParenthesis = ToTerm(")");

        // 定义语法规则
        selectStmt.Rule = selectKeyword + identifier + fromKeyword + (identifier | subquery);
        subquery.Rule = leftParenthesis + selectStmt + rightParenthesis;

        // 设置根规则
        this.Root = selectStmt;
    }
}

```

```csharp
using Irony.Parsing;

public class SqlGrammar : Grammar
{
    public SqlGrammar()
    {
        // 定义终结符和非终结
        var selectStmt = new NonTerminal("SelectStmt");
        var selectKeyword = ToTerm("SELECT");
        var fromKeyword = ToTerm("FROM");
        var joinKeyword = ToTerm("JOIN");
        var onKeyword = ToTermON");
        var groupByKeyword = ToTerm("GROUP BY");
        var whereKeyword = ToTerm("WHERE");
        var identifier = new IdentifierTerminal("Identifier");
        var condition = new NonTerminal("Condition");

        // 新增 JOIN 规则
        var joinClause = new NonTerminal("JoinClause");
        var joinTable = new NonTerminal("JoinTable");

        // 新增 GROUP BY 和 WHERE 规则
        var groupByClause = new NonTerminal("GroupByClause");
        var whereClause = new NonTerminal("WhereClause");

        // 定义语法规
        selectStmt.Rule = selectKeyword + identifier + fromKeyword + (identifier | joinClause) +
                          groupByClause + whereClause;
        joinClause.Rule = joinKeyword + identifier + joinTable + onKeyword + condition;
        joinTable.Rule = identifier + onKeyword + condition;
        groupByClause.Rule = groupByKeyword + identifier;
        whereClause.Rule = whereKeyword + condition;

        condition.Rule = identifier + "=" + identifier; 这里只是一个简单的条件规，你可以根据需要展它

        设置根规则
        this.Root = selectStmt;
    }
}
```

```csharp
var grammar = new Grammar();

// Define variable rule
var variableRule = new NonTerminal("Variable");
variableRule.Rule =Term("@") + Identifier;

// Define table variable rule
var tableVariableRule = new NonTerminal("TableVariable");
tableVariableRule.Rule = ToTerm("#") + Identifier;

// Add rules to the grammar
grammar.Root = variableRule | tableVariableRule;
```

```csharp
using Irony.Parsing;
using System;

namespace SQLParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var grammar = new SqlGrammar();
            var parser = new Parser(grammar);

            while (true)
            {
                Console.Write("Enter a SQL statement: ");
                var input = Console.ReadLine();

                var parseTree = parser.Parse(input);
                if (parseTree.HasErrors())
                {
                    Console.WriteLine("Invalid SQL statement.");
                    continue;
                }

                // Process the parse tree and execute the SQL statement
                // ...

                Console.WriteLine("SQL statement parsed successfully.");
            }
        }
    }

    public class SqlGrammar : Grammar
    {
        public SqlGrammar() : base(caseSensitive: false)
        {
            // Terminals
            var select = ToTerm("SELECT", "select");
            var from = ToTerm("FROM", "from");
            var where = ToTerm("WHERE", "where");
            var groupBy =Term("GROUP BY "groupBy");
            var orderBy = ToTermORDER BY", "orderBy");
            var identifier = new IdentifierTerminal("identifier");
            var variable = new IdentifierTerminal("variable");

            // Non-terminals
            var selectStatement = new NonTerminal("selectStatement");
            var columnList = new NonTerminal("columnList");
            var tableList = new NonTerminal("tableList");
            var whereClause = new NonTerminal("whereClause");
            var groupByClause = new NonTerminal("groupByClause");
            var orderByClause = new NonTerminal("orderByClause");

            // Productions
            selectStatement.Rule = select + columnList + from + tableList + whereClause + groupByClause + orderByClause;
            columnList.Rule = MakePlusRule(columnList, ToTerm(","), identifier);
            tableList.Rule = MakePlusRule(tableList, ToTerm(","), identifier);
            whereClause.Rule = Empty | where + identifier + "=" + variable;
            groupByClause.Rule = Empty | groupBy + identifier;
            orderByClause.Rule = Empty | orderBy + identifier;

            // Set the root rule
            this.Root = selectStatement;

            // Mark punctuation and transient terminals
            MarkPunctuation(select, from, where, groupBy, orderBy, ",", "=");
            MarkTransient(columnList, tableList, whereClause, groupByClause, orderByClause);

            // Register the non-terminals for error reporting (optional)
            RegisterBracePair(", ")");
            RegisterBracePair("[", "]");
            RegisterBracePair("{", "}");
            MarkReservedWords(select, from, where, groupBy, orderBy);
        }
    }
}
```

# sql支持

## sql

> ConditionExpr 条件表达式

(>,<,>=,<=,=,in between) 

in 后面可以跟子查询

in (select name from xxx)

> 目标表达式

> 其他


## 表达式拆分

> 变量表达式

declare @xxx xxx;

> 赋值表达式

赋值可以是变量，也可以是列，update里面

set @xxx = xxx,@xxx = xxx;

> 列表达式

*，列名，函数，逻辑运算，方法，数值，日期,参数等，还可以是子查询

> 集合对象表达式

表名，视图，表变量，临时表等

> 目标表达式

集合对象表达式|查询表达式|union表达式

> 条件表达式

condition and|or condition

condition = 列表达式 (>,<,>=,<=,=,in between) 列表达式

> order by 表达式

order by 列表达式

> group by 表达式 

group by 列表达式

> having表达式

having condition

> 查询表达式 （能查询出结果的，包括select 和select union select等）

查询表达式 =

select 
(top/distinct)?
列表达式(*，列名，函数，逻辑运算，方法，数值，日期,参数等) 别名？
目标表达式(from xxx 目标表达式也可以是另一个查询表达式或者union表达式或者是表名、视图等)
join on 表达式(join 一个目标表达式，left join ,inner join cross join 等)
APPLY 表达式?
条件表达式(where)
排序表达式(order by)
分组表达式(group by 列表达式)
having表达式

或者

查询表达式 union 查询表达式



> insert 表达式

insert into 
集合对象表达式 (列名)?
values(),()? | 查询表达式

直接给值或者是查询结果

> update 表达式

update
目标别名？
目标表达式
赋值表达式(set xx=xxx)
join on 表达式(join 一个目标表达式，left join ,inner join cross join 等)
条件表达式(where)

> delete 表达式

delete
目标别名？
目标表达式
join on 表达式(join 一个目标表达式，left join ,inner join cross join 等)
条件表达式

> drop 表达式

drop [table/column] [TableName];

> case when condition then 列表达式 end 别名？

> rownumber() 表达式

rownumber() over(partition by 列表达式 order by 列表达式)

这里面的列表达式不能嵌套rownumber表达式，这个得处理下，再做细分

> 类型表达式 

nvarchar()

table(
	列名 类型，
	列名 类型
)

> APPLY 表达式

[OUTER/CROSS] APPLY 查询表达式

OUTER APPLY把一列展开成多列

> ANY/ALL  表达式

> OFFSET FETCH

> NULL

> merge

> transaction

>  CTE (Common Table Expression (CTE))

WITH expression_name (column_list)
AS
(
    -- Anchor member
    initial_query  
    UNION ALL
    -- Recursive member that references expression_name.
    recursive_query  
)

> EXCEPT

> EXISTS

> NULLIF

> ROLLUP 

> CUBE

> SELECT INTO 

> GROUPING SETS

> join 关键字

INNER JOIN 、LEFT JOIN、RIGHT JOIN、FULL OUTER JOIN、Self Join、CROSS JOIN

> PIVOT

> OFFSET FETCH
```sql 
ORDER BY column_list [ASC |DESC]
OFFSET offset_row_count {ROW | ROWS}
FETCH {FIRST | NEXT} fetch_row_count {ROW | ROWS} ONLY
```