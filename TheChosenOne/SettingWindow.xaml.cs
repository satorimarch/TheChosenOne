using System;
using System.Windows;
using System.Windows.Controls;
using TheChosenOne.Themes;

namespace TheChosenOne
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        private int checkedMode;
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
            TextBox_MinNumber.Text = setting.MinNum.ToString();
            TextBox_MaxNumber.Text = setting.MaxNum.ToString();

            ComboBox_Theme.ItemsSource = ThemeController.Themes;

            int index = 0;
            foreach (Theme item in ComboBox_Theme.Items) {
                if (item.Path == ThemeController.CurrentTheme) {
                    ComboBox_Theme.SelectedIndex = index;
                    break;
                }
                index++;
            }

            ((RadioButton)FindName("RadioBtn_mode_" + setting.Mode.ToString())).IsChecked = true;
            if (setting.HasAni) RadioBtn_ani_1.IsChecked = true;
            else RadioBtn_ani_0.IsChecked = true;
        }

        private void ButtonClick_ConfirmChange(object sender, RoutedEventArgs e)
        {
            try {
                setting.Interval = int.Parse(TextBox_Interval.Text);
                setting.DrawInterval = int.Parse(TextBox_DrawInterval.Text);
                setting.MinNum = int.Parse(TextBox_MinNumber.Text);
                setting.MaxNum = int.Parse(TextBox_MaxNumber.Text);
                setting.Mode = checkedMode;

                setting.Theme = ((Theme)ComboBox_Theme.SelectedItem).Path;

                if (setting.MinNum > setting.MaxNum) {
                    (setting.MinNum, setting.MaxNum) = (setting.MaxNum, setting.MinNum);
                }

                setting.HasAni = (bool)RadioBtn_ani_1.IsChecked;
                if (setting.HasAni && setting.Interval <= 15) {
                    setting.HasAni = false;
                    MessageBox.Show("由于更新间隔过小, 动画已自动关闭", "确认", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                ((MainWindow)Owner).InitSetting();
            }

            catch (Exception ex) {
                MessageBox.Show($"修改失败\n{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            setting.Save();
            this.Close();
            MessageBox.Show("修改成功", "消息", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void RadionBtnChecked_Mode(object sender, RoutedEventArgs e)
        {
            string name = ((RadioButton)sender).Name;
            checkedMode = int.Parse(name.Substring(name.LastIndexOf('_') + 1));
        }
    }
}
