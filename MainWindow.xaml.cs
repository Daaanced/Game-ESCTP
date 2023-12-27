using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            canvas1.Focus();
        }
        private void Canvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Thickness currentMargin = canvas1.Margin;
            
            if (e.Key == Key.A)
            {
                Grid.SetColumn(canvas1, 0);
                currentMargin = new Thickness (50,0,0,50);

            }
            if (e.Key == Key.D)
            {
                Grid.SetColumn(canvas1, 2);
                currentMargin = new Thickness(00, 0, 50, 50);
            }
            canvas1.Margin = currentMargin;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CreateSquares();
        }
        private void CreateSquares()
        {
            int[] values = { 1, 2, 3 };
            Random random = new Random();

            // Перемешиваем значения в массиве
            for (int i = 0; i < values.Length; i++)
            {
                int randomIndex = random.Next(i, values.Length);
                int temp = values[i];
                values[i] = values[randomIndex];
                values[randomIndex] = temp;
            }

            // Создаем и выводим квадраты на экран
            for (int i = 0; i < values.Length; i++)
            {
                int value = values[i];
                Color color;

                // Определяем цвет в зависимости от значения
                switch (value)
                {
                    case 1:
                        color = Colors.Red;
                        break;
                    case 2:
                        color = Colors.Green;
                        break;
                    case 3:
                        color = Colors.Blue;
                        break;
                    default:
                        color = Colors.Black;
                        break;
                }

                // Создаем квадрат и устанавливаем его свойства
                Rectangle square = new Rectangle();
                square.Width = 50;
                square.Height = 50;
                square.Fill = new SolidColorBrush(color);

                // Располагаем квадраты снизу-вверх во втором столбце
                Grid.SetColumn(square, 1);
                Grid.SetRow(square, values.Length - i - 1);

                // Добавляем квадрат на Grid
                grid.Children.Add(square);
            }
        }
    }

}
