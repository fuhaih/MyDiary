using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// HL7消息
    /// </summary>
    public class HMessage : HToken,IDictionary<string,IList<HSegment>>
    {

        private const string MSH = "MSH";
        public HHeader HHeader {
            get
            {
                IList<HSegment> segments = new List<HSegment>();
                if (HSegments.TryGetValue(MSH, out segments))
                {
                    return (HHeader)segments.FirstOrDefault();
                }
                else { 
                    return null;
                }
            }
        }

        private IDictionary<string, IList<HSegment>> HSegments = new Dictionary<string, IList<HSegment>>();

        public ICollection<string> Keys => HSegments.Keys;

        public ICollection<IList<HSegment>> Values => HSegments.Values;

        public int Count => HSegments.Count;

        public bool IsReadOnly => HSegments.IsReadOnly;

        public IList<HSegment> this[string key] { get => HSegments[key]; set => HSegments[key] = value; }

        public HMessage()
        {
        }

        public void TryAddOneValue(string key, HSegment value)
        {
            IList<HSegment> segments = null;
            if (HSegments.TryGetValue(key, out segments))
            {
                segments.Add(value);
            }
            else {
                segments = segments ?? new List<HSegment>();
                segments.Add(value);
                HSegments.Add(key, segments);
            }
        }

        public static HMessage Parse(string value)
        {
            HMessage message = new HMessage();
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("参数不能为空");
            }
            string[] segments = value.Split(Environment.NewLine);
            string headerValue = segments.FirstOrDefault();
            HHeader header = HHeader.Parse(headerValue);
            header.Parent = message;
            message.TryAddOneValue(header.SegmentName, header);
            //var hSegments = new List<HSegment>();
            for (int i = 1; i < segments.Length; i++)
            {
                var segment = segments[i];
                if (string.IsNullOrEmpty(segment)) continue;
                var hSegment = HSegment.Parse(segment, header.HSeparator);
                hSegment.Parent = message;
                if (string.IsNullOrEmpty(hSegment.SegmentName)) continue;
                message.TryAddOneValue(hSegment.SegmentName,hSegment);
            }
            return message;
        }

        //public override IEnumerator<HToken> GetEnumerator()
        //{
        //    yield return HHeader;
        //    foreach (var segment in HSegments)
        //    { 
        //        yield return segment;
        //    }
        //}

        public void Add(string key, IList<HSegment> value)
        {
            HSegments.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return HSegments.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return HSegments.Remove(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out IList<HSegment> value)
        {
            return HSegments.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, IList<HSegment>> item)
        {
            HSegments.Add(item);
        }

        public void Clear()
        {
            HSegments.Clear();
        }

        public bool Contains(KeyValuePair<string, IList<HSegment>> item)
        {
            return HSegments.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, IList<HSegment>>[] array, int arrayIndex)
        {
            HSegments.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, IList<HSegment>> item)
        {
            return HSegments.Remove(item);
        }

        IEnumerator<KeyValuePair<string, IList<HSegment>>> IEnumerable<KeyValuePair<string, IList<HSegment>>>.GetEnumerator()
        {
            return HSegments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return HSegments.GetEnumerator();
        }

        public override HToken SelectToken(string path)
        {
            int index = path.IndexOf(PATH_SEGMENT_SPARATOR);
            string subPath = index <0?path : path.Substring(0, index);

            Regex regex = new Regex(@"^(\w+)(\[(\d+)\])?$");
            var match = regex.Match(subPath);
            if (match == null)
            {
                return null;
            }
            string segmentNameValue = match.Groups[1].Value;
            string segmentIndexValue = match.Groups[3].Success ? match.Groups[3].Captures[0].Value : "0"; ;
            IList<HSegment> segments = null;
            if (TryGetValue(segmentNameValue, out segments)&&int.TryParse(segmentIndexValue,out int segmentIndex))
            {
                var hSegment = segments?[segmentIndex];
                if (index > 0)
                {
                    return hSegment?.SelectToken(path.Substring(index+1));
                }
                else {
                    return hSegment;
                }
            }
            else {
                return null;
            }
        }

        public override void VisitText(StringBuilder builder)
        {
            int i = 0;
            foreach (var group in HSegments)
            {
                foreach (var item in group.Value)
                {
                    if (i > 0)
                    {
                        builder.Append(Environment.NewLine);
                    }
                    item.VisitText(builder);
                    i++;
                }
            }
        }
    }
}
