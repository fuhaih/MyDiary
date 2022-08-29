using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// Token基类
    /// </summary>
    public abstract class HToken
    {
        public const char PATH_SEGMENT_SPARATOR = '-';

        public const char PATH_SPARATOR = '.';

        /// <summary>
        /// 父类
        /// </summary>
        public HToken Parent { get; internal set; }

        /// <summary>
        /// 根
        /// </summary>
        private HMessage _root;

        /// <summary>
        /// 根
        /// </summary>
        public HMessage Root { 
            get {
                if (_root == null)
                {
                    _root = Parent == null ? (HMessage)this : Parent.Root;
                }
                return _root;
            } }

        /// <summary>
        /// 根据路径查找值 格式PID-1[0].1  ORC[1]-1[0].1 只有段和字段有数组标识[]，如果字段不是使用~的数组，把[]去掉
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract HToken SelectToken(string path);

        /// <summary>
        /// 获取text文本，目前HMessage是无序的，需要有序可以添加上NextToken、PreToken这两个字段给HToken，组成链式
        /// 其他Token是有序的
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            StringBuilder builder = new StringBuilder();
            VisitText(builder);
            return builder.ToString();
        }

        public abstract void VisitText(StringBuilder builder);
    }
}
