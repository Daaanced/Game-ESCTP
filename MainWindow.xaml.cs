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
            _timberman = new Timberman(GameField, "./imgs/timberman2.png");
            _tree = new Tree(GameField, "./imgs/log.png");

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
            _body.RenderTransform = new ScaleTransform { ScaleX = -1 };
            Canvas.SetLeft(_body, _leftPosition);
        }
        public void MoveRight()
        {
            _body.RenderTransform = new ScaleTransform { ScaleX = 1 };
            Canvas.SetLeft(_body, _rightPosition);
        }

    }

    public class Tree
    {
        private int _height = 10;
        private int _leftPosition = 210;
        private int _bottomPosition = 20;
        private int _itemSize = 50;
        private List<int> _itemsBottomPositions;
        private List<TreeItem> _items = new List<TreeItem>();
        private string _imagePath;

        public Tree(Canvas field, string imagePath)
        {
            _imagePath = imagePath;
            _itemsBottomPositions = new List<int>();
            for (int i = 0; i < _height; i++)
            {
                _items.Add(new TreeItem(field, _itemSize, _leftPosition, _imagePath));
                _itemsBottomPositions.Add(_bottomPosition + i * _itemSize);
            }
            Draw();
        }

        public void Chop(Canvas field)
        {
            _items.RemoveAt(0);
            _items.Add(new TreeItem(field, _itemSize, _leftPosition, _imagePath));
            Draw();
        }

        public void Draw()
        {
            for (int i = 0; i < _height; i++)
            {
                _items[i].MoveDown(_itemsBottomPositions[i]);
            }
        }

    }

    public class TreeItem 
    {
        private int _size;
        private TextBlock _body;
        static int count = 0;

        public TreeItem(Canvas field, int size, int left_position, string imagePath) 
        {
            _size = size;
            _body = new TextBlock {Width = _size, Height = _size};
            _body.Text = count.ToString();
            _body.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative))
            };
            field.Children.Add(_body);
            Canvas.SetLeft(_body, left_position);
            count++;
        }

        public void MoveDown(int bottom_position)
        {
            Canvas.SetBottom(_body, bottom_position);
        }

        public void Delete(Canvas field) 
        {
            field.Children.Remove(_body);
        }

    }
    

}
