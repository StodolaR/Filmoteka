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
    /// Interaction logic for RatingBlock.xaml
    /// </summary>
    public partial class RatingBlock : UserControl
    {
        UIElementCollection starLabels;
        public int RatingValue
        {
            get { return (int)GetValue(RatingValueProperty); }
            set { SetValue(RatingValueProperty, value); }
        }
        public static readonly DependencyProperty RatingValueProperty =
            DependencyProperty.Register("RatingValue", typeof(int), typeof(RatingBlock), new PropertyMetadata(OnSourcePropertyChanged));
        public static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {      
            RatingBlock control = d as RatingBlock;
            control.RatingValue = (int)e.NewValue;
            control. starLabels = control. StarPanel.Children;
            for (int i = 0; i < control. RatingValue; i++)
            {
                (control. starLabels[i] as Label).Width = 15;
                (control.starLabels[i] as Label).Height = 15;
                (control.starLabels[i] as Label).Content = control.StarCreating(Brushes.Goldenrod);
            }
            for (int i = control.RatingValue; i < 5; i++)
            {               
                (control. starLabels[i] as Label).Width = 15;
                (control.starLabels[i] as Label).Height = 15;
                (control.starLabels[i] as Label).Content = control.StarCreating(Brushes.Black);
            }
        }
        public RatingBlock()
        {
            InitializeComponent();           
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
