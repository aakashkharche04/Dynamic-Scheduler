using LogRequest;
using ManageMessages.Enums;
using MessageModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ManageMessages
{
    [Serializable]
    public class HandleMessage
    {
        public int maxMessages = 0;
        public string[] messages = null;
        private FilePath path = null;
        private DownStreamRequest dsr = null;
        private static object locker = new Object();

        public HandleMessage()
        {
            maxMessages = Convert.ToInt32(ConfigurableEnums.Messages.maxMessages);
            messages = new string[maxMessages];
            path = new FilePath();
            dsr = new DownStreamRequest();
        }
        public void receiveMessages(string[] Messages)
        {
            try
            {
                if (Messages.Length > 0)
                {
                    if(MaintainCount.count == 0)
                    {
                        MaintainCount.count = Messages.Length;
                    }
                    else
                    {
                        MaintainCount.count += Messages.Length;
                    }
                   if(MaintainCount.count <= Convert.ToInt32(ConfigurableEnums.Messages.maxMessages))
                   {
                       lock (locker) {
                           using (var fs = new FileStream(path.getPath1(), FileMode.OpenOrCreate, FileAccess.Write,FileShare.Write))
                           {
                               //BinaryFormatter bf = new BinaryFormatter();
                               //bf.Serialize(fs, this);
                               StreamWriter File = new StreamWriter(fs);
                               for (int i = 0; i < Messages.Length; i++)
                               {
                                   File.WriteLine(Messages[i]);
                               }
                               File.Close();
                          } 
                       }
                       
                   }
                    else
                    {
                        MaintainCount.count = 0;
                           if(dsr.RequestLogger() == 1)
                           {
                               File.WriteAllText(path.getPath1(), string.Empty);
                               File.WriteAllText(path.getPath2(), string.Empty);
                           }
                           else
                           {

                           }
                    }
              }
            }
            catch(IOException)
            {

                using (var fs = new FileStream(path.getPath2(), FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.Write))
                {
                    StreamWriter File = new StreamWriter(fs);
                    for (int i = 0; i < Messages.Length; i++)
                    {
                        File.WriteLine(Messages[i]);
                    }
                    File.Close();
                } 
            }
            catch(Exception ex)
            {
                throw ex;
            }
             
            }
   
        //public void saveMessages(string[] Messages)
        //{
        //    try
        //    {
        //        if (dsr.saveMessages(Messages) == 1)
        //        {

        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
