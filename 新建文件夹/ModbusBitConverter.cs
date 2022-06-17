using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Modbus
{
    public class ModbusBitConverter
    {
        /// <summary>
        /// 获取比特位值，返回值是0或者1
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="startIndex"></param>
        /// <param name="bitAddress"></param>
        /// <returns></returns>
        public static Int16 ToBit(byte[] value, int startIndex, int bitAddress)
        {
            int bit = BitConverter.ToInt16(value, startIndex);
            //BitConverter转换是小端在前的，modbus的数据是大端在前的
            //所以数据第0位返回来是在第9位，
            if (bitAddress < 8)
            {
                bit = bit >> 8;
            }
            bit = bit >> (bitAddress % 8);
            bit = bit & 1;
            return (Int16)bit;
        }

        /// <summary>
        /// byte转换为16位整数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static Int16 ToInt16(byte[] value, int startIndex)
        {
            byte[] data = value.Skip(startIndex).Take(2).ToArray();
            data = SwapValue(data, false, false);
            return BitConverter.ToInt16(data, 0);
            //return BitConverter.ToInt16(value, startIndex);
        }

        /// <summary>
        /// byte转换为16位无符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(byte[] value, int startIndex)
        {
            byte[] data = value.Skip(startIndex).Take(2).ToArray();
            data = SwapValue(data, false, false);
            return BitConverter.ToUInt16(data, 0);
        }

        /// <summary>
        /// byte转换为32位整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">开始下标</param>
        /// <param name="isLittleEndian">是否小端</param>
        /// <param name="swap">是否交换</param>
        /// <returns></returns>
        public static Int32 ToInt32(byte[] value, int startIndex, bool isLittleEndian, bool swap)
        {
            //BitConverter 默认就是小端
            if (isLittleEndian && !swap)
            {
                return BitConverter.ToInt32(value, startIndex);
            }
            else {
                byte[] data = value.Skip(startIndex).Take(4).ToArray();
                data = SwapValue(data,isLittleEndian,swap);
                return BitConverter.ToInt32(data, 0);
            }
        }

        /// <summary>
        /// byte转换为32位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">开始下标</param>
        /// <param name="isLittleEndian">是否小端</param>
        /// <param name="swap">是否交换</param>
        /// <returns></returns>
        public static UInt32 ToUInt32(byte[] value, int startIndex, bool isLittleEndian, bool swap)
        {
            //BitConverter 默认就是小端
            if (isLittleEndian && !swap)
            {
                return BitConverter.ToUInt32(value, startIndex);
            }
            else
            {
                byte[] data = value.Skip(startIndex).Take(4).ToArray();
                data = SwapValue(data, isLittleEndian, swap);
                return BitConverter.ToUInt32(data, 0);
            }
        }

        /// <summary>
        /// byte 转换为64位整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">开始下标</param>
        /// <param name="isLittleEndian">是否小端</param>
        /// <param name="swap">是否交换</param>
        /// <returns></returns>
        public static Int64 ToInt64(byte[] value, int startIndex, bool isLittleEndian, bool swap)
        {
            //BitConverter 默认就是小端
            if (isLittleEndian && !swap)
            {
                return BitConverter.ToInt64(value, startIndex);
            }
            else
            {
                byte[] data = value.Skip(startIndex).Take(8).ToArray();
                data = SwapValue(data, isLittleEndian, swap);
                return BitConverter.ToInt64(data, 0);
            }
        }

        /// <summary>
        /// byte 转换为64位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">开始下标</param>
        /// <param name="isLittleEndian">是否小端</param>
        /// <param name="swap">是否交换</param>
        /// <returns></returns>
        public static UInt64 ToUInt64(byte[] value, int startIndex, bool isLittleEndian, bool swap)
        {
            //BitConverter 默认就是小端
            if (isLittleEndian && !swap)
            {
                return BitConverter.ToUInt64(value, startIndex);
            }
            else
            {
                byte[] data = value.Skip(startIndex).Take(8).ToArray();
                data = SwapValue(data, isLittleEndian, swap);
                return BitConverter.ToUInt64(data, 0);
            }
        }

        /// <summary>
        /// byte 转换为float
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">开始下标</param>
        /// <param name="isLittleEndian">是否小端</param>
        /// <param name="swap">是否交换</param>
        /// <returns></returns>
        public static float ToSingle(byte[] value, int startIndex, bool isLittleEndian, bool swap)
        {
            //BitConverter 默认就是小端
            if (isLittleEndian && !swap)
            {
                return BitConverter.ToSingle(value, startIndex);
            }
            else
            {
                byte[] data = value.Skip(startIndex).Take(4).ToArray();
                data = SwapValue(data, isLittleEndian, swap);
                return BitConverter.ToSingle(data, 0);
            }
        }

        /// <summary>
        /// byte转换为double
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">开始下标</param>
        /// <param name="isLittleEndian">是否小端</param>
        /// <param name="swap">是否交换</param>
        /// <returns></returns>
        public static double ToDouble(byte[] value, int startIndex, bool isLittleEndian, bool swap)
        {
            //BitConverter 默认就是小端
            if (isLittleEndian && !swap)
            {
                return BitConverter.ToDouble(value, startIndex);
            }
            else
            {
                byte[] data = value.Skip(startIndex).Take(8).ToArray();
                data = SwapValue(data, isLittleEndian, swap);
                return BitConverter.ToDouble(data, 0);
            }
        }

        /// <summary>
        /// 交换数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isLittleEndian"></param>
        /// <param name="swap"></param>
        /// <returns></returns>
        private static byte[] SwapValue(byte[] value, bool isLittleEndian, bool swap)
        {
            if (!isLittleEndian)
            {
                value = value.Reverse().ToArray();
            }
            if (swap)
            {
                for (int i = 0; i < value.Length; i = i + 2)
                {
                    if (i + 1 < value.Length)
                    {
                        byte temple = value[i];
                        value[i] = value[i + 1];
                        value[i + 1] = temple;
                    }
                }
            }
            return value;
        }
    }
}
