using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TheChosenOne.Themes
{
    public class ThemeController
    {
        public static readonly List<Theme> themes;
        public static string currentTheme { get; private set; }
    
        static ThemeController()
        {
            themes = new List<Theme>();
            themes.Add(new Theme("默认主题", "DefaultTheme", "AliceBlue"));
            themes.Add(new Theme("深色主题", "DarkTheme", "#232323"));
        }

        public static void ChangeTheme(string themePath)
        {
            Uri uri = new Uri($"Themes/{themePath}.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = uri };
            currentTheme = themePath;
        }
    }

    public class Theme
    {
        public string name { get; set; }
        public string path { get; set; }
        public SolidColorBrush brush { get; set; }

        public Theme(string n, string p, string b)
        {
            name = n;
            path = p;
            brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(b));
        }
    }
}
