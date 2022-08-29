using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// 字段
    /// </summary>
    public class HField : HContainer<HToken>
    {

        public IList<HToken> _hComponents  = new List<HToken>();

        protected override IList<HToken> ChildrenTokens => _hComponents;

        public static HToken Parse(string value, HSeparator separator = null)
        {
            separator = separator ?? HSeparator.Default;

            string[] array = value.Split(separator.ArraySeparator);

            if (array.Length == 1)
            {
                return ParseOneField(array[0], separator);
            }
            else {
                HFieldArray hFieldArray = new HFieldArray();
                foreach (var item in array)
                {
                    var field = ParseOneField(item, separator);
                    field.Parent = hFieldArray;
                    hFieldArray.Add(field);
                }
                return hFieldArray;
            }
            //var hComponents = new List<HComponent>();
            //string[] values = value.Split(splitCode);
            //foreach (var item in values)
            //{
            //    var hComponent = new HComponent(item,this);
            //    hComponents.Add(hComponent);
            //}
            //HComponents = hComponents;
        }

        private static HToken ParseOneField(string value, HSeparator separator = null)
        {
            string[] components = value.Split(separator.ComponentSeparator);
            if (components.Length == 1)
            {
                HToken token = HComponent.Parse(components[0],separator);
                if (token is HValue)
                {
                    return token;
                }
                else
                {
                    HField field = new HField();
                    token.Parent = field;
                    field.Add(token);
                    return field;
                }
            }
            else {
                HField field = new HField();
                foreach (var item in components)
                {
                    HToken token = HComponent.Parse(item, separator);
                    token.Parent = field;
                    field.Add(token);
                }
                return field;
            }
        }

        public override void VisitText(StringBuilder builder)
        {
            int i = 0;
            foreach (var item in ChildrenTokens)
            {
                if (i > 0)
                {
                    builder.Append(Root.HHeader.HSeparator.ComponentSeparator);
                }
                item.VisitText(builder);
                i++;
            }
        }
    }
}
