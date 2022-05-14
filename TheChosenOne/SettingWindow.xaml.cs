using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheChosenOne.Themes;

namespace TheChosenOne
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        private int CheckedMode;
        private setting1 setting = setting1.Default;

        public SettingWindow()
        {
            InitializeComponent();
            InitContent();
        }

        private void InitContent()
        {
            TextBox_Interval.Text = setting.Interval.ToString();
            TextBox_DrawInterval.Text = setting.DrawInterval.ToString();
            TextBox_MinNumber.Text = setting.MinNumber.ToString();
            TextBox_MaxNumber.Text = setting.MaxNumber.ToString();

            ComboBox_Theme.ItemsSource = ThemeController.themes;

            int index = 0;
            foreach(Theme item in ComboBox_Theme.Items) {
                if(item.path == ThemeController.currentTheme) {
                    ComboBox_Theme.SelectedIndex = index;
                    break;
                }
                index++;
            }

            ((RadioButton)FindName("RadioBtn_mode_" + setting.Mode.ToString())).IsChecked = true;
            if (setting.AniOn) RadioBtn_ani_1.IsChecked = true;
            else RadioBtn_ani_0.IsChecked = true;
        }

        private void Button_Confirm_change(object sender, RoutedEventArgs e)
        {
            try {
                setting.Interval = int.Parse(TextBox_Interval.Text);
                setting.DrawInterval = int.Parse(TextBox_DrawInterval.Text);
                setting.MinNumber = int.Parse(TextBox_MinNumber.Text);
                setting.MaxNumber = int.Parse(TextBox_MaxNumber.Text);
                setting.Mode = CheckedMode;

                setting.Theme = ((Theme)ComboBox_Theme.SelectedItem).path;

                if (setting.MinNumber > setting.MaxNumber) {
                    int temp = setting.MaxNumber;
                    setting.MaxNumber = setting.MinNumber;
                    setting.MinNumber = temp;
                }
                
                setting.AniOn = (bool)RadioBtn_ani_1.IsChecked;
                if(setting.AniOn && setting.Interval <= 15) {
                    setting.AniOn = false;
                    MessageBox.Show("由于更新间隔过小, 动画已自动关闭", "确认", MessageBoxButton.OK, MessageBoxImage.Warning);                    
                }

                ((MainWindow)Owner).Init_Setting();
            }

            catch (Exception ex){
                MessageBox.Show($"修改失败\n{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            setting.Save();
            this.Close();
            MessageBox.Show("修改成功", "消息", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }

        private void RadionBtn_mode_Checked(object sender, RoutedEventArgs e)
        {
            string name = ((RadioButton)sender).Name;
            CheckedMode = int.Parse(name.Substring(name.LastIndexOf('_') + 1));
        }
    }
}
