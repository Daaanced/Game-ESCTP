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
        private int _size = 100;
        private Rectangle _body;
        private int _bottomPosition = 20;
        private int _leftPosition = 175;
        private int _leftPosition1 = 215;
        private int _rightPosition = 345;
        private int _rightPosition1 = 310;
        private string _imagePath = "./imgs/timberman.png";
        private string _imagePath1 = "./imgs/timbermanChop.png";
        public Timberman(Canvas field)
        {
            // Создаем дровосека
            string imagePath = _imagePath;
            this._body = new Rectangle { Name = "_timberman", Width = _size, Height = _size};
            Canvas.SetZIndex(_body, 1);
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

        public async void MoveLeft()
        {

            // Устанавливаем трансформацию и позицию для движения влево
            _body.RenderTransform = new ScaleTransform { ScaleX = 1 };
            Canvas.SetLeft(_body, _leftPosition1);
            IsLeft = true;

            UpdateImage(_imagePath1);
            await Task.Delay(100);
            Canvas.SetLeft(_body, _leftPosition);
            UpdateImage(_imagePath);
        }

        public async void MoveRight()
        {
            // Устанавливаем трансформацию и позицию для движения вправо
            _body.RenderTransform = new ScaleTransform { ScaleX = -1 };
            Canvas.SetLeft(_body, _rightPosition1);
            IsLeft = false;

            UpdateImage(_imagePath1);
            await Task.Delay(100);
            Canvas.SetLeft(_body, _rightPosition);
            UpdateImage(_imagePath);
        }

        private void UpdateImage(string imagePath)
        {
            // Обновляем изображение
            _body.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative))
            };
        }

        public void Delete(Canvas field)
        {
            field.Children.Remove(_body);
        }
    }
}
