## lexer 词法分析

* IdentifierToken 标志符
* DotToken .
* OpenParenToken (
* CloseParenToken )
* NumericLiteralToken 数值
* CommaToken ,
* OpenBracketToken [
* CloseBracketToken ]
* PlusToken +
* MinusToken  -
* AsteriskToken *
* SlashToken /
* PercentToken %
* CaretToken ^
* AmpersandToken &
* BadToken @
* ExclamationToken !
* TildeToken ~
* StringLiteralToken "desfs"
* CharacterLiteralToken 's'
* EqualsToken =
* EndOfFileToken 

lexer 在生成token后后续的语法分析不需要用到空格和换行等信息，所以不需要记录空格和换行生成对应token
```sql
SELECT 
	  ''as astestname
	  ,''testname
	  ,'' spacetestname
      ,[Name]
  FROM [RportTest].[dbo].[ReportData]
```
这段sql中''会解析为一个CharacterLiteralToken，后续的as也会解析为一个IdentifierToken，所以''后面不跟着空格和不跟着空格，最后解析出来的token都是一致的，最后都不影响sql的执行

.net中的注释信息也会在生成token时候忽略掉