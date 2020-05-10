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
        public MainWindow()
        {
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
            // Get every item in the root
          
            // Create a Tree-View Item to represent it
            var treeViewItem = new TreeViewItem {
                // Add the title
                Header = FakeOS.Volume.Name,
                // Add the path
                Tag = FakeOS.Volume.Tag
            };

            // Create new test file inside the volume
            var treeItem = FakeOS.AddDirectory("Test", ItemType.File, 0);

            // Add a dummy child to the root Tree-View Item
            treeViewItem.Items.Add(null);

            // Listen out for item being expended
            treeViewItem.Expanded += TreeItem_Expanded;

                // Add an item to the main tree
                FolderView.Items.Add(treeViewItem);
        }

        /// <summary>
        /// When a disk/folder is expanded find sub folders/files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeItem_Expanded(object sender, RoutedEventArgs e)
        {
            var treeViewItem = (TreeViewItem)sender;

            // Exit if the item contains data
            if (treeViewItem.Items.Count != 1 || treeViewItem.Items[0] != null)
            {
                return;
            }

            // Remove the dummy child
            treeViewItem.Items.Clear();

            // Get the full path
            var fullPath = (string)treeViewItem.Tag;

            // Create a blank list for directories
            var directories = new List<string>();

            // Get nested directories
            foreach (var dir in FakeOS.GetDirectories(fullPath))
            {
                var newChildTreeViewItem = new TreeViewItem
                {
                    Header = dir.Name,
                    Tag = dir.Tag
                };
                newChildTreeViewItem.Expanded += TreeItem_Expanded;
                treeViewItem.Items.Add(newChildTreeViewItem);
            }
        }
        #endregion

    }
}
