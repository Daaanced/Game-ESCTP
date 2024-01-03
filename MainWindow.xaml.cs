using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MainWindow()
        {
            InitializeComponent();
            GameField.Focus();
            GameField.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("./imgs/background2.png", UriKind.Relative))
            };
            _timberman = new Timberman(GameField, "./imgs/timberman.png");
            _tree = new Tree(GameField);
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
            

        }
    }

    public class Timberman
    {
        private int _size = 50;
        private Rectangle _body;
        private int _bottomPosition = 20;
        private int _leftPosition = 150;
        private int _rightPosition = 270;

        public Timberman(Canvas field, string imagePath)
        {
            // Создаем дровосека
            this._body = new Rectangle { Name = "_timberman", Width = _size, Height = _size};
            _body.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative))
            };
            // задаем точку вокруг которой будут происходить трансформации (конкретно отражение картинки по горизонтали)
            _body.RenderTransformOrigin = new Point(0.5, 0.5);
            field.Children.Add(this._body);
            Canvas.SetBottom(_body, _bottomPosition);
            // дровосек в начале игры стоит слева
            this.MoveLeft();
        }

        public void MoveLeft()
        {
            _body.RenderTransform = new ScaleTransform { ScaleX = 1 };
            Canvas.SetLeft(_body, _leftPosition);
        }
        public void MoveRight()
        {
            _body.RenderTransform = new ScaleTransform { ScaleX = -1 };
            Canvas.SetLeft(_body, _rightPosition);
        }

    }

}
