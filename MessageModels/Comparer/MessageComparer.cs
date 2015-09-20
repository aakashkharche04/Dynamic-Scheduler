using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModels.Comparer
{
    public class MessageComparer:IEqualityComparer<MessageDTO>
    {
        public bool Equals(MessageDTO obj1, MessageDTO obj2)
        {
            return (obj1.ItemId == obj2.ItemId);
        }

        public int GetHashCode(MessageDTO obj)
        {
            unchecked
            {
                if (obj == null)
                    return 0;
                int hashCode = obj.ItemId.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.ItemId.GetHashCode();
                return hashCode;
            }
        }
    }
}
