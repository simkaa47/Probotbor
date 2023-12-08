﻿using System;
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
using System.Windows.Shapes;

namespace Probotbor.View.Pages.EquipmentTab
{
    /// <summary>
    /// Interaction logic for ProbotbornikWindow.xaml
    /// </summary>
    public partial class ProbotbornikWindow : Window
    {
        public ProbotbornikWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = true;
                this.Close(); 
            }
        }
    }
}
