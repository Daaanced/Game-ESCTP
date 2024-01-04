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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private Timberman _timberman;
        private Tree _tree;
        private System.Timers.Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            
            GameField.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/background2.png", UriKind.Relative))
            };
            NewGame();

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
            }
            // рубим дерево
            _tree.Chop(GameField);
            GameTimer.Value += 3;
            Check();
        }
        public void Check()
        {
            if (_tree.Items[0].Type == 2 && _timberman.IsLeft || _tree.Items[0].Type == 3 && !_timberman.IsLeft)
            {
                GameOver();
            }
        }


        public void GameOver()
        {
            _timer.Stop();
            GameField.Focusable = false;
            GameEndMenu.IsOpen = true;
            _timberman.Delete(GameField);
            _tree.Delete(GameField);

        }

        public void NewGame()
        {
            GameEndMenu.IsOpen = false;
            _timberman = new Timberman(GameField);
            _tree = new Tree(GameField);
            GameField.Focusable = true;
            GameField.Focus();

            //TimerCallback tm = new TimerCallback(Count);
            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timer.Start();
            GameTimer.Value = 100;

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
                    GameTimer.Value -= 1;
                }
                else
                {
                    GameOver();
                }
            }));
        }

    }
}
