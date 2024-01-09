using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static WpfApp2.TreeItem;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private Timberman _timberman;
        private Tree _tree;
        private System.Timers.Timer _timer;
        private int _score;
        private int _record;
        private const string _recordPath = "./record.txt";
        private double _multiplyer = 0.5;
        private bool _isPause = false;


        public MainWindow()
        {
            InitializeComponent();

            GameField.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/background2.png", UriKind.Relative))
            };

            MainMenuPanel.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/wood.png", UriKind.Relative))
            };

            EndMenuPanel.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/wood.png", UriKind.Relative))
            };

            PauseMenuPanel.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/wood.png", UriKind.Relative))
            };
            GameMainMenu.IsOpen = true;

            // Загружаем рекорд из файла / Создаем файл с рекордом если такового нет
            if (File.Exists(_recordPath))
                File.ReadAllBytes(_recordPath);
            else
            {
                _record = 0;
                using (Stream stream = new FileStream(_recordPath, FileMode.OpenOrCreate))
                {
                    var bytes = Encoding.UTF8.GetBytes(_record.ToString());
                    stream.Write(bytes, 0, bytes.Length);
                }
            }


        }

        public void NextTurn(object sender, KeyEventArgs e)
        {
            // перемещаем дровосека
            switch (e.Key)
            {
                case Key.A:
                    _timberman.MoveLeft();
                    break;
                case Key.D:
                    _timberman.MoveRight();
                    break;
                case Key.P:
                    Pause();
                    return;

                default: return;
            }
            _timer.Start();
            if (_score % 20 == 0) _multiplyer += 0.2;
            Check();
            // рубим дерево
            _tree.Chop(GameField, _timberman);
            GameTimer.Value += 3;
            UpdateScore(++_score);
            Check();
        }
        public void Check()
        {
            if (_tree.Items[0].Type == (int)TreeType.Left && _timberman.IsLeft || _tree.Items[0].Type == (int)TreeType.Right && !_timberman.IsLeft)
                GameOver();
        }


        public void GameOver()
        {
            if (_score > _record)
            {
                _record = _score;
                using (Stream stream = new FileStream("./record.txt", FileMode.Open))
                {
                    var bytes = Encoding.UTF8.GetBytes(_record.ToString());
                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            _timer.Stop();
            GameTimer.Visibility = Visibility.Hidden;
            GameField.Focusable = false;
            EndGameScoreText.Text = _score.ToString();
            EndGameRecordText.Text = _record.ToString();
            GameEndMenu.IsOpen = true;
            _timberman.Delete(GameField);
            _tree.Delete(GameField);
            _multiplyer = 0.5;
        }

        public void NewGame()
        {
            GameEndMenu.IsOpen = false;
            GameMainMenu.IsOpen = false;
            _tree = new Tree(GameField);
            _timberman = new Timberman(GameField);
            GameField.Focusable = true;
            GameField.Focus();


            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            GameTimer.Visibility = Visibility.Visible;
            GameTimer.Value = 100;
            UpdateScore();

        }

        public void Pause()
        {
            _timer.Stop();
            GamePauseMenu.IsOpen = true;
            GameField.Focusable = false;
            ContinueButton.Focus();
        }

        public void Continue()
        {
            _timer.Start();
            GameField.Focusable = true;
            GameField.Focus();
            GamePauseMenu.IsOpen = false;
        }

        private void UpdateScore(int score = 0)
        {
            _score = score;
            ScoreText.Text = "Счет: " + _score.ToString();
        }

        public void NewGameButtonHandler(object sender, EventArgs e)
        {
            NewGame();
        }

        public void ExitButtonHandler(object sender, EventArgs e)
        {
            this.Close();
        }
        private void NewGameButton1_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("MouseEnter event fired");
            ((Button)sender).Background = Brushes.Blue;
        }

        private void NewGameButton1_MouseLeave(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("MouseLeave event fired");
            ((Button)sender).Background = Brushes.Brown;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => {
                if (GameTimer.Value > 0)
                {
                    GameTimer.Value -= _multiplyer;
                }
                else
                {
                    GameOver();
                }
            }));
        }

        private void ContinueButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
                Continue();
            e.Handled = true;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            Continue();
        }
    }
}
