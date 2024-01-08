using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.CodeDom.Compiler;
using System.Windows.Threading;

namespace WpfApp2
{
    public class Tree
    {
        public int _previousType;
        private int _height = 10;
        private int _leftPosition = 210;
        private int _bottomPosition = 20;
        private int _itemSize = 75;
        private List<int> _itemsBottomPositions;
        public List<TreeItem> Items = new List<TreeItem>();

        public Tree(Canvas field)
        {

            _itemsBottomPositions = new List<int>();
            for (int i = 0; i < _height; i++)
            {
                Generate(field);              
                _itemsBottomPositions.Add(_bottomPosition + i * _itemSize);
            }
            Draw();
        } 

        private void Generate(Canvas field)
        {
            TreeItem.TreeType newType;
            if (_previousType == 0)
                newType = TreeItem.TreeType.Middle;
            else
            {
                do
                {
                    Random random = new Random();
                    newType = (TreeItem.TreeType) random.Next(1, 4);
                } while (Math.Abs((int)(_previousType - newType)) > 1);
            }
            _previousType = (int)newType;
            Items.Add(new TreeItem(field, newType));
        }

        public void Chop(Canvas field, Timberman timberman)
        {
            // Получаем первый элемент и удаляем его из списка
            TreeItem choppedItem = Items[0];
            Items.RemoveAt(0);

            // Создаем и добавляем новый элемент
             Generate(field);

            // Запускаем анимацию вращения для предмета в течение 0.4 секунд
            DoubleAnimation rotationAnimation = new DoubleAnimation
            {
                To = timberman.IsLeft ? 360 : -360, // Полный оборот
                Duration = TimeSpan.FromSeconds(0.4)
            };

            // Привязываем анимацию к свойству RenderTransform.Angle элемента
            choppedItem.Body.RenderTransformOrigin = new Point(0.5, 0.5);
            choppedItem.Body.RenderTransform = new RotateTransform();
            choppedItem.Body.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);

            // Анимация перемещения на 300 в течение 0.4 секунд
            DoubleAnimation positionAnimation = new DoubleAnimation
            {
                To = timberman.IsLeft ? choppedItem.LeftPosition + 300 : choppedItem.LeftPosition - 300,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            // Привязываем анимацию к свойству Canvas.Left элемента
            Storyboard.SetTarget(positionAnimation, choppedItem.Body);
            Storyboard.SetTargetProperty(positionAnimation, new PropertyPath(Canvas.LeftProperty));

            // Создаем и запускаем Storyboard для перемещения
            Storyboard positionStoryboard = new Storyboard();
            positionStoryboard.Children.Add(positionAnimation);

            // Удаляем старый элемент после завершения анимации перемещения
            positionStoryboard.Completed += (sender, e) =>
            {
                choppedItem.Delete(field);
            };
            positionStoryboard.Begin();

            Draw(); // Перерисовываем дерево после анимаций
        }

        public void Draw()
        {
            for (int i = 0; i < _height; i++)
            {
                Items[i].MoveDown(_itemsBottomPositions[i]);
            }
        }

        public void Delete(Canvas field)
        {
            for (int i = 0; i < _height; i++)
            {
                Items[i].Delete(field);
            }
        }
    }
    public class TreeItem
    {
        public enum TreeType 
        {
            Left = 1,
            Middle = 2,
            Right = 3,
        }

        public int Type;
        int left_position = 275;
        private int _size = 150;
        private int _height = 75;
        private int _branch_size = 75;
        private Rectangle _body;
        private string _imagePath = "./imgs/log.png";
        private string _imagePathR = "./imgs/logWithBranchR.png";
        private string _imagePathL = "./imgs/logWithBranchL.png";
        public Rectangle Body => _body; // Добавляем свойство для доступа к _body извне
        public double LeftPosition => Canvas.GetLeft(_body); // Добавляем свойство для получения текущей позиции по горизонтали
        public TreeItem(Canvas field, TreeType newType)
        {
            
            Type = (int)newType;
            string imagePath;
            switch (newType)
            {
                case TreeType.Left:
                    imagePath = _imagePathL;
                    left_position -= _branch_size;
                    break;

                case TreeType.Right:
                    imagePath = _imagePathR;
                    break;

                default:
                    imagePath = _imagePath;
                    _size -= _branch_size;
                    break;
            }
            _body = new Rectangle { Width = _size, Height = _height };

            _body.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative))
            };
            field.Children.Add(_body);
            Canvas.SetLeft(_body, left_position);          
            
        }

        public void MoveDown(int bottom_position)
        {
            // Запускаем анимацию для предмета в течение 0.1 секунд
            DoubleAnimation animation = new DoubleAnimation
            {
                To =bottom_position,
                Duration = TimeSpan.FromSeconds(0.1)
            };

            // Привязываем анимацию к свойству Canvas.Bottom элемента
            Storyboard.SetTarget(animation, Body);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.BottomProperty));

            // Создаем и запускаем Storyboard
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);

            // Вылетает ошибка если не задать что-нибудь в Completed
            storyboard.Completed += (sender, e) =>
            {};
            storyboard.Begin();
            Canvas.SetBottom(_body, bottom_position);
        }

        public void Delete(Canvas field)
        {
            field.Children.Remove(_body);
        }

    }
}
