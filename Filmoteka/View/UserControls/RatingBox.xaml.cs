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
                        ((control.starButtons[i] as Button).Content as Path).Fill = Brushes.Black;
                    }
                }
            }         
        }
        public RatingBox()
        {
            InitializeComponent();
            starButtons = StarPanel.Children;       
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
                    ((starButtons[i] as Button).Content as Path).Fill = Brushes.Goldenrod;
                }
                for (int i = rating + 1; i <= 5; i++)
                {
                    ((starButtons[i] as Button).Content as Path).Fill = Brushes.Black;
                }
            }
        }
    }
}
