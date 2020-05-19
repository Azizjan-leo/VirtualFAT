using System.Windows;
using System.Windows.Input;

namespace VirtualFAT
{
    /// <summary>
    /// Interaction logic for EnterFolderName.xaml
    /// </summary>
    public partial class EnterFolderName : Window
    {
        public EnterFolderName()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(this, ResponseTextBox);
            Keyboard.Focus(ResponseTextBox);
        }
        public string ResponseText
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ResponseTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            OkButton.IsEnabled = !string.IsNullOrEmpty(ResponseText);
        }

        private void ResponseTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (!string.IsNullOrEmpty(ResponseText))
                    DialogResult = true;

            }
        }
    }
}
