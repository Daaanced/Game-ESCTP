using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ArrayItem> Array { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            Array = new ObservableCollection<ArrayItem>();
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                int value = random.Next(1, 4);
                Color color = GetColor(value);
                Array.Add(new ArrayItem { Value = value, Color = new SolidColorBrush(color) });
            }

            canvas1.Focus();
        }
        
        private void NextElement()
        {
            Array.RemoveAt(Array.Count - 1);
            Random random = new Random();
            int value = random.Next(1, 4);
            Color color = GetColor(value);
            Array.Insert(0, new ArrayItem { Value = value, Color = new SolidColorBrush(color) });
        }

        private void Canvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Thickness currentMargin = canvas1.Margin;
            //TranslateTransform trans = new TranslateTransform();
            //Tree.RenderTransform = trans;
            //DoubleAnimation animY = new DoubleAnimation(0,100,TimeSpan.FromSeconds(0.5));
            if (e.Key == Key.A)
            {
                Grid.SetColumn(canvas1, 0);
                currentMargin = new Thickness(50, 0, 0, 50);
               // DoubleAnimation animY = new DoubleAnimation(0, 100, TimeSpan.FromSeconds(0.5));
                //trans.BeginAnimation(TranslateTransform.XProperty, animY);
                NextElement();
            }
            if (e.Key == Key.D)
            {
                Grid.SetColumn(canvas1, 2);
                currentMargin = new Thickness(0, 0, 50, 50);
               // DoubleAnimation animY = new DoubleAnimation(0, -100, TimeSpan.FromSeconds(0.5));
                //trans.BeginAnimation(TranslateTransform.XProperty, animY);
                NextElement();
            }
            canvas1.Margin = currentMargin;
        }
        private Color GetColor(int value)
        {
            // Здесь вы можете определить логику присвоения цвета в зависимости от значения элемента массива
            // В данном примере используется простая логика, где цвет зависит от значения элемента

            switch (value)
            {
                case 1:
                    return Colors.Red;
                case 2:
                    return Colors.Blue;
                case 3:
                    return Colors.Green;
                default:
                    return Colors.Gray;
            }
        }
    }

    public class ArrayItem : INotifyPropertyChanged
    {
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        private SolidColorBrush _color;
        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
