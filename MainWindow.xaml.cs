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
        private Tumberman tumberman;
        private Tree tree;
        public MainWindow()
        {
            InitializeComponent();
            game_field.Focus();
            tumberman = new Tumberman(game_field);
            tree = new Tree(game_field);

        }
        
        public void Dbg(object sender, RoutedEventArgs e)
        {
            tree.Chop(game_field);
        }

        public void NextTurn(object sender, KeyEventArgs e)
        {
            // перемещаем дровосека
            switch (e.Key)
            {
                case Key.A:
                    tumberman.MoveLeft();
                    break;
                case Key.D:
                    tumberman.MoveRight();
                    break;
            }
            // рубим дерево
            tree.Chop(game_field);
            

        }
    }

    public class Tumberman
    {
        private int _size = 50;
        private Rectangle _body;
        private int _bottom_position = 20;
        private int _left_position = 150;
        private int _right_position = 270;

        public Tumberman(Canvas field)
        {
            // Создаем дровосека
            this._body = new Rectangle { Name = "tumberman", Width = _size, Height = _size, Fill= Brushes.LightBlue};
            field.Children.Add(this._body);
            Canvas.SetBottom(_body, _bottom_position);
            this.MoveLeft();
        }

        public void MoveLeft()
        {
            Canvas.SetLeft(_body, _left_position);
        }
        public void MoveRight()
        {
            Canvas.SetLeft(_body, _right_position);
        }

    }

    public class Tree
    {
        private int _height = 5;
        private int _left_position = 210;
        private int _bottom_position = 20;
        private int _interval_margin = 1;
        private int _item_size = 50;
        private List<int> _items_bottom_positions;
        private List<TreeItem> _items = new List<TreeItem>();

        public Tree(Canvas field)
        {
            _items_bottom_positions = new List<int>();
            for (int i = 0; i < _height; i++)
            {
                _items.Add(new TreeItem(field, _item_size, _left_position));
                _items_bottom_positions.Add(_bottom_position + i * (_item_size + _interval_margin));
            }
            Draw();
        }

        public void Chop(Canvas field)
        {
            _items.RemoveAt(0);
            _items.Add(new TreeItem(field, _item_size, _left_position));
            Draw();
        }

        public void Draw()
        {
            for (int i = 0; i < _height; i++)
            {
                _items[i].MoveDown(_items_bottom_positions[i]);
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
