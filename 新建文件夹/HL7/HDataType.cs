using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    public enum HDataType
    {
        /// <summary>
        /// 字符串string
        /// </summary>
        ST,
        /// <summary>
        /// 文本text
        /// </summary>
        TX,
        /// <summary>
        /// 格式化文本
        /// </summary>
        FT,
        /// <summary>
        /// number
        /// </summary>
        NM,
        /// <summary>
        /// 序列ID
        /// </summary>
        SI,
        /// <summary>
        /// 结构化数据
        /// </summary>
        SN,
        /// <summary>
        /// HL7表的编码值
        /// </summary>
        ID,
        /// <summary>
        /// 用户定义表的编码
        /// </summary>
        IS,
        /// <summary>
        /// 实体标识符
        /// </summary>
        EI,
        /// <summary>
        /// 日期
        /// </summary>
        DT,
        /// <summary>
        /// 时间
        /// </summary>
        TM,
        /// <summary>
        /// 编码要素
        /// </summary>
        CE,
        /// <summary>
        /// 具有校验数位的扩展符合ID
        /// </summary>
        CX,
        /// <summary>
        /// 扩展符合ID号和ID名
        /// </summary>
        XCN,
        /// <summary>
        /// 扩展地址
        /// </summary>
        XAD,
        /// <summary>
        /// 扩展姓名
        /// </summary>
        XPN,
        /// <summary>
        /// 扩展通讯号码
        /// </summary>
        XTN
    }
}
