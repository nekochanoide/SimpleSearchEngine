using System.Windows;
using System.Windows.Input;

namespace SimpleSearchEngine.Password
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
            pswd.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void pswd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Close();
        }
    }
}
