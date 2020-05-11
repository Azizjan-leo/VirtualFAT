using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            // Create a Tree-View Item to represent Volume
            var treeViewItem = new TreeViewItem {
                // Add the title
                Header = FakeOS.Volume.Name,
                // Add the path
                Tag = FakeOS.Volume.Tag,
                // Name for image
                Name = FakeOS.Volume.Type.ToString()

            };

            // Create new test file inside the volume
            FakeOS.AddDirectory("Test", ItemType.file, 0);
            var mathematics = FakeOS.AddDirectory("Mathematics", ItemType.folder, 0);
            FakeOS.AddDirectory("Music", ItemType.folder, mathematics.Id);
            // Add a dummy child to the root Tree-View Item
            treeViewItem.Items.Add(null);
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.Items.Add("Create new item");
           
            treeViewItem.ContextMenu = contextMenu;
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
                    // Let's puth the type here to conveert it into image later :)
                    Name = dir.Type.ToString(),
                    Tag = dir.Tag
                };
                ContextMenu contextMenu = new ContextMenu();
                
                contextMenu.Items.Add("Delete");

                if (dir.Type == ItemType.folder || dir.Type == ItemType.drive)
                {
                    newChildTreeViewItem.Items.Add(null);
                    contextMenu.Items.Add("Create new item");
                    newChildTreeViewItem.Expanded += TreeItem_Expanded;
                }
                newChildTreeViewItem.ContextMenu = contextMenu;
                treeViewItem.Items.Add(newChildTreeViewItem);
            }

            if(treeViewItem.Name == ItemType.folder.ToString())
                treeViewItem.Name = "folderOpen";
        }
        #endregion

    }
}
