using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// 段
    /// </summary>
    public class HSegment : HContainer<HToken>
    {
        public string SegmentName {
            get {
                return ((HValue)ChildrenTokens.FirstOrDefault())?.Value;
            }
        }

        private IList<HToken> _hFields = new List<HToken>();

        /// <summary>
        /// 子项，需要做类型约束的话可以定义接口
        /// </summary>
        protected override IList<HToken> ChildrenTokens => _hFields;

        public static HSegment Parse(string value, HSeparator separator =null)
        {
            separator = separator ?? HSeparator.Default;
            HSegment segment = new HSegment();
            string[] fields = value.Split(separator.FieldSeparator);
            foreach (var field in fields)
            {
                var hField = HField.Parse(field, separator);
                hField.Parent = segment;
                segment.Add(hField);
            }
            return segment;
        }

        public override HToken SelectToken(string path)
        {
            int index = path.IndexOf(PATH_SPARATOR);
            string subPath = index < 0 ? path : path.Substring(0, index);
            //匹配2[1]
            Regex regex = new Regex(@"^(\d+)(\[(\d+)\])?$");
            var match = regex.Match(subPath);
            if (match == null)
            {
                return null;
            }
            string numberValue = match.Groups[1].Value;
            string numberIndexValue = match.Groups[3].Success ? match.Groups[3].Captures[0].Value:"0";
            if (int.TryParse(numberValue, out int number) && int.TryParse(numberIndexValue, out int numberIndex))
            {
                var token = ChildrenTokens[number];
                if (token is HFieldArray && match.Groups[3].Success)
                {
                    token = ((HFieldArray)token)[numberIndex];
                }
                else if (numberIndex != 0)
                {
                    token = null;
                }
                if (index > 0)
                {
                    return token?.SelectToken(path.Substring(index+1));
                }
                else
                {
                    return token;
                }
            }
            else {
                return null;
            }

        }

        public override void VisitText(StringBuilder builder)
        {
            int i = 0;
            foreach (var item in ChildrenTokens)
            {
                if (i > 0)
                { 
                    builder.Append(Root.HHeader.HSeparator.FieldSeparator);
                }
                item.VisitText(builder);
                i++;
            }
        }
    }
}
