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
        private Tumberman _tumberman;
        private Tree _tree;
        public MainWindow()
        {
            InitializeComponent();
            GameField.Focus();
            _tumberman = new Tumberman(GameField);
            _tree = new Tree(GameField);

        }
        
        public void Dbg(object sender, RoutedEventArgs e)
        {
            // здесь можно проводить отладку
        }

        public void NextTurn(object sender, KeyEventArgs e)
        {
            // перемещаем дровосека
            switch (e.Key)
            {
                case Key.A:
                    _tumberman.MoveLeft();
                    break;
                case Key.D:
                    _tumberman.MoveRight();
                    break;
            }
            // рубим дерево
            _tree.Chop(GameField);
            

        }
    }

    public class Tumberman
    {
        private int _size = 50;
        private Rectangle _body;
        private int _bottomPosition = 20;
        private int _leftPosition = 150;
        private int _rightPosition = 270;

        public Tumberman(Canvas field)
        {
            // Создаем дровосека
            this._body = new Rectangle { Name = "_tumberman", Width = _size, Height = _size, Fill= Brushes.LightBlue};
            field.Children.Add(this._body);
            Canvas.SetBottom(_body, _bottomPosition);
            this.MoveLeft();
        }

        public void MoveLeft()
        {
            Canvas.SetLeft(_body, _leftPosition);
        }
        public void MoveRight()
        {
            Canvas.SetLeft(_body, _rightPosition);
        }

    }

    public class Tree
    {
        private int _height = 5;
        private int _leftPosition = 210;
        private int _bottomPosition = 20;
        private int _intervalMargin = 1;
        private int _itemSize = 50;
        private List<int> _itemsBottomPositions;
        private List<TreeItem> _items = new List<TreeItem>();

        public Tree(Canvas field)
        {
            _itemsBottomPositions = new List<int>();
            for (int i = 0; i < _height; i++)
            {
                _items.Add(new TreeItem(field, _itemSize, _leftPosition));
                _itemsBottomPositions.Add(_bottomPosition + i * (_itemSize + _intervalMargin));
            }
            Draw();
        }

        public void Chop(Canvas field)
        {
            _items.RemoveAt(0);
            _items.Add(new TreeItem(field, _itemSize, _leftPosition));
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

        public TreeItem(Canvas field, int size, int left_position) 
        {
            _size = size;
            _body = new TextBlock {Width = _size, Height = _size, Background = Brushes.Brown };
            _body.Text = count.ToString();
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
