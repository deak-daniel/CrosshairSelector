using CrosshairSelector.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public class PageChangedEventArgs : EventArgs
    {
        public CrosshairConfigPage Source { get; set; }
        public PageChangedEventArgs(CrosshairConfigPage source) 
            : base()
        {
            Source = source;
        }
    }
}
