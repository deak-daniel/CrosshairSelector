using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public enum CrosshairEventFlags
    {
        None,
        NewCrosshairRequested
    }
    public class CrosshairModifiedEventArgs : EventArgs
    {
        public ICrosshair? Crosshair { get; private set; }
        public CrosshairEventFlags? Flag { get; private set; }
        public CrosshairModifiedEventArgs() 
            : base()
        { }
        public CrosshairModifiedEventArgs(Crosshair crosshair, CrosshairEventFlags flag = CrosshairEventFlags.None)
            : base()
        {
            Crosshair = crosshair;
            Flag = flag;
        }
    }
}
