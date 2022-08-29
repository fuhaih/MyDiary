using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// 字段数组类
    /// </summary>
    public class HFieldArray : HContainer<HToken>
    {
        public IList<HToken> _hComponents = new List<HToken>();

        protected override IList<HToken> ChildrenTokens => _hComponents;

        public override HToken SelectToken(string path)
        {
            throw new NotImplementedException();
        }

        public override void VisitText(StringBuilder builder)
        {
            int i = 0;
            foreach (var item in ChildrenTokens)
            {
                if (i > 0)
                {
                    builder.Append(Root.HHeader.HSeparator.ArraySeparator);
                }
                item.VisitText(builder);
                i++;
            }
        }
    }
}
