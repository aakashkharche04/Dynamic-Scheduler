using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region MessageNamespaces
using MessageModels;
#endregion

namespace LogRequest
{
    public static class MaintainMerchants
    {
        public static IList<MerchantDTO> MerchantIdCounts = new List<MerchantDTO>();

        public static void addMerchantKey(MerchantDTO merchant)
        {
            if(checkKey(merchant.MerhchantId))
            {
                MerchantIdCounts.Add(merchant);
            }
        }

        public static bool checkKey(int key)
        {
            bool flag = true;
            if(MerchantIdCounts.Count > 0)
            {
                if(MerchantIdCounts.Where(m => m.MerhchantId == key).Any())
                {
                    flag = false;
                }
            }
            return flag;
        }
        
    }
}
