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

namespace Filmoteka.View.UserControls
{
    /// <summary>
    /// Interaction logic for AddRating.xaml
    /// </summary>
    public partial class AddRating : UserControl
    {
        public AddRating()
        {
            InitializeComponent();
            UserCollectionViewModel userCollectionViewModel = (UserCollectionViewModel)Application.Current.FindResource("userCollectionViewModel");
            MovieCollectionViewModel movieCollectionViewModel = (MovieCollectionViewModel)Application.Current.FindResource("movieCollectionViewModel");
            AddRatingViewModel addRatingViewModel = new AddRatingViewModel(userCollectionViewModel, movieCollectionViewModel);
            DataContext = addRatingViewModel;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rbRating.RatingValue = 0;
            tbxReview.Text = null;
        }
    }
}
