using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheChosenOne.Themes;

using Timer = System.Timers.Timer;

namespace TheChosenOne
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        enum Mode
        {
            Sequential,
            FirstRandom,
            Random,
        }

        private Mode mode;
        private int maxNumber;
        private int minNumber;
        private int currNumber;
        private bool gameStart;
        private Random random = new Random();
        private Timer TimerChangeNumber = new Timer();
        private Timer TimerDrawNumber = new Timer();

        public MainWindow()
        {
            gameStart = false;

            TimerDrawNumber.Elapsed += (o, e) =>
            {
                TBNumber.Dispatcher.BeginInvoke(
                    new Action(
                        delegate
                        {
                            TBNumber.Text = currNumber.ToString();
                        }
                    )
                );
            };

            InitializeComponent();
            Init_Setting();
        }


        internal void Init_Setting()
        {
            TimerChangeNumber.Dispose();
            TimerChangeNumber = new Timer();

            TimerChangeNumber.Interval = setting1.Default.Interval;
            TimerDrawNumber.Interval = setting1.Default.DrawInterval;
            minNumber = setting1.Default.MinNumber;
            maxNumber = setting1.Default.MaxNumber;

            TBNumber.Text = minNumber.ToString();

            mode = (Mode)setting1.Default.Mode;

            switch (mode) {
                case Mode.Sequential:
                    TimerChangeNumber.Elapsed += TickSequential;
                    break;

                case Mode.FirstRandom:
                    TimerChangeNumber.Elapsed += TickSequential;
                    break;

                case Mode.Random:
                    TimerChangeNumber.Elapsed += TickRandom;
                    break;
            }

            ThemeController.ChangeTheme(setting1.Default.Theme);
        }


        private void TickSequential(object sender, ElapsedEventArgs e)
        {
            if (currNumber < maxNumber) currNumber++;
            else currNumber = minNumber;
            Console.WriteLine($"{currNumber} id:{Thread.CurrentThread.ManagedThreadId}");
        }


        private void TickRandom(object sender, ElapsedEventArgs e)
        {
            currNumber = random.Next(minNumber, maxNumber);
        }


        private void Button_Click_Pause(object sender, RoutedEventArgs e)
        {
            if (gameStart) {

                TimerChangeNumber.Stop();
                TimerDrawNumber.Stop();
                ButtonPause.Content = "开始";
                gameStart = false;
            }
            else {
                ButtonPause.Content = "结束";
                gameStart = true;
                if (mode == Mode.FirstRandom) {
                    currNumber = random.Next(minNumber, maxNumber);
                }

                TimerChangeNumber.Start();
                TimerDrawNumber.Start();
            }
        }

        private void Button_Click_Open_Setting(object sender, RoutedEventArgs e)
        {
            if (gameStart) {
                Button_Click_Pause(null, null);
            }
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Owner = this;
            settingWindow.ShowDialog();
        }
    }
}
