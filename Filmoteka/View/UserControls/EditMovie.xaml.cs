﻿using Filmoteka.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for EditMovie.xaml
    /// </summary>
    public partial class EditMovie : UserControl
    {
        public EditMovie()
        {
            InitializeComponent();
            UserCollectionViewModel userCollectionViewModel = (UserCollectionViewModel)Application.Current.FindResource("userCollectionViewModel");
            MovieCollectionViewModel movieCollectionViewModel = (MovieCollectionViewModel)Application.Current.FindResource("movieCollectionViewModel");
            EditMovieViewModel editMovieViewModel = new EditMovieViewModel(userCollectionViewModel, movieCollectionViewModel);
            DataContext = editMovieViewModel;
        }
        private void tbxYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void btnPicturePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Obrázky (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|Všechny soubory|*.*";
            if (dialog.ShowDialog() == true)
            {
                tbxPicturePath.Text = dialog.FileName;
                tbxPicturePath.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (EditMovieViewModel)DataContext;
            viewModel.EditModeClose.Execute(null);
        }
    }
}
