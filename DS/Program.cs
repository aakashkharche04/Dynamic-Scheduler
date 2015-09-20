using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

#region ModuleNamespaces
using ManageMessages;
using MessageModels.KeyValuePairs;
#endregion


namespace DS
{
    class Program
    {
        static void Main(string[] args)
        {
             Timer timer =  new Timer(10000);
             timer.Elapsed += new ElapsedEventHandler(sendMessage);
             timer.AutoReset = true;
             timer.Enabled = true;
            Console.ReadKey();
        }

        public static void sendMessage(object source, ElapsedEventArgs e)
        {
            try
            {
                KeyValues kv = new KeyValues();
                IList<KeyValuePair<int, string>> priorityPairs = kv.getPriorities();

                IList<KeyValuePair<int, string>> datatypes = kv.getDataTypes();
                Random rd = new Random();
                int[] marketPlace = new int[] { 1, 2, 3, 4, 5, 6, 7 };
                string[] messageArray = new string[50];
                for (int i = 0; i < messageArray.Length; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    string itemId = "Item" + rd.Next(0,9999).ToString();
                    string merchantId = rd.Next(0, 99999).ToString();
                    string market = marketPlace[i % 7].ToString();
                    string priority = priorityPairs[i % 5].Value;
                    //string payload = (i % 13).ToString();
                    string dataType = datatypes[i % 3].Value;
                    string payload = "payload";
                    DateTime requestTime = DateTime.Now;
                    sb.Append(string.Format("{0},{1},{2},{3},{4},{5},{6}", itemId, merchantId, market, priority, dataType, payload, requestTime));
                    messageArray[i] = sb.ToString();
                }
                HandleMessage hm = new HandleMessage();
                hm.receiveMessages(messageArray);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

   
}
