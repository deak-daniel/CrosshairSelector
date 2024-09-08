using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrosshairSelector
{
    public static class StringExtensions
    {
        public static Key ToKey(this string value)
        {

            Key res = Key.None;
            if (value != null)
            {
                if (value.Length > 0 && char.IsDigit(value[0]))
                {
                    res = (Key)(Convert.ToInt32(value) + (int)Key.D0);
                }
                if (res == Key.None)
                {
                    throw new FormatException("The string was either in a wrong format or was too long.");
                }
            }
            return res;
        }
    }
}
