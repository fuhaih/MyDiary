using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Shared.Model.Protocols.Modbus
{
    /// <summary>
    /// Modbus功能码
    /// </summary>
    public enum ModbusCommandCode
    {
        /// <summary>
        /// 读线圈
        /// </summary>
        ReadCoilStatus = 1,

        /// <summary>
        /// 读线圈异常
        /// </summary>
        ReadCoilStatusError = 0x81,

        /// <summary>
        /// 写单个线圈
        /// </summary>
        WriteCoilStatus = 5,

        /// <summary>
        /// 写单个线圈异常
        /// </summary>
        WriteCoilStatusError = 0x85,

        /// <summary>
        /// 写多个线圈
        /// </summary>
        WriteMultiCoilStatus = 15,

        /// <summary>
        /// 写多个线圈异常
        /// </summary>
        WriteMultiCoilStatusError = 0x8F,

        /// <summary>
        /// 读离散输入
        /// </summary>
        ReadInputsStatus = 2,

        /// <summary>
        /// 读离散输入异常
        /// </summary>
        ReadInputsStatusError = 0x82,

        /// <summary>
        /// 读输入寄存器
        /// </summary>
        ReadInputRegisters = 4,

        /// <summary>
        /// 读输入寄存器异常
        /// </summary>
        ReadInputRegistersError = 0x84,

        /// <summary>
        /// 读保持寄存器
        /// </summary>
        ReadHoldingRegisters = 3,

        /// <summary>
        /// 读保持寄存器异常
        /// </summary>
        ReadHoldingRegistersError = 0x83,

        /// <summary>
        /// 写单个保持寄存器
        /// </summary>
        WriteHoldingRegisters = 6,

        /// <summary>
        /// 写单个保持寄存器异常
        /// </summary>
        WriteHoldingRegistersError = 0x86,

        /// <summary>
        /// 写多个保持寄存器
        /// </summary>
        WriteMultiHoldingRegisters = 16,

        /// <summary>
        /// 写多个保持寄存器异常
        /// </summary>
        WriteMultiHoldingRegistersError = 0x90,

        /// <summary>
        /// 读/写多个保持寄存器
        /// </summary>
        ReadWriteMultiHoldingRegisters = 23,//这个功能码有点疑惑，有待研究

        /// <summary>
        /// 读/写多个保持寄存器异常
        /// </summary>
        ReadWriteMultiHoldingRegistersError = 0x97,

        /// <summary>
        /// 屏蔽写保持寄存器
        /// </summary>
        ScreenWriteHoldingRegisters = 22,

        /// <summary>
        /// 屏蔽写保持寄存器异常
        /// </summary>
        ScreenWriteHoldingRegistersError = 0x96
    }
}
