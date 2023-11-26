using Probotbor.View.UserControls.Parameters;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Probotbor.View.UserControls.Buttons
{
    /// <summary>
    /// Interaction logic for VissmaButton.xaml
    /// </summary>
    [ContentProperty("InnerContent")]
    public partial class VissmaButton : ParameterCommon
    {
        public VissmaButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty InnerContentProperty =
        DependencyProperty.Register("InnerContent", typeof(object), typeof(VissmaButton));

        public object InnerContent
        {
            get { return (object)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }



        public Brush MousePressedSolidColorBrush
        {
            get { return (Brush)GetValue(MousePressedSolidColorBrushProperty); }
            set { SetValue(MousePressedSolidColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MousePressedSolidColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MousePressedSolidColorBrushProperty =
            DependencyProperty.Register("MousePressedSolidColorBrush", typeof(Brush), typeof(VissmaButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));




        public Brush MouseOverSolidColorBrush
        {
            get { return (Brush)GetValue(MouseOverSolidColorBrushProperty); }
            set { SetValue(MouseOverSolidColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverSolidColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverSolidColorBrushProperty =
            DependencyProperty.Register("MouseOverSolidColorBrush", typeof(Brush), typeof(VissmaButton), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));


    }
}
