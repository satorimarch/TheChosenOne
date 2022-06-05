using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private bool hasAni;
        private bool aniIsShowing;

        private readonly setting1 setting = setting1.Default;
        private readonly Random random = new Random();
        private readonly Timer timerChangeNum = new Timer();
        private readonly Timer timerPaintNum = new Timer();
        private readonly Timer timerAnimation = new Timer();
        private readonly DoubleAnimation dAniScroll = new DoubleAnimation();
        private readonly DoubleAnimation dAniReturn = new DoubleAnimation();


        public MainWindow()
        {
            gameStart = false;

            timerPaintNum.Elapsed += (o, e) =>
            {
                TBlockNum.Dispatcher.BeginInvoke(
                    new Action(() =>
                        {
                            TBlockNum.Text = currNum.ToString();
                        }
                    )
                );
            };

            dAniScroll.From = -65;
            dAniScroll.To = 65;

            dAniReturn.From = -65;
            dAniReturn.To = 0;

            timerAnimation.Elapsed += (o, e) =>
            {
                TBlockNum.Dispatcher.Invoke(
                    new Action(() =>
                        {
                            if (aniIsShowing)
                                TBlockNum_tTransform.BeginAnimation(TranslateTransform.YProperty, dAniScroll);
                        }
                    )
                );
            };

            InitializeComponent();
            InitSetting();
        }

        internal void InitSetting()
        {
            timerChangeNum.Interval = setting.Interval;
            timerPaintNum.Interval = setting.DrawInterval;

            timerAnimation.Interval = setting.Interval;
            dAniScroll.Duration = new Duration(TimeSpan.FromMilliseconds(setting.Interval));
            dAniReturn.Duration = new Duration(TimeSpan.FromMilliseconds(setting.Interval));

            hasAni = setting.HasAni;

            minNum = setting.MinNum;
            maxNum = setting.MaxNum;

            if (currNum != 0) TBlockNum.Text = currNum.ToString();
            else TBlockNum.Text = minNum.ToString();

            mode = (Mode)setting.Mode;

            switch (mode) {
                case Mode.Sequential:
                case Mode.FirstRandom:
                    timerChangeNum.Elapsed -= TickRandom;
                    timerChangeNum.Elapsed -= TickSequential;
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
                if (hasAni) {
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

                if (mode == Mode.FirstRandom || mode == Mode.Random) {
                    currNum = random.Next(minNum, maxNum);
                }
                else currNum = minNum;

                timerChangeNum.Start();
                timerPaintNum.Start();
                if (hasAni) {
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
