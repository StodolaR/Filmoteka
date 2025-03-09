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
    /// Interaction logic for PasswordUserControl.xaml
    /// </summary>
    public partial class PasswordUserControl : UserControl
    {
        public string PasswordDP
        {
            get { return (string)GetValue(PasswordDPProperty); }
            set { SetValue(PasswordDPProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PasswordDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordDPProperty =
            DependencyProperty.Register("PasswordDP", typeof(string), typeof(PasswordUserControl), new PropertyMetadata(OnSourcePropertyChanged));
        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue.ToString() == string.Empty)
            {
                PasswordUserControl? control = d as PasswordUserControl;
                if(control != null)
                {
                    control.passwordBox.Password = string.Empty;
                }
            }
        }

        public PasswordUserControl()
        {
            InitializeComponent();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordDP = passwordBox.Password;
        }
    }
}
