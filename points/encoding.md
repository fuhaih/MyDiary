>指针对齐

# Encoding

## ascii

ascii码表有127个字符，所以使用ascii码进行编码时候，一个字符是转换为一个byte(8位 255)字节，但是如果要编译中文或者其他复杂字符时候，是力不从心的，需要使用其他编码格式把对中文进行编码，一般一个中文会转换为两个byte字节

## unicode

## utf-8

# 值类型编码

## int

dotnet中int 类型有`Int16`、`Int32`、`Int64`，都是有符号整数，分别是16位、32位、64位存储空间

首位是符号位

正数的存储就是存储对于的二进制形式，

如Int16存储1为 00000000 00000001

负数是使用补码来存储的，这样方便在寄存器中进行计算

如-1

-1的二进制位 10000000 00000001

-1的反码为  11111111 11111110 

-1的补码为  11111111 11111111

所以Int16存储-1为 11111111 11111111

> int类型和byte[]相互转换

上面清楚了int类型的存储格式，那么就可以很容易实现int类型和byte的相互转换

int 转byte[]
```csharp
int test = 307;
int size = sizeof(int);
byte[] int2bytes = new byte[size];
for (int i = 0; i < size; i++) {
    // 通过&操作来获取后八位的值，转为byte
    // 255 是 11111111
    int2bytes[i] = (byte)(test & 255);
    //通过移位操作移除后八位
    test = test >> 8;
}
```

byte[] 转int
```csharp
// 前面int转byte时，把低位数值放在了byte[]数组前面，
// 要byte转int时，需要先从高位进行计算，所以这里转换一下
int2bytes = int2bytes.Reverse().ToArray()
int result = int2bytes[0];
for (int i = 1; i < int2bytes.Length; i++) {
    // 通过移位操作算出高位
    result = result << 8;
    // 再加上低位数值
    result =result + int2bytes[i];
}
```

## float

# 其他特殊编码

## base64

## 16进制字符串hex

四位比特位能表示0-15，正好是以为16进制的大小，所以一个字节byte可以转换成两个16进制

byte 转 hex
```csharp
// 方法1
BitConverter.ToString(bytes).Replace("-", " ");

//方法2
StringBuilder sb = new StringBuilder();
for (int i = 0; i < bytes.Length; i++)
{
    sb.Append(bytes[i].ToString("x2"));
}
return sb.ToString();

//方法3 byte拆分两个4位，转换为0-9A-F对应的ascii码的byte，再转换成字符串
byte[] hex_bytes = new byte[bytes.Length * 2];
for (int i = 0; i < test_rev_bytes.Length; i++)
{
    int value1 = bytes[i] & 15;
    // 0-9对应的ascii是48-57 A-F对应的ascii是65-70
    byte byte1 = value1 < 10 ? (byte)(value1 + 48) : (byte)(value1 + 55);
    int value2 = bytes[i] >> 4;
    byte byte2 = value2 < 10 ? (byte)(value2 + 48) : (byte)(value2 + 55);
    hex_bytes[i * 2] = byte2 ;//高位在前
    hex_bytes[i * 2 + 1] = byte1;//地位在后
}
string hex_bytes_test = Encoding.ASCII.GetString(hex_bytes);
```

hex 转 byte

```csharp
//方法1
byte[] result = new byte[str.Length / 2];
for (int i = 0; i < str.Length; i += 2)
{
    //可以直接使用Convert.ToByte(str.Substring(i, 2), 16);方法吧16进制字符串转换为byte类型
    //int value = Convert.ToInt32(str.Substring(i, 2), 16);
    result[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
}
return result;

//方法2
int[] values = new int[bytes.Length];
//转换为数字表示
for (int i = 0; i < bytes.Length; i++) {
    int value = bytes[i];//ASCII码
    // 字符0-9的ASCII码在48-57之间
    if (value >= 48 && value <= 57)
    {
        values[i] = value - 48;
    }
    //字符A-F在ASCII码在65-70之间
    else if (value >= 65 && value <= 70) {
        values[i] = value - 55;
    }
}
byte[] tens_bytes = new byte[values.Length / 2];
// 每两位十六进制转换为一个byte
for (int i = 0; i < values.Length; i=i+2) {
    int byte_ascii = values[i] * 16 + values[i + 1];
    tens_bytes[i / 2] = (byte)byte_ascii;
}
```


