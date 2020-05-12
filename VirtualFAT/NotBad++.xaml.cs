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
using System.Windows.Shapes;

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
        bool Changes = false;
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
                        break;
                    case
                        MessageBoxResult.No:
                        Close();
                        break;
                    case
                        MessageBoxResult.Cancel:
                        break;
                }
            }
            else
                Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Changes = true;
        }
    }
}
