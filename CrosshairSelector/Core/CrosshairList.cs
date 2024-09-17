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
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
        public Crosshair this[int index]
        {
            get { return list[index]; }
            set { list[index] = value ?? default(Crosshair)!; }
        }
        public bool Contains(ICrosshair item)
        {
            return list.Contains(item);
        }
    }
}
