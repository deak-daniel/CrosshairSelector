using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrosshairSelector
{
    public static class AppTheme
    {
        public static void ChangeTheme(Uri uri)
        {
            ResourceDictionary theme = new ResourceDictionary()
            {
                Source = uri
            };
            ResourceDictionary prev = new ResourceDictionary()
            {
                Source = new Uri("Themes/LightTheme.xaml",UriKind.Relative)
            };
            App.Current.Resources.MergedDictionaries.Remove(prev);
            App.Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}
