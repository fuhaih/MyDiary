using SR.WorkSchedule.Shared.Helper;
using SR.WorkSchedule.Shared.Model.Protocols.Modbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Modbus
{

    /**
     * Modbus rtu和Modbus tcp两个协议的本质都是MODBUS协议，都是靠MODBUS寄存器地址来交换数据；但所用的硬件接口不一样，Modbus RTU一般采用串口RS232C或RS485/422，而Modbus TCP一般采用以太网口。现在市场上有很多协议转换器，可以轻松的将这些不同的协议相互转换 如：Intesisbox可以把modbus rtu转换成Modbus tcp
     * 实际上Modbus协议包括ASCII、RTU、TCP。
     * 标准的Modicon控制器使用RS232C实现串行的Modbus。Modbus的ASCII、RTU协议规定了消息、数据的结构、命令和就答的方式，数据通讯采用Maser/Slave方式。
     * Modbus协议需要对数据进行校验，串行协议中除有奇偶校验外，ASCII模式采用LRC校验，RTU模式采用16位CRC校验.
     * ModbusTCP模式没有额外规定校验，因为TCP协议是一个面向连接的可靠协议。
     * TCP和RTU协议非常类似，只要把RTU协议的两个字节的校验码去掉，然后在RTU协议的开始加上5个0和一个6并通过TCP/IP网络协议发送出去即可
     */

    /// <summary>
    /// Modbus Rtu协议通讯
    /// 发送的指令是固定8个字符的
    /// </summary>
    public class ModbusRtuClient:IDisposable
    {
        /**
        * 无校验 （no parity）
        * 奇校验 （odd parity）：如果字符数据位中"1"的数目是偶数，校验位为"1"，如果"1"的数目是奇数，校验位应为"0"。（校验位调整个数）
        * 偶校验 （even parity）：如果字符数据位中"1"的数目是偶数，则校验位应为"0"，如果是奇数则为"1"。（校验位调整个数）
        * mark parity：校验位始终为1
        * space parity：校验位始终为0
        * 串行通信的数据是逐位传输的,波特率、数据长度、停止位和奇偶校验位是更底层的数据传输的配置
        * 就是配置串口通讯每次发送的数据的频率、数据长度、停止位和校验，停止位标志着一次数据传输结束
        * modbus的一条指令会根据上面的配置分多次传输，crc校验是用来校验整个指令的数据
        */
        SerialPort serialPort = null;
        private bool disposedValue;

        /// <summary>
        /// 每次发送指令后，接收到的数据
        /// </summary>
        private byte[] Buffer;

        /// <summary>
        /// Buffer数据存储位置
        /// Buffer会根据发送的指令，固定好大小
        /// DataReceived 可能会触发多次，如果是多次，那么需要保存好Position的位置
        /// </summary>
        private int Position=0;

        /// <summary>
        /// 上次发送指令的操作码
        /// </summary>
        public ModbusCommandCode CommandCode { get;private set; }
        private TaskCompletionSource _taskCompletion;

        public ModbusRtuClient(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.DataReceived += DataReceived;
            serialPort.Open();
        }

        /// <summary>
        /// 读取保持寄存器
        /// </summary>
        /// <returns></returns>
        public async Task ReadHoldingRegisters(Int16 slaveId, Int16 address, Int16 quantity)
        {
            await SendReadInstructionsAsync(slaveId,ModbusCommandCode.ReadHoldingRegisters,address, quantity);
        }

        /// <summary>
        /// 发送读指令
        /// </summary>
        /// <param name="code">功能码</param>
        /// <param name="address">地址</param>
        /// <param name="number">参数数量(地址数量)</param>
        /// <returns></returns>
        public async Task SendReadInstructionsAsync(Int16 slaveId, ModbusCommandCode code,Int16 address,Int16 quantity)
        {
            int length = 5 + quantity * 2;
            Buffer = new byte[length];

            _taskCompletion = new TaskCompletionSource();
            CommandCode = code;
            byte[] bytes = new byte[8];
            List<string> values = new List<string>();
            UInt16 function = (UInt16)code;

            int header = slaveId << 8;
            header = header + function;

            //BitConverter.GetBytes 转换后是 低位->高位
            //modbus中是要 高位->低位
            //所以使用Reverse转换一下

            // 头，从机地址 和 操作码
            byte[] headerBytes = BitConverter.GetBytes((UInt16)header).Reverse().ToArray();
            values.Add(headerBytes.HexEecode());
            bytes[0] = headerBytes[0];
            bytes[1] = headerBytes[1];

            // 参数地址
            byte[] addressBytes = BitConverter.GetBytes(address).Reverse().ToArray();
            values.Add(addressBytes.HexEecode());
            bytes[2] = addressBytes[0];
            bytes[3] = addressBytes[1];

            // 参数个数
            byte[] numberBytes = BitConverter.GetBytes(quantity).Reverse().ToArray();
            values.Add(numberBytes.HexEecode());
            bytes[4] = numberBytes[0];
            bytes[5] = numberBytes[1];

            //校验位是 低位->高位顺序
            //byte[] valiBytes = bytes.Take(6).ToArray();
            UInt16 crc = bytes.CalculateCrc(6);
            byte[] crcBytes = BitConverter.GetBytes(crc);
            values.Add(crcBytes.HexEecode());
            bytes[6] = crcBytes[0];
            bytes[7] = crcBytes[1];

            // crc校验
            string str = string.Join(" ", values.ToArray());
            //richTextBoxResult.Text = code;
            serialPort.Write(bytes, 0, bytes.Length);
            Task result =await Task.WhenAny(_taskCompletion.Task,Task.Delay(1000));
            
            if (result != _taskCompletion.Task)
            {
                throw new TimeoutException($"串口{serialPort.PortName}地址{address}读取超时");
            }
        }

        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="address">发送指令的时候的地址</param>
        /// <param name="currentAddress">当前参数的地址</param>
        /// <param name="dataType">当前参数的数据类型</param>
        /// <param name="isLittleEndian">是否是小端在前</param>
        /// <param name="swap">是否swap</param>
        /// <returns></returns>
        public object GetData(int address, int currentAddress, ModbusDataType dataType, bool isLittleEndian, bool swap,int bit)
        {
            //一个地址两个字节
            int start = 3 + (currentAddress - address) * 2;
            switch (dataType)
            {
                case ModbusDataType.Bit:return ModbusBitConverter.ToBit(Buffer,start,bit);
                case ModbusDataType.Signed16: return ModbusBitConverter.ToInt16(Buffer, start);
                case ModbusDataType.Unsigned16: return ModbusBitConverter.ToUInt16(Buffer, start);
                case ModbusDataType.Signed32: return ModbusBitConverter.ToInt32(Buffer, start, isLittleEndian, swap);
                case ModbusDataType.Unsigned32: return ModbusBitConverter.ToUInt32(Buffer, start, isLittleEndian, swap);
                case ModbusDataType.Signed64: return ModbusBitConverter.ToInt64(Buffer, start, isLittleEndian, swap);
                case ModbusDataType.Unsigned64: return ModbusBitConverter.ToUInt64(Buffer, start, isLittleEndian, swap);
                case ModbusDataType.Float:return ModbusBitConverter.ToSingle(Buffer,start,isLittleEndian, swap);
                case ModbusDataType.Double:return ModbusBitConverter.ToDouble(Buffer,start,isLittleEndian, swap);
                default: throw new Exception("dataType 类型异常");
            }
        }

        /// <summary>
        /// 数据获取，这个是
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                StartReceived();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void StartReceived()
        {

            int readCount = Buffer.Length - Position;

            if (!_taskCompletion.Task.IsCompleted && serialPort.IsOpen && serialPort.BytesToRead > 0&& readCount>0)
            {
                //Buffer = new byte[serialPort.BytesToRead];

                readCount = readCount > serialPort.BytesToRead ? serialPort.BytesToRead : readCount;
                serialPort.Read(Buffer, Position, readCount);
                Position += readCount;
                if (Position >= Buffer.Length)
                {
                    byte[] crcBytes = Buffer.TakeLast(2).ToArray();
                    UInt16 crcValue = BitConverter.ToUInt16(crcBytes);
                    UInt16 calCrcValue = Buffer.CalculateCrc(Buffer.Length - 2);
                    if (crcValue != calCrcValue)
                    {
                        _taskCompletion.TrySetException(new Exception("crc校验异常"));
                    }
                    byte[] headBytes = Buffer.Take(2).ToArray();
                    var header = BitConverter.ToUInt16(headBytes);
                    var slaveId = (Int16)(header & 0xff);
                    var function = header >> 8;
                    if (function != (int)CommandCode)
                    {
                        byte[] errorBytes = new byte[2];
                        errorBytes[0] = 0;
                        errorBytes[1] = Buffer.Skip(2).Take(1).FirstOrDefault();
                        int error = BitConverter.ToUInt16(errorBytes);
                        _taskCompletion.TrySetException(new Exception($"操作异常，异常功能码{function},异常码{error}"));
                    }
                    _taskCompletion.TrySetResult();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                if (serialPort!=null&&serialPort.IsOpen)
                {
                    //serialPort的bug,请求结束时把DataReceived去掉，并关闭串口
                    //否则会出现异常 https://github.com/dotnet/runtime/issues/44952
                    
                    serialPort.Close();
                }
                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~ModbusRtuClient()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
