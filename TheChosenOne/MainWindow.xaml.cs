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
using System.Windows.Media.Animation;
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
        private int maxNum;
        private int minNum;
        private int currNum;
        private bool gameStart;
        private bool showAni;
        private bool aniIsShowing;

        private setting1 setting = setting1.Default;
        private Random random = new Random();
        private Timer timerChangeNum = new Timer();
        private Timer timerPaintNum = new Timer();
        private Timer timerAnimation = new Timer();
        private DoubleAnimation dAniScroll = new DoubleAnimation();
        private DoubleAnimation dAniReturn = new DoubleAnimation();

        public MainWindow()
        {
            gameStart = false;

            timerPaintNum.Elapsed += (o, e) =>
            {
                TBlockNum.Dispatcher.BeginInvoke(
                    new Action(
                        delegate
                        {
                            if (TBlockNum.Text != currNum.ToString()) {
                                TBlockNum.Text = currNum.ToString();
                            }
                        }
                    )
                );
            };

            dAniScroll.From = -65;
            dAniScroll.To = 65;

            dAniReturn.From = -65;
            dAniReturn.To = 0;

            timerAnimation.Elapsed += ShowAnimation;

            InitializeComponent();
            InitSetting();
        }

        private void ShowAnimation(object sender, ElapsedEventArgs e)
        {
            TBlockNum.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        if (aniIsShowing) {
                            TBlockNum_tTransform.BeginAnimation(TranslateTransform.YProperty, dAniScroll);
                        }
                    }
                )
            );
        }

        internal void InitSetting()
        {
            timerChangeNum.Interval = setting.Interval;
            timerPaintNum.Interval = setting.DrawInterval;

            timerAnimation.Interval = setting.Interval;
            dAniScroll.Duration = new Duration(TimeSpan.FromMilliseconds(setting.Interval));
            dAniReturn.Duration = new Duration(TimeSpan.FromMilliseconds(setting.Interval));

            showAni = setting.ShowAni;

            minNum = setting.MinNum;
            maxNum = setting.MaxNum;


            TBlockNum.Text = minNum.ToString();

            mode = (Mode)setting.Mode;

            switch (mode) {
                case Mode.Sequential:
                    timerChangeNum.Elapsed -= TickRandom;
                    timerChangeNum.Elapsed += TickSequential;
                    break;

                case Mode.FirstRandom:
                    timerChangeNum.Elapsed -= TickRandom;
                    timerChangeNum.Elapsed += TickSequential;
                    break;

                case Mode.Random:
                    timerChangeNum.Elapsed -= TickSequential;
                    timerChangeNum.Elapsed += TickRandom;
                    break;
            }

            ThemeController.ChangeTheme(setting.Theme);
        }


        private void TickSequential(object sender, ElapsedEventArgs e)
        {
            if (currNum < maxNum) currNum++;
            else currNum = minNum;
#if DEBUG
            Console.WriteLine($"{currNum} id:{Thread.CurrentThread.ManagedThreadId}");
#endif
        }


        private void TickRandom(object sender, ElapsedEventArgs e)
        {
            currNum = random.Next(minNum, maxNum);
#if DEBUG
            Console.WriteLine($"{currNum} id:{Thread.CurrentThread.ManagedThreadId}");
#endif
        }


        private void ButtonClick_Pause(object sender, RoutedEventArgs e)
        {
            if (gameStart) {
                timerChangeNum.Stop();
                timerPaintNum.Stop();
                if (showAni) {
                    timerAnimation.Stop();
                    aniIsShowing = false;
                    TBlockNum_tTransform.BeginAnimation(TranslateTransform.YProperty, dAniReturn);
                }
                ButtonPause.Content = "开始";
                gameStart = false;
            }

            else {
                gameStart = true;
                ButtonPause.Content = "结束";
                if (mode == Mode.FirstRandom) {
                    currNum = random.Next(minNum, maxNum);
                }
                timerChangeNum.Start();
                timerPaintNum.Start();
                if (showAni) {
                    aniIsShowing = true;
                    timerAnimation.Start();
                }
            }
        }

        private void ButtonClick_OpenSetting(object sender, RoutedEventArgs e)
        {
            if (gameStart) {
                ButtonClick_Pause(null, null);
            }
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Owner = this;
            settingWindow.ShowDialog();
        }
    }
}
