using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WpfApp2
{
    public class Tree
    {
        private int _height = 10;
        private int _leftPosition = 210;
        private int _bottomPosition = 20;
        private int _itemSize = 50;
        private List<int> _itemsBottomPositions;
        private List<TreeItem> _items = new List<TreeItem>();

        public Tree(Canvas field)
        {
            _itemsBottomPositions = new List<int>();
            for (int i = 0; i < _height; i++)
            {
                _items.Add(new TreeItem(field));
                _itemsBottomPositions.Add(_bottomPosition + i * _itemSize);
            }
            Draw();
        }

        public void Chop(Canvas field)
        {
            _items[0].Delete(field);
            _items.RemoveAt(0);
            _items.Add(new TreeItem(field));
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
        int left_position = 210;
        private int _size = 100;
        private int _height = 50;
        private TextBlock _body;
        static int count = 0;
        private string _imagePath = "./imgs/log.png";
        private string _imagePathR = "./imgs/logWithBranchR.png";
        private string _imagePathL = "./imgs/logWithBranchL.png";
        public TreeItem(Canvas field)
        {
            Random random = new Random();
            int value = random.Next(1, 4);
            string imagePath;
            switch (value)
            {
                case 2:
                    imagePath = _imagePathL;
                    left_position = 160;
                    break;

                case 3:
                    imagePath = _imagePathR;
                    break;

                default:
                    imagePath = _imagePath;
                    _size = 50;
                    break;
            }
            _body = new TextBlock { Width = _size, Height = _height };
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
