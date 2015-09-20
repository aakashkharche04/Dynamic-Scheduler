using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModels.Comparer
{
    public class MessageAttributesComparer:IEqualityComparer<MessageDTO>
    {
        public bool Equals(MessageDTO obj1, MessageDTO obj2)
        {
            return ((obj1.MerchantId == obj2.MerchantId) && (obj1.DataType == obj2.DataType) &&  (obj1.Priority == obj2.Priority) && (obj1.MarkertPlace == obj2.MarkertPlace));
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
