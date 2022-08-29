using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// 组件
    /// </summary>
    public class HComponent:HContainer<HToken>
    {
        private IList<HToken> _hSubComponents = new List<HToken>();

        protected override IList<HToken> ChildrenTokens => _hSubComponents;

        public static HToken Parse(string value, HSeparator separator = null)
        {
            separator = separator ?? HSeparator.Default;
            string[] subValues = value.Split(separator.SubComponentSeparator);
            if (subValues.Length == 1)
            {
                return new HValue(subValues[0]);
            }
            else {
                HComponent component = new HComponent();
                foreach (var subValue in subValues)
                {
                    var subComponent = new HValue(subValue);
                    subComponent.Parent = component;
                    component.Add(subComponent);
                }
                return component;
            }
        }

        public override void VisitText(StringBuilder builder)
        {
            int i = 0;
            foreach (var item in ChildrenTokens)
            {
                if (i > 0)
                {
                    builder.Append(Root.HHeader.HSeparator.SubComponentSeparator);
                }
                item.VisitText(builder);
                i++;
            }
        }
    }
}
