using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace WpfApp2
{
    public class Timberman
    {
        public bool IsLeft = true;
        private int _size = 50;
        private Rectangle _body;
        private int _bottomPosition = 20;
        private int _leftPosition = 150;
        private int _rightPosition = 270;
        private string _imagePath = "./imgs/timberman.png";
        public Timberman(Canvas field)
        {
            // Создаем дровосека
            string imagePath = _imagePath;
            this._body = new Rectangle { Name = "_timberman", Width = _size, Height = _size };
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
            IsLeft = true;
        }
        public void MoveRight()
        {
            _body.RenderTransform = new ScaleTransform { ScaleX = -1 };
            Canvas.SetLeft(_body, _rightPosition);
            IsLeft = false;
        }
    }
}
