using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Shared.Model.Protocols.Modbus
{
    /**
     * 每个地址16位也就是两个字节
     * 
     * 32位的数据就是4个字节
     * 
     * modbus 一个地址是两位，这两位默认是大端
     * 
     * 大端就是数据的高位在前，低位在后，小端相反，这里说的是字节顺序
     * 
     * 大端swap就是数据的高位在前，低位在后，然后存储到对应地址的时候，地址内的两个字节顺序再交换一下(swap)
     * 
     * 小端swap就是数据的低位在前，高位在后，然后存储到对应地址的时候，地址内的两个字节顺序再交换一下(swap)
     * 
     */

    /// <summary>
    /// Modbus协议的数据类型
    /// </summary>
    public enum ModbusDataType
    {
        /// <summary>
        /// 位，一般用来放布尔类型，一个地址16位可以放16位的数据
        /// </summary>
        Bit,
        /// <summary>
        /// 16位有符号整数
        /// </summary>
        Signed16,
        /// <summary>
        /// 16位无符号整数
        /// </summary>
        Unsigned16,
        /// <summary>
        /// 32位有符号整数
        /// </summary>
        Signed32,
        /// <summary>
        /// 16位无符号整数
        /// </summary>
        Unsigned32,
        /// <summary>
        /// 32位有符号整数
        /// </summary>
        Signed64,
        /// <summary>
        /// 16位无符号整数
        /// </summary>
        Unsigned64,
        /// <summary>
        /// 浮点数
        /// </summary>
        Float,
        /// <summary>
        /// 
        /// </summary>
        Double
    }
}
