using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// 值，当没有子项的时候，直接用HValue
    /// </summary>
    public class HValue:HToken
    {
        public string Value { get; private set; }

        public HValue(string value)
        {
            this.Value = value;
        }

        public override HToken SelectToken(string path)
        {
            return null;
        }

        public override void VisitText(StringBuilder builder)
        {
            builder.Append(Value);
        }
    }
}
