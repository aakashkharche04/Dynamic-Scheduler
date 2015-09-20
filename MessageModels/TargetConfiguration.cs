using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModels
{
    public class TargetConfiguration
    {
        private int targetPayload = 30;
        private int maxMerchantId = 10;
        private int hours = 1;

        public int getTargetLoad()
        {
            return targetPayload;
        }

        public int getMaxMerchantId()
        {
            return maxMerchantId;
        }

        public int getTime()
        {
            return hours;
        }
    }
}
