using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR.WorkSchedule.Net.Protocols.HL7
{
    /// <summary>
    /// 容器类，包含IList子信息的类的父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HContainer<T> : HToken, IEnumerable<T>, IList<T> where T : HToken
    {

        protected abstract IList<T> ChildrenTokens
        {
            get;
        }


        public T this[int index] { get => ChildrenTokens[index]; set => ChildrenTokens[index] = value; }

        public int Count => ChildrenTokens.Count;

        public bool IsReadOnly => ChildrenTokens.IsReadOnly;

        public void Add(T item)
        {
            ChildrenTokens.Add(item);
        }

        public void Clear()
        {
            ChildrenTokens.Clear();
        }

        public bool Contains(T item)
        {
            return ChildrenTokens.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ChildrenTokens.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return ChildrenTokens.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            ChildrenTokens.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return ChildrenTokens.Remove(item);
        }

        public void RemoveAt(int index)
        {
            ChildrenTokens.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var field in ChildrenTokens)
            {
                yield return field;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var field in ChildrenTokens)
            {
                yield return field;
            }
        }

        public override HToken SelectToken(string path)
        {
            int index = path.IndexOf(PATH_SPARATOR);
            string subPath = index < 0 ? path : path.Substring(0, index);
            if (int.TryParse(subPath, out int number))
            {
                var token = ChildrenTokens[number];
                if (index > 0)
                {
                    return token?.SelectToken(path.Substring(index+1));
                }
                else
                {
                    return token;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
