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