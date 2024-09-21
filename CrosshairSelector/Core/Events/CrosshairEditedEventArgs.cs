using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public class CrosshairEditedEventArgs : EventArgs
    {
        public string? CrosshairName { get; private set; }
        public CrosshairEditedEventArgs(string name) 
            : base()
        {
            CrosshairName = name;
        }
    }
}
