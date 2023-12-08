using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probotbor.View.UserControls.Parameters
{
    public class ParameterCommon:UserControl
    {
        #region Command
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ParameterCommon), new PropertyMetadata(null));
        #endregion

        #region ParameterCommand
        public object ParameterCommand
        {
            get { return (object)GetValue(ParameterCommandProperty); }
            set { SetValue(ParameterCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParameterCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParameterCommandProperty =
            DependencyProperty.Register("ParameterCommand", typeof(object), typeof(ParameterCommon), new PropertyMetadata(null));
        #endregion

        #region ParamWidth
        public int ParamWidth
        {
            get { return (int)GetValue(ParamWidthProperty); }
            set { SetValue(ParamWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParamWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParamWidthProperty =
            DependencyProperty.Register("ParamWidth", typeof(int), typeof(ParameterCommon), new PropertyMetadata(100));
        #endregion

        #region DescriptionInvisible
        public bool DescriptionInvisible
        {
            get { return (bool)GetValue(DescriptionInvisibleProperty); }
            set { SetValue(DescriptionInvisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DescriptionInvisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionInvisibleProperty =
            DependencyProperty.Register("DescriptionInvisible", typeof(bool), typeof(ParameterCommon), new PropertyMetadata(false)); 
        #endregion

    }
}
