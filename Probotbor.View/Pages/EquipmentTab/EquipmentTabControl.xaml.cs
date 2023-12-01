﻿using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace Probotbor.View.Pages.EquipmentTab
{
    /// <summary>
    /// Interaction logic for EquipmentTabControl.xaml
    /// </summary>
    public partial class EquipmentTabControl : UserControl
    {
        public EquipmentTabControl()
        {
            InitializeComponent();
        }

        private void ScrollToUp(object sender, RoutedEventArgs e)
        {
            ScrollBar.ScrollToTop();
        }

        private void ScrollToDown(object sender, RoutedEventArgs e)
        {
            ScrollBar.ScrollToBottom();
        }


        public RelayCommand OpenProbotborWindow
        {

            get => new RelayCommand(() =>
            {
                var pw = new ProbotbornikWindow();                
                pw.DataContext = this.DataContext;
                pw.ShowDialog();
            });
        }

    }
}