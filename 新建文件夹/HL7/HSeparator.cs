using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// hl7协议的分割符，hl7协议的分隔符可以自定义的，但是一般情况下是使用默认，分隔符是从头中解析出来
    /// </summary>
    public class HSeparator
    {
        public static HSeparator Default = new HSeparator();

        /// <summary>
        /// 字段分割符 |
        /// </summary>
        public char FieldSeparator { get; private set; }

        /// <summary>
        /// 数组分割符 ~
        /// </summary>
        public char ArraySeparator { get; private set; }

        /// <summary>
        /// 组件分割符 ^
        /// </summary>
        public char ComponentSeparator { get; private set; }

        /// <summary>
        /// 子组件分割符 &
        /// </summary>
        public char SubComponentSeparator { get; private set; }

        /// <summary>
        /// 转义符
        /// </summary>
        public char EscapeCharacter { get; private set; }

        public HSeparator():this('|', '~', '^', '&', '\\')
        { 
            
        }

        public HSeparator(char fieldSeparator, char arraySeparator, char componentSeparator, char subComponentSeparator, char escapeCharacter)
        { 
            FieldSeparator = fieldSeparator;
            ArraySeparator = arraySeparator;
            ComponentSeparator = componentSeparator;
            SubComponentSeparator = subComponentSeparator;
            EscapeCharacter = escapeCharacter;
        }
    }
}
