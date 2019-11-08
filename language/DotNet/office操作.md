# office操作

```csharp
static void CreateWord()
{
    string connectstr = @"Data Source=192.168.68.11;Initial Catalog=TTVVP_System;User ID=sa;Password=TT_database@2106";
    DataTable alltable = new DataTable();
    using (SqlConnection con = new SqlConnection(connectstr))
    {
        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM sys.tables order by name", con);
        adapter.Fill(alltable);
    }
    object missing = Missing.Value;
    MSWord.Application wordApp = null;
    MSWord.Document wordDoc = null;

    wordApp = new MSWord.ApplicationClass();
    wordApp.Visible = true;
    wordDoc = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);
    for (int i = 0; i < alltable.Rows.Count; i++)
    {
        string tablename = Convert.ToString(alltable.Rows[i]["name"]);
        AddTable(wordDoc, connectstr, tablename);
    }

    object path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.docx");
    wordDoc.SaveAs2(ref path, ref missing, ref missing, ref missing, ref missing);
}

static void AddTable(MSWord.Document wordDoc, string connectstr, string tablename)
{

    DataSet ds = new DataSet();
    using (SqlConnection con = new SqlConnection(connectstr))
    {
        SqlDataAdapter adapter = new SqlDataAdapter("sp_help " + tablename, con);
        adapter.Fill(ds);
    }
    DataTable dtable = ds.Tables[1];
    DataTable pk = ds.Tables[5];
    var sections = wordDoc.Sections;

    DataRow indexrow = pk.Select().FirstOrDefault(m => Convert.ToString(m["index_name"]).IndexOf("PK") == 0);
    string key = indexrow == null ? "" : Convert.ToString(indexrow["index_keys"]);
    string[] keys = key.Split(',').Select(m => m.Trim()).ToArray();
    MSWord.Range range = wordDoc.Range(wordDoc.Paragraphs.Last.Range.Start, wordDoc.Paragraphs.Last.Range.End);
    range.Text = tablename;
    range.Font.Size = 14;
    range.InsertParagraphAfter();
    range.InsertParagraphAfter();
    MSWord.Range range1 = wordDoc.Range(wordDoc.Paragraphs.Last.Range.Start, wordDoc.Paragraphs.Last.Range.End);
    MSWord.Table table = wordDoc.Tables.Add(range1, dtable.Rows.Count, 6, null, null);
    table.Borders.Enable = 1;//默认表格没有边框
                              //给表格中添加内容

    //设置表头
    table.Cell(1, 1).Range.Text = "序号";
    table.Cell(1, 1).Range.Bold = 1;
    table.Cell(1, 1).Range.Font.Name = "仿宋";
    table.Cell(1, 1).Range.Font.Size = 12;
    table.Cell(1, 2).Range.Text = "字段";

    table.Cell(1, 2).Range.Font.Name = "仿宋";
    table.Cell(1, 2).Range.Bold = 1;
    table.Cell(1, 2).Range.Font.Size = 12;
    table.Cell(1, 3).Range.Text = "类型";
    table.Cell(1, 3).Range.Font.Name = "仿宋";
    table.Cell(1, 3).Range.Bold = 1;
    table.Cell(1, 3).Range.Font.Size = 12;
    table.Cell(1, 4).Range.Text = "键";
    table.Cell(1, 4).Range.Bold = 1;
    table.Cell(1, 4).Range.Font.Name = "仿宋";
    table.Cell(1, 4).Range.Font.Size = 12;
    table.Cell(1, 5).Range.Text = "名称";
    table.Cell(1, 5).Range.Bold = 1;
    table.Cell(1, 5).Range.Font.Size = 12;
    table.Cell(1, 5).Range.Font.Name = "仿宋";
    table.Cell(1, 6).Range.Text = "说明";
    table.Cell(1, 6).Range.Bold = 1;
    table.Cell(1, 6).Range.Font.Name = "仿宋";
    table.Cell(1, 6).Range.Font.Size = 12;

    for (int i = 0; i < dtable.Rows.Count; i++)
    {
        DataRow row = dtable.Rows[i];
        string type = Convert.ToString(row["Type"]) + "(" + Convert.ToString(row["Length"]) + ")";
        string name = Convert.ToString(row["Column_name"]);
        bool iskey = keys.Contains(name);
        table.Cell(2 + i, 1).Range.Text = (i + 1).ToString();
        table.Cell(2 + i, 1).Range.Font.Name = "新宋体";
        table.Cell(2 + i, 1).Range.Font.Size = 9.5f;
        table.Cell(2 + i, 2).Range.Font.Name = "新宋体";
        table.Cell(2 + i, 2).Range.Font.Size = 9.5f;
        table.Cell(2 + i, 2).Range.Text = name;
        table.Cell(2 + i, 3).Range.Font.Name = "新宋体";
        table.Cell(2 + i, 3).Range.Font.Size = 9.5f;
        table.Cell(2 + i, 3).Range.Text = type;
        table.Cell(2 + i, 4).Range.Font.Name = "新宋体";
        table.Cell(2 + i, 4).Range.Font.Size = 9.5f;
        table.Cell(2 + i, 4).Range.Text = iskey ? "PK" : "";
        table.Cell(2 + i, 5).Range.Text = "";
        table.Cell(2 + i, 5).Range.Font.Name = "新宋体";
        table.Cell(2 + i, 5).Range.Font.Size = 9.5f;
        table.Cell(2 + i, 6).Range.Text = "";
        table.Cell(2 + i, 6).Range.Font.Name = "新宋体";
        table.Cell(2 + i, 6).Range.Font.Size = 9.5f;
    }
}
```