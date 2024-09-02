using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public interface ICrosshairView
    {
        void Modify(ICrosshair crosshair);
    }
}
