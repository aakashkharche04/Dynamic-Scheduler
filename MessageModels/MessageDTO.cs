using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModels
{
    public class MessageDTO
    {
        public string ItemId { get; set; }
        public int MerchantId { get; set; }
        public int MarkertPlace { get; set; }
        public string Priority { get; set; }
        public string DataType { get; set; }
        public string Payload { get; set; }
        public int PriorirtId { get; set; }
        public DateTime requestTime { get; set; }
        public int SendPriority { get; set; }
    }
}
