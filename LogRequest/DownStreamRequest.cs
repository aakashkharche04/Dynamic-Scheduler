using ManageMessages.Enums;
using MessageModels;
using MessageModels.Comparer;
using MessageModels.KeyValuePairs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace LogRequest
{
    public class DownStreamRequest
    {
        private FilePath path = null;
        private KeyValues kv = null;
        IList<KeyValuePair<int, string>> priorties = null;
        private TargetConfiguration tc = null;
        private MessageComparer mc = null;
        private MessageAttributesComparer mac = null;

        public DownStreamRequest()
        {
            path = new FilePath();
            kv = new KeyValues();
            priorties = kv.getPriorities();
            tc = new TargetConfiguration();
            mc = new MessageComparer();
            mac = new MessageAttributesComparer();
        }

        public int RequestLogger()
        {
            try
            {
                    IList<string> Requests = new List<string>();
                    string[] fileArray = new string[] { path.getPath1(), path.getPath2() };
                    foreach (string file in fileArray)
                    {
                        //FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read);
                        //BinaryFormatter bs = new BinaryFormatter();
                        //StreamReader sr = (StreamReader)bs.Deserialize(fs);
                        StreamReader sr = new StreamReader(file);
                        while (!sr.EndOfStream)
                        {
                            if (sr.ReadLine() != null)
                            {
                                Requests.Add(sr.ReadLine());
                            }
                        }
                            sr.Close();
                    }
                    IList<MessageDTO> saveMessages = applyConfiguration(Requests).Distinct(mac).ToList();
                    //IList<MessageDTO> saveMessages = applyConfiguration(Requests).ToList();
                    string[] saveFormattedMessages = formatMessages(saveMessages);
                    using (var fs = new FileStream(path.getTargetPath(), FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        StreamWriter File = new StreamWriter(fs);
                        for (int j = 0; j < saveFormattedMessages.Length; j++)
                        {
                            File.WriteLine(saveFormattedMessages[j]);
                        }
                        File.Close();
                    }
                return 1;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string[] formatMessages(IList<MessageDTO> messageList)
        {
            string[] fm = new string[messageList.Count];
            for(int i=0 ; i < messageList.Count ; i++)
            {
                MessageDTO mess = messageList[i];
                fm[i] = string.Format("{0},{1},{2},{3},{4}", mess.ItemId, mess.MerchantId, mess.MarkertPlace, mess.Priority, mess.DataType);
            }
            return fm;
        }

        public IList<MessageDTO> applyConfiguration(IList<string> messages)
        {
            try
            {
                if(messages.Count > 0)
                {
                    IList<MessageDTO> orderedMessages = priortizeRequest(messages);
                    IList<MessageDTO> merchantIdMessages = checkMoreMerchantId(orderedMessages);
                    IList<MessageDTO> appliedPayload = applyPayload(merchantIdMessages);
                    return appliedPayload;
                }
                else
                {
                    return new List<MessageDTO>();
                }
               
                //check for more merchantId
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private IList<MessageDTO> priortizeRequest(IList<string> messages)
        {
            try
            {
                IList<MessageDTO> orderedMessages = (messages.Where(m => m != null).Select(str => new MessageDTO
                {
                    ItemId = str.Split(',')[0],
                    MerchantId = Convert.ToInt32(str.Split(',')[1]),
                    MarkertPlace = Convert.ToInt32(str.Split(',')[2]),
                    Priority = str.Split(',')[3],
                    DataType = str.Split(',')[4],
                    Payload = str.Split(',')[5],
                    PriorirtId = Convert.ToInt32((str.Split(',')[3] == priorties[0].Value ? priorties[0].Key : (str.Split(',')[3] == priorties[1].Value ? priorties[1].Key
                            : (str.Split(',')[3] == priorties[2].Value ? priorties[2].Key : (str.Split(',')[3] == priorties[3].Value
                                ? priorties[3].Key : priorties[4].Key))))),
                    requestTime = Convert.ToDateTime(str.Split(',')[6])
                }).OrderBy(mess => mess.PriorirtId)).ToList();
                return orderedMessages;
            }
            catch (FormatException fe)
            {
                throw fe;
            }
            catch(Exception ex)
            {
                throw ex;
            }      
        }

        private IList<MessageDTO> applyPayload(IList<MessageDTO> messages)
        {
            int payload = tc.getTargetLoad();
            if(messages.Count > payload )
            {
                IList<MessageDTO> partialMessages = messages.Take(payload).ToList();
                IList<MessageDTO> remainingMessages = messages.Except(partialMessages, mc).ToList();
                return partialMessages;
            }
            else
            {
                return messages;
            }
        }

       private IList<MessageDTO> checkMoreMerchantId(IList<MessageDTO> messages)
        {
            IList<MessageDTO> finalMessages = new List<MessageDTO>();
           int maxMerchant = tc.getMaxMerchantId();
           int time = tc.getTime();
            var groupedMessages = messages.OrderBy(m => m.requestTime).GroupBy(m => m.MerchantId).Select(m => m).ToList();
            IList<int> uniqueMerchantIds = messages.Select(m => m.MerchantId).Distinct().ToList();
           if(uniqueMerchantIds.Count < messages.Count)
           {
               foreach (int merhcantId in uniqueMerchantIds)
               {
                   int Count = groupedMessages.Where(mg => mg.Key == merhcantId).Count();
                   if (MaintainMerchants.checkKey(merhcantId))
                   {
                       MerchantDTO merchant = new MerchantDTO();
                       merchant.MerhchantId = merhcantId;
                       merchant.date = DateTime.Now;

                       if (Count <= maxMerchant)
                       {
                           merchant.count = Count;
                           MaintainMerchants.MerchantIdCounts.Add(merchant);
                       }
                       else
                       {
                           merchant.count = maxMerchant;
                           foreach (MessageDTO mess in groupedMessages.Where(mg => mg.Key == merhcantId).Take(Count))
                           {
                               finalMessages.Add(mess);
                           }
                           //save remaining messages
                       }
                   }
                   else
                   {
                       MerchantDTO mm = MaintainMerchants.MerchantIdCounts.Where(m => m.MerhchantId == merhcantId).Select(m => m).FirstOrDefault();
                       IList<MessageDTO> sameMerchantMessags = new List<MessageDTO>();
                       foreach (MessageDTO mess in groupedMessages.Where(mg => mg.Key == merhcantId && mg.Where(m => m.requestTime >= mm.date && m.requestTime <= mm.date.AddHours(time)).Any()))
                       {
                           sameMerchantMessags.Add(mess);
                       }
                       if (mm.count + sameMerchantMessags.Count <= maxMerchant)
                       {
                           foreach (MessageDTO mess in sameMerchantMessags)
                           {
                               finalMessages.Add(mess);
                           }
                       }
                       else
                       {
                           // save remai
                       }

                   }
               }
           }
           else
           {
               finalMessages = messages;
           }
           
           return finalMessages;
        }

       // public int saveMessages(string[] Messages)
       //{
       //    try
       //    {
       //        WriteInFile(path.getPath1(), Messages);
       //        WriteInFile(path.getPath2(), Messages);
       //        return 1;
       //    }
       //     catch(Exception ex)
       //    {
       //        throw ex;
       //    } 
       //}

        //TODO combining it with NO sql DB
        //public void LogRecords(int number)
        //{

        //}

        //private void WriteInFile(string filePath, string[] Messages)
        //{
        //    try
        //    {
        //        using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        //        {
        //            StreamWriter File = new StreamWriter(fs);
        //            for (int i = 0; i < Messages.Length; i++)
        //            {
        //                File.WriteLine(Messages[i]);
        //            }
        //            File.Close();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }   
        //}
    }
}
