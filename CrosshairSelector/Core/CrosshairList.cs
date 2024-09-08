using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    [DataContract]
    public class CrosshairList
    {
        [DataMember]
        public List<Crosshair> list { get; set; }
        public int Count{
            get { 
                return list.Count; 
            }  
        }
        public CrosshairList()
        {
            list = new List<Crosshair>();
        }

        public void Add(Crosshair item)
        {
            list.Add(item);
        }
    }
}
