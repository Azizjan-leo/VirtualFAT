using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                Name = "Id" + FakeOS.Volume.Id.ToString()

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
                    Name = "Id" + dir.Id.ToString(),
                    Tag = dir.Tag
                };
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem = new MenuItem()
                {
                    Header = "Delete",
                    Tag = dir.Tag
                };
                if (dir.Type == ItemType.folder || dir.Type == ItemType.drive)
                {
                    newChildTreeViewItem.Items.Add(null);

                    contextMenu.Items.Add("Create new item");
                    newChildTreeViewItem.Expanded += TreeItem_Expanded;
                }

                menuItem.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_Click));
                contextMenu.Items.Add(menuItem);
                newChildTreeViewItem.ContextMenu = contextMenu;
                treeViewItem.Items.Add(newChildTreeViewItem);
            }

            if(treeViewItem.Name == ItemType.folder.ToString())
                treeViewItem.Name = "folderOpen";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            TreeItem itemToDelete = FakeOS.Volume.GetTreeItem(menuItem.Tag.ToString());
            TreeItem parant = FakeOS.Volume.GetParantOf(itemToDelete.Id);
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {itemToDelete.Name}?", "Artemiy OS", MessageBoxButton.OKCancel);
            switch (result)
            {
                case MessageBoxResult.OK:
                    FakeOS.Volume.RemoveTreeItem(itemToDelete.Id);

                    TreeViewItem parantTVI = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(FolderView, "Id" + parant.Id); //FolderView.Items.GetItemAt(0);
                    TreeViewItem childTVI = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(FolderView, "Id" + itemToDelete.Id); //FolderView.Items.GetItemAt(0);
                    parantTVI.Items.Remove(childTVI);

                    FolderView.UpdateLayout();
                    MessageBox.Show("Item was deleted", "Artemiy OS");
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }

        }
        #endregion

    }
}
