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

namespace Filmoteka.View.UserControls
{
    /// <summary>
    /// Interaction logic for RatingBox2.xaml
    /// </summary>
    public partial class RatingBox : UserControl
    {
        UIElementCollection starButtons;

        public int RatingValue
        {
            get { return (int)GetValue(RatingValueProperty); }
            set { SetValue(RatingValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RatingValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RatingValueProperty =
            DependencyProperty.Register("RatingValue", typeof(int), typeof(RatingBox), new PropertyMetadata(OnSourcePropertyChanged));

        public static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == 0)
            {
                RatingBox control = d as RatingBox;
                if (control != null)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        (control.starButtons[i] as Button).Content = control.StarCreating(Brushes.Black);
                    }
                }
            }
           
        }

        public RatingBox()
        {
            InitializeComponent();
            starButtons = StarPanel.Children;
            for (int i = 1; i <= 5; i++)
            {
                (starButtons[i] as Button).Content = StarCreating(Brushes.Black);
                (starButtons[i] as Button).Tag = i;
                (starButtons[i] as Button).Width = 20;
                (starButtons[i] as Button).Click += StarButton_Click;
                
            }
        }

        private void StarButton_Click(object sender, RoutedEventArgs e)
        {
            
            Button starButton = sender as Button;
            int rating = Convert.ToInt32(starButton.Tag);
            RatingValue = rating;
            if (starButton != null)
            {
                for (int i = 1; i <= rating; i++)
                {
                    (starButtons[i] as Button).Content = StarCreating(Brushes.Goldenrod);
                }
                for (int i = rating + 1; i <= 5; i++)
                {
                    (starButtons[i] as Button).Content = StarCreating(Brushes.Black);
                }
            }
        }
        private Path StarCreating(Brush color)
        {
            Path star = new Path();
            star.Data = Geometry.Parse
                ("M 0 4.5 L 4.1 4.3 L 5.8 0 L 7.5 4.3 L 11.5 4.5 L 8.3 7.3 L 9.7 12 L 5.8 9.2 L 2.1 12 L 3.3 7.3 Z");
            star.Fill = color;
            return star;
        }
    }
}
