using System.Windows;

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
    }
}
