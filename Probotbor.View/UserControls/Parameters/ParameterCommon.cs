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

    }
}
