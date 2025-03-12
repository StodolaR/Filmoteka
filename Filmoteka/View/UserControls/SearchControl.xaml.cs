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
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        private string[]? searchStrings;
        public SearchControl()
        {
            InitializeComponent();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            MovieViewModel movie = e.Item as MovieViewModel;
            if (movie != null && searchStrings != null)
            {
                bool isAccepted = false;
                foreach (string searchString in searchStrings)
                {
                    if (movie.Name.Contains(searchString))
                    {
                        isAccepted = true;
                    }
                }
                e.Accepted = isAccepted;
            }
            else 
            {
                e.Accepted = false; 
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text != "")
            {
                searchStrings = tbSearch.Text.Split(" ");
                lbSearch.SelectedItem = null;
                CollectionViewSource.GetDefaultView(lbSearch.ItemsSource).Refresh();
                lbSearch.Visibility = Visibility.Visible;
            }
        }
        private void lbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbSearch.Text = "";
            lbSearch.Visibility = Visibility.Collapsed;
        }
    }
}
