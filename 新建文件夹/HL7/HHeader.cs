using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// 头信息
    /// </summary>
    public class HHeader : HSegment
    {

        /// <summary>
        /// 编码字符，通常为^~\& MSH-1
        /// </summary>
        public HSeparator HSeparator { get; set; }

        /// <summary>
        /// 发送应用 MSH-3
        /// </summary>
        public string SendingApplication { get; set; }

        /// <summary>
        /// 发送应用命名空间ID MSH-3-1 
        /// </summary>
        public string SendingApplicationNamespaceID { get;set;}

        /// <summary>
        /// 发送应用应用ID MSH-3-2 
        /// </summary>
        public string SendingApplicationUniversalID { get; set; }

        /// <summary>
        /// 发送设备 MSH-4 发送设备。目前等同 MSH-3
        /// </summary>
        public string SendingFacility { get; set; }

        /// <summary>
        /// 接收设备 MSH-5
        /// </summary>
        public string ReceivingApplication { get; set; }

        /// <summary>
        /// 接收设备命名空间ID MSH-5-1 
        /// </summary>
        public string ReceivingApplicationNamespaceID { get; set; }

        /// <summary>
        /// 接收设备应用ID MSH-5-2 
        /// </summary>
        public string ReceivingApplicationUniversalID { get; set; }

        /// <summary>
        /// 接收设备 MSH-6。目前等同 MSH-5
        /// </summary>
        public string ReceivingFacility { get; set; }

        /// <summary>
        /// 消息发生时间
        /// </summary>
        public DateTime DateTimeOfMessage { get; set; }

        /// <summary>
        /// 加密，不知道干啥用 MSH-8
        /// </summary>
        public string Security { get; set; }

        /// <summary>
        /// 消息类型 MSH-9-1
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 事件 MSH-9-2
        /// </summary>
        public string TriggerEvent { get; set; }

        /// <summary>
        /// 消息结构  MSH-9-3
        /// </summary>
        public string MessageStructure { get; set; }

        /// <summary>
        /// 用于唯一标识某条消息。在 ACK 消息中，必须使用到该Field MSH-10
        /// </summary>
        public string MessageControlID { get; set; }

        /// <summary>
        /// 处理 ID 号  MSH-11
        /// </summary>
        public string ProcessingID { get; set; }

        /// <summary>
        /// 版本号 MSH-12 
        /// </summary>
        public string VersionID { get; set; }

        /// <summary>
        /// 顺序号 MSH-13
        /// </summary>
        public string SequenceNumber { get; set; }

        public HHeader(HSeparator hSeparator)
        {
            this.HSeparator = hSeparator;
        }

        public static HHeader Parse(string value)
        {
            string segmentName = value.Substring(0, 3);
            
            if (segmentName != "MSH")
            {
                throw new Exception($"HL7格式头异常{value}");
            }
            
            char fieldSeparator = value[3];
            
            string[] fields = value.Split(fieldSeparator);
            if (fields.Length < 12)
            {
                throw new Exception($"HL7格式头异常{value}");
            }
            string encodingCharacters = fields[1];
            
            HSeparator separator = new HSeparator(fieldSeparator, encodingCharacters[1], encodingCharacters[0], encodingCharacters[3], encodingCharacters[2]);
            HHeader header = new HHeader(separator);
            HValue segmentNameValue = new HValue(segmentName);
            segmentNameValue.Parent = header;
            HValue encodingValue = new HValue(encodingCharacters);
            encodingValue.Parent = header;
            header.Add(segmentNameValue);
            header.Add(encodingValue);
            for (int i =2;i<fields.Length;i++)
            {
                var field = fields[i];
                var hField = HField.Parse(field, separator);
                hField.Parent = header;
                header.Add(hField);
            }
            return header;
            //EncodingCharacters = fields[1];
            //SendingApplication = fields[2];
        }
    }
}
