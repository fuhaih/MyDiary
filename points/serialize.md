# xml

## 数组处理

数组可以直接使用`[XmlElement]`特性来标记，会把数组所有元素都展开，和其他属性放在同一级标签下

```csharp
public class SourceConfig
{
    public int Delay { get; set; } = 2;
    public string SourcePath { get; set; }
    public string IssuePath { get; set; }
    public string CronExpressions { get; set; }
    
    [XmlElement("Point")]
    public List<ModbusPoint> Points { get; set; }
}
```

```xml
<?xml version="1.0"?>
<SourceConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Delay>4</Delay>
  <CronExpressions>0 2/15 * * * ?</CronExpressions>
  <SourcePath></SourcePath>
  <IssuePath></IssuePath>
  <Point ID="Point1" ></Point>
  <Point ID="Point0" ></Point>
  <Point ID="Point2" ></Point>
  <Point ID="Point3" ></Point>
</SourceConfig>
```

也可以使用`[XmlArray]`和`[XmlArrayItem]`包装起来

```csharp
public class SourceConfig
{
    public int Delay { get; set; } = 2;
    public string SourcePath { get; set; }
    public string IssuePath { get; set; }
    public string CronExpressions { get; set; }
    [XmlArray("Points")]
    [XmlArrayItem("Point")]
    public List<ModbusPoint> Points { get; set; }
}
```

```xml
<?xml version="1.0"?>
<SourceConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Delay>4</Delay>
  <CronExpressions>0 2/15 * * * ?</CronExpressions>
  <SourcePath></SourcePath>
  <IssuePath></IssuePath>
  <Points>
    <Point ID="Point1" ></Point>
    <Point ID="Point0" ></Point>
    <Point ID="Point2" ></Point>
    <Point ID="Point3" ></Point>
  </Points>

</SourceConfig>
```

## 多态处理

```csharp
public class SourceConfig
{
    public int Delay { get; set; } = 2;
    public string SourcePath { get; set; }
    public string IssuePath { get; set; }
    public string CronExpressions { get; set; }
    [XmlArray("Points")]
    [XmlArrayItem(Type = typeof(NormalPoint), ElementName = "NormalPoint")]
    [XmlArrayItem(Type = typeof(SummationPoint), ElementName = "SummationPoint")]
    [XmlArrayItem(Type = typeof(StrategyPoint), ElementName = "StrategyPoint")]
    public List<ModbusPoint> Points { get; set; }
}
```

```xml
<?xml version="1.0"?>
<SourceConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Delay>4</Delay>
  <CronExpressions>0 2/15 * * * ?</CronExpressions>
  <SourcePath></SourcePath>
  <IssuePath></IssuePath>
  <Points>
    <NormalPoint ID="Point1"></NormalPoint>
    <SummationPoint ID="Point0" ></SummationPoint>
    <StrategyPoint ID="Point2" ></StrategyPoint>
  </Points>
</SourceConfig>
```

也可以使用`[XmlInclude]`

```csharp
[XmlInclude(typeof(NormalPoint))]
[XmlInclude(typeof(SummationPoint))]
[XmlInclude(typeof(StrategyPoint))]
public class ModbusPoint
{
}
```

```csharp
public class SourceConfig
{
    public int Delay { get; set; } = 2;
    public string SourcePath { get; set; }
    public string IssuePath { get; set; }
    public string CronExpressions { get; set; }
    [XmlArray("Points")]
    [XmlArrayItem("Point")]
    public List<ModbusPoint> Points { get; set; }
}
```

```xml
<?xml version="1.0"?>
<SourceConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Delay>4</Delay>
  <CronExpressions>0 2/15 * * * ?</CronExpressions>
  <SourcePath></SourcePath>
  <IssuePath></IssuePath>
  <Points>
    <Point ID="Point1" xsi:type="NormalPoint" ></Point>
    <Point ID="Point0" xsi:type="SummationPoint"></Point>
    <Point ID="Point2" xsi:type="StrategyPoint"></Point>
  </Points>

</SourceConfig>
```
# json

# byte