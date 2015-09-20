using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModels.KeyValuePairs
{
    public class KeyValues
    {
        private IList<KeyValuePair<int, string>> priorityPairs = new List<KeyValuePair<int, string>> { 
                new KeyValuePair<int,string>(1,"Highest"),
                new KeyValuePair<int, string>(2,"High"),
                new KeyValuePair<int,string>(3,"Normal"),
                new KeyValuePair<int,string>(4,"Low"),
                new KeyValuePair<int,string>(5,"Lowest")
            };

       private IList<KeyValuePair<int, string>> datatypes = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int,string>(1,"ITEM"),
                new KeyValuePair<int,string>(2,"AUX_ITEM"),
                new KeyValuePair<int,string>(3,"PRICE"),
                new KeyValuePair<int,string>(4,"PRODUCT"),
            };

       public IList<KeyValuePair<int, string>> getPriorities()
       {
           return priorityPairs;
       }

       public IList<KeyValuePair<int, string>> getDataTypes()
       {
           return datatypes;
       }
    }
}
