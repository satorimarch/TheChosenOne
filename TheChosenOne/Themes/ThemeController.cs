using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace TheChosenOne.Themes
{
    public static class ThemeController
    {
        public static readonly List<Theme> Themes;
        public static string CurrentTheme { get; private set; }

        static ThemeController()
        {
            Themes = new List<Theme>
            {
                new Theme("默认主题", "DefaultTheme", "AliceBlue"),
                new Theme("深色主题", "DarkTheme", "#232323"),
            };
        }

        public static void ChangeTheme(string themePath)
        {
            Uri uri = new Uri($"Themes/{themePath}.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = uri };
            CurrentTheme = themePath;
        }
    }

    public class Theme
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public SolidColorBrush Brush { get; set; }

        public Theme(string n, string p, string b)
        {
            Name = n;
            Path = p;
            Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(b));
        }
    }
}
