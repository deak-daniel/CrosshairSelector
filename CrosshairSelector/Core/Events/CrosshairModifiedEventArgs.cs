using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public class CrosshairModifiedEventArgs : EventArgs
    {
        public ICrosshair? Crosshair { get; private set; }
        public CrosshairModifiedEventArgs() 
            : base()
        { }
        public CrosshairModifiedEventArgs(Crosshair crosshair)
            : base()
        {
            Crosshair = crosshair;
        }
    }
}
