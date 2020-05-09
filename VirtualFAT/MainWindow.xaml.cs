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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtualFAT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<TreeItem> Drives { get; set; }

        public MainWindow()
        {
            Drives = new List<TreeItem>();
            Drives.Add(new TreeItem { Id = 0, Name = "C:\\" });
            Drives.Add(new TreeItem { Id = 0, Name = "D:\\" });
            InitializeComponent();
        }

        #region On Loaded
        /// <summary>
        /// When the program first loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var drive in Drives)
            {
                var treeItem = new TreeViewItem {
                    Header = drive.Name
                };

                FolderView.Items.Add(treeItem);
            }
        }
        #endregion

    }
}
