using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public class CrosshairsRequestedEventArgs : EventArgs
    {
        public List<Crosshair> Crosshairs { get; set; }
        public CrosshairsRequestedEventArgs(List<Crosshair> crosshairs) 
            : base()
        {
            Crosshairs = crosshairs;    
        }
    }
}
