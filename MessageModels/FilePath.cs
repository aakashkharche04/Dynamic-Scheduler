using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModels
{
    public class FilePath
    {
        private string path1 = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, "data1.txt");
        private string path2 = string.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, "data2.txt");
        private string path3 = "C:/Users/Lenovo/Desktop/log.txt";

        public string getPath1()
        {
            return path1;
        }

        public string getPath2()
        {
            return path2;
        }

        public string getTargetPath()
        {
            return path3;
        }
    }
}
