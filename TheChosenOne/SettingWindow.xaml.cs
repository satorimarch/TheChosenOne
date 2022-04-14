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

            ((RadioButton)FindName("RadioBtn_mode_" + setting.Mode.ToString())).IsChecked = true;
            //throw new NotImplementedException();
        }

        private void Button_Confirm_change(object sender, RoutedEventArgs e)
        {
            try {
                setting.Interval = int.Parse(TextBox_Interval.Text);
                setting.DrawInterval = int.Parse(TextBox_DrawInterval.Text);
                setting.MinNumber = int.Parse(TextBox_MinNumber.Text);
                setting.MaxNumber = int.Parse(TextBox_MaxNumber.Text);
                setting.Mode = CheckedMode;

                ((MainWindow)Owner).Init_Setting();
            }
            catch {
                MessageBox.Show("输入无效", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                this.Close();
                MessageBox.Show("修改成功", "消息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RadionBtn_mode_Checked(object sender, RoutedEventArgs e)
        {
            string name = ((RadioButton)sender).Name;
            CheckedMode = int.Parse(name.Substring(name.LastIndexOf('_') + 1));
        }
    }
}
