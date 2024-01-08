using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        double multiplyer = 0.5;

        public MainWindow()
        {
            InitializeComponent();
            
            GameField.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/background2.png", UriKind.Relative))
            };

            MainMenuText.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/wood.png", UriKind.Relative))
            };

            EndMenuText.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/wood.png", UriKind.Relative))
            };
            GameMainMenu.IsOpen = true;

        }

        public void NextTurn(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.D && e.Key != Key.A)
                return;
            // перемещаем дровосека
            switch (e.Key)
            {
                case Key.A:
                    _timberman.MoveLeft();
                    break;
                case Key.D:
                    _timberman.MoveRight();
                    break;
            }
            _timer.Start();
            if (_score % 20 == 0) multiplyer += 0.2;
            Check();
            // рубим дерево
            _tree.Chop(GameField,_timberman);
            GameTimer.Value += 3;
            Check();
            UpdateScore(++_score);
        }
        public void Check()
        {
            if (_tree.Items[0].Type == (int)TreeType.Left && _timberman.IsLeft || _tree.Items[0].Type == (int)TreeType.Right && !_timberman.IsLeft)
                GameOver();
        }


        public void GameOver()
        {
            if (_score > _record) _record = _score;
            _timer.Stop();
            GameField.Focusable = false;
            EndGameScoreText.Text = _score.ToString();
            EndGameRecordText.Text = _record.ToString();
            GameEndMenu.IsOpen = true;
            _timberman.Delete(GameField);
            _tree.Delete(GameField);
            multiplyer = 0.5;
        }

        public void NewGame()
        {
            GameEndMenu.IsOpen = false;
            GameMainMenu.IsOpen = false;
            _tree = new Tree(GameField);
            _timberman = new Timberman(GameField);
            GameField.Focusable = true;
            GameField.Focus();

            //TimerCallback tm = new TimerCallback(Count);
            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);           
            GameTimer.Value = 100;
            UpdateScore();

        }

        private void UpdateScore(int score=0)
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

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {           
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)(() => {
                if (GameTimer.Value > 0)
                {
                    GameTimer.Value -= multiplyer;
                }
                else
                {
                    GameOver();
                }
            }));
        }
    }
}
