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
            control. starLabels = control.StarPanel.Children;
            for (int i = 0; i < control.RatingValue; i++)
            {
                ((control.starLabels[i] as Label).Content as Path).Fill = Brushes.Goldenrod;
            }
        }
        public RatingBlock()
        {
            InitializeComponent();           
        }
    }
}
