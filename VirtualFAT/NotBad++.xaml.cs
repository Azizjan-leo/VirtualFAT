using System.Windows;
using System.Windows.Controls;

namespace VirtualFAT
{
    /// <summary>
    /// Interaction logic for NotBad__.xaml
    /// </summary>
    public partial class NotBad : Window
    {
        public NotBad()
        {
            InitializeComponent();
        }
        public bool Changes = false;
        public bool IsNewDoc;
        public string DocName { get; set; }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (IsNewDoc)
            {
                var dialog = new EnterFolderName();
                dialog.Title = "Enter document name";
                if (dialog.ShowDialog() == true)
                {
                    DocName = dialog.ResponseText;
                }
            }
            DialogResult = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (Changes == true)
            {
                MessageBoxResult result = MessageBox.Show($"Do you want to save changes?", "Artemiy OS", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case
                        MessageBoxResult.Yes:
                        if (IsNewDoc)
                        {
                            var dialog = new EnterFolderName();
                            dialog.Message.Text = "Enter document name";
                            if (dialog.ShowDialog() == true)
                            {
                                DocName = dialog.ResponseText;
                            }
                        }
                        DialogResult = true;
                        Changes = false;
                        break;
                    case
                        MessageBoxResult.No:
                        Changes = false;
                        Close();
                        break;
                    case
                        MessageBoxResult.Cancel:
                        break;
                }
            }
            else
            {
                Changes = false;
                Close();
            }
              
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Changes = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Changes == true)
            {
                MessageBoxResult result = MessageBox.Show($"Do you want to save changes?", "Artemiy OS", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case
                        MessageBoxResult.Yes:
                        if (IsNewDoc)
                        {
                            var dialog = new EnterFolderName();
                            dialog.Message.Text = "Enter document name";
                            if (dialog.ShowDialog() == true)
                            {
                                DocName = dialog.ResponseText;
                            }
                        }
                        DialogResult = true;
                        e.Cancel = false;
                        break;
                    case
                        MessageBoxResult.No:
                        e.Cancel = false;
                        break;
                    case
                        MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }
    }
}
