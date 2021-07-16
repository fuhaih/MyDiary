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



