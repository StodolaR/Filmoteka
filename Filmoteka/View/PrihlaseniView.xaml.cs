﻿using Filmoteka.ViewModel;
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

namespace Filmoteka.View
{
    /// <summary>
    /// Interaction logic for PrihlaseniView.xaml
    /// </summary>
    public partial class PrihlaseniView : UserControl
    {
        public PrihlaseniView()
        {
            InitializeComponent();
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (UserCollectionViewModel)DataContext;
            viewModel.FormReset.Execute(null);
        }
    }
}
