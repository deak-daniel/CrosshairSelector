using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairSelector
{
    public static class PcInformations
    {
        public static Tuple<int, int> GetResolution()
        {
            return new Tuple<int, int>((int)System.Windows.SystemParameters.PrimaryScreenWidth,
                                       (int)System.Windows.SystemParameters.PrimaryScreenHeight);
        }   
        public static Tuple<int,int> GetMiddlePoint()
        {
            Tuple<int, int> screenRes = GetResolution();
            return new Tuple<int, int>(screenRes.Item1 / 2, screenRes.Item2 / 2);
        }
    }
}
