using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

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
      
            // Add a dummy child to the root Tree-View Item
            treeViewItem.Items.Add(null);

            var contextMenu = new ContextMenu();
            MenuItem menuItemCreate = new MenuItem()
            {
                Header = "Create new folder",
                Tag = FakeOS.Volume.Tag
            };
            menuItemCreate.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickCreateFolder));
            contextMenu.Items.Add(menuItemCreate);
            MenuItem menuItemCreateFile = new MenuItem()
            {
                Header = "Create new file",
                Tag = FakeOS.Volume.Tag
            };
            menuItemCreateFile.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickCreateFile));
            contextMenu.Items.Add(menuItemCreateFile);
            MenuItem menuItemRename = new MenuItem()
            {
                Header = "Rename",
                Tag = FakeOS.Volume.Tag
            };
            menuItemRename.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickRename));
            contextMenu.Items.Add(menuItemRename);
            treeViewItem.ContextMenu = contextMenu;

            // Listen out for item being expended
            treeViewItem.Expanded += TreeItem_Expanded;

            // Add an item to the main tree
            FolderView.Items.Add(treeViewItem);
        }

        
        #endregion
        /// <summary>
        /// When a disk/folder is expanded find sub folders/files
        /// </summary>
        /// <param name="sender">TreeViewItem</param>
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
                MenuItem menuItemDelete = new MenuItem()
                {
                    Header = "Delete",
                    Tag = dir.Tag
                };
                MenuItem menuItemRename = new MenuItem()
                {
                    Header = "Rename",
                    Tag = FakeOS.Volume.Tag
                };
                menuItemRename.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickRename));
                contextMenu.Items.Add(menuItemRename);
                if (dir.Type == ItemType.folder || dir.Type == ItemType.drive)
                {
                    newChildTreeViewItem.Items.Add(null);
                    MenuItem menuItemCreateFolder = new MenuItem()
                    {
                        Header = "Create new folder",
                        Tag = dir.Tag
                    };
                    menuItemCreateFolder.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickCreateFolder));
                    contextMenu.Items.Add(menuItemCreateFolder);
                    MenuItem menuItemCreateFile = new MenuItem()
                    {
                        Header = "Create new file",
                        Tag = dir.Tag
                    };
                    menuItemCreateFile.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickCreateFile));
                    contextMenu.Items.Add(menuItemCreateFile);
                    newChildTreeViewItem.ContextMenu = contextMenu;
                    newChildTreeViewItem.Expanded += TreeItem_Expanded;
                }
                else // If it is a file let's add an Open command 
                {
                    MenuItem menuItemOpenFile = new MenuItem()
                    {
                        Header = "Open",
                        Tag = dir.Tag
                    };
                    menuItemOpenFile.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickOpenFile));
                    contextMenu.Items.Add(menuItemOpenFile);
                }
                // Bind the handler of removing items
                menuItemDelete.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickDelete));
                contextMenu.Items.Add(menuItemDelete);
                newChildTreeViewItem.ContextMenu = contextMenu;
                treeViewItem.Items.Add(newChildTreeViewItem);
            }

            if(treeViewItem.Name == ItemType.folder.ToString())
                treeViewItem.Name = "folderOpen";
        }
        #region ContextHandlers
        private void MenuItem_ClickRename(object sender, RoutedEventArgs e)
        {
            var dialog = new EnterFolderName();
            dialog.Message.Text = "Enter new name";
            if (dialog.ShowDialog() == true)
            {
                var menuItem = sender as MenuItem;

                // Find the treeItem from OS
                TreeItem treeItem = FakeOS.Volume.GetTreeItem(menuItem.Tag.ToString());
                treeItem.Name = dialog.ResponseText;

                //// Find treeViewItem of item user want to rename in the Tree
                TreeViewItem itemTVI = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(FolderView, "Id" + treeItem.Id); //FolderView.Items.GetItemAt(0);
                if (itemTVI.Tag.ToString().Count(f => f == '\\') == 1)// So it is the volume
                {
                    treeItem.Tag = treeItem.Name + ":\\";
                    itemTVI.Tag = treeItem.Tag;
                }
                else
                {
                    treeItem.Tag = treeItem.Tag.Substring(0, treeItem.Tag.LastIndexOf('\\') + 1);
                    treeItem.Tag += treeItem.Name;


                }
                itemTVI.Header = treeItem.Name;
                foreach (MenuItem item in itemTVI.ContextMenu.Items)
                {
                    item.Tag = itemTVI.Tag;
                }
                //itemTVI.Tag = treeItem.Tag;
                FolderView.UpdateLayout();
            }
        }
        private void MenuItem_ClickOpenFile(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            TreeItem treeItem = FakeOS.Volume.GetTreeItem(menuItem.Tag.ToString());
            var notBad = new NotBad();
            notBad.Title += " " + treeItem.Name;
            notBad.IsNewDoc = false;
            notBad.TextContent.Text = treeItem.Document.Content;
            notBad.Changes = false;
            if (notBad.ShowDialog() == true)
            {
                treeItem.Document.Content = notBad.TextContent.Text;
                treeItem.Document.LastModification = DateTime.UtcNow;
            }
        }
        private void MenuItem_ClickCreateFile(object sender, RoutedEventArgs e)
        {
            string docName;

            var dialog = new EnterFolderName();
            dialog.Message.Text = "Enter document name";
            if (dialog.ShowDialog() == true)
            {
                docName = dialog.ResponseText;
                var menuItem = sender as MenuItem;

                // Find the parant from OS
                TreeItem parant = FakeOS.Volume.GetTreeItem(menuItem.Tag.ToString());
               
                // Create our new child and put it among the parants children
                TreeItem child = FakeOS.AddDirectory(docName, ItemType.file, parant.Id);

                // Find parant of item user want to delete in the Tree
                TreeViewItem parantTVI = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(FolderView, "Id" + parant.Id); //FolderView.Items.GetItemAt(0);
                                                                                                                        // Exit if the item contains data
                if (parantTVI.Items.Count == 1 && parantTVI.Items[0] == null)
                {
                    // Remove the dummy child
                    parantTVI.Items.Clear();
                }

                // Create new Tree-ViewItem for our new child
                TreeViewItem childTVI = new TreeViewItem()
                {
                    // Add the title
                    Header = child.Name,
                    // Add the path
                    Tag = child.Tag,
                    // Name for image
                    Name = "Id" + child.Id.ToString()
                };

                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItemOpen = new MenuItem()
                {
                    Header = "Open",
                    Tag = child.Tag
                };
                MenuItem menuItemRename = new MenuItem()
                {
                    Header = "Rename",
                    Tag = child.Tag
                };
                menuItemRename.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickRename));
                contextMenu.Items.Add(menuItemRename);
                menuItemOpen.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickOpenFile));
                contextMenu.Items.Add(menuItemOpen);
                MenuItem menuItemDelete = new MenuItem()
                {
                    Header = "Delete",
                    Tag = child.Tag
                };
                menuItemDelete.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickDelete));
                contextMenu.Items.Add(menuItemDelete);

                childTVI.ContextMenu = contextMenu;

                // Add new child Tree-ViewItem to the parant
                parantTVI.Items.Add(childTVI);
                FolderView.UpdateLayout();

                var notBad = new NotBad();
                notBad.Title += " " + docName; 
                if (notBad.ShowDialog() == true)
                {
                    child.Document.Content = notBad.TextContent.Text;
                    child.Document.LastModification = DateTime.UtcNow;
                }
            }
        }
        private void MenuItem_ClickCreateFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new EnterFolderName();
            if (dialog.ShowDialog() == true)
            {
                var menuItem = sender as MenuItem;

                // Find the parant from OS
                TreeItem parant = FakeOS.Volume.GetTreeItem(menuItem.Tag.ToString());
                // Create our new child and put it among the parants children
                TreeItem child = FakeOS.AddDirectory(dialog.ResponseText, ItemType.folder, parant.Id);

                // Find parant of item user want to delete in the Tree
                TreeViewItem parantTVI = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(FolderView, "Id" + parant.Id); //FolderView.Items.GetItemAt(0);

                if (parantTVI.Items.Count == 1 && parantTVI.Items[0] == null)
                {
                    // Remove the dummy child
                    parantTVI.Items.Clear();
                }

                // Create new Tree-ViewItem for our new child
                TreeViewItem childTVI = new TreeViewItem()
                {
                    // Add the title
                    Header = child.Name,
                    // Add the path
                    Tag = child.Tag,
                    // Name for image
                    Name = "Id" + child.Id.ToString()
                };

                // Add a dummy child to the root Tree-View Item for ability to expand our new folder
                childTVI.Items.Add(null);

                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItemCreate = new MenuItem()
                {
                    Header = "Create new folder",
                    Tag = child.Tag
                };
                menuItemCreate.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickCreateFolder));
                contextMenu.Items.Add(menuItemCreate);
                MenuItem menuItemRename = new MenuItem()
                {
                    Header = "Rename",
                    Tag = child.Tag
                };
                menuItemRename.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickRename));
                contextMenu.Items.Add(menuItemRename);
                MenuItem menuItemDelete = new MenuItem()
                {
                    Header = "Delete",
                    Tag = child.Tag
                };
                menuItemDelete.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickDelete));
                contextMenu.Items.Add(menuItemDelete);
                MenuItem menuItemCreateFile = new MenuItem()
                {
                    Header = "Create file",
                    Tag = child.Tag
                };
                menuItemCreateFile.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MenuItem_ClickCreateFile));
                contextMenu.Items.Add(menuItemCreateFile);

                childTVI.ContextMenu = contextMenu;
                // Listen out for item being expended
                childTVI.Expanded += TreeItem_Expanded;

                // Add new child Tree-ViewItem to the parant
                parantTVI.Items.Add(childTVI);
                FolderView.UpdateLayout();
            }
        }
        private void MenuItem_ClickDelete(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;

            // Find the item from OS
            TreeItem itemToDelete = FakeOS.Volume.GetTreeItem(menuItem.Tag.ToString());
            // Find its parant from OS
            TreeItem parant = FakeOS.Volume.GetParantOf(itemToDelete.Id);
            // Let's be sure that user really wants to delete the item
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {itemToDelete.Name}?", "Artemiy OS", MessageBoxButton.OKCancel);
            switch (result)
            {
                case MessageBoxResult.OK:
                    // Remove item from OS
                    FakeOS.Volume.RemoveTreeItem(itemToDelete.Id);
                    // Find parant of item user want to delete in the Tree
                    TreeViewItem parantTVI = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(FolderView, "Id" + parant.Id); //FolderView.Items.GetItemAt(0);
                    // Find item from the Tree by itself
                    TreeViewItem childTVI = (TreeViewItem)LogicalTreeHelper.FindLogicalNode(FolderView, "Id" + itemToDelete.Id); //FolderView.Items.GetItemAt(0);
                    // Remove child from children of parant 
                    parantTVI.Items.Remove(childTVI);
                    // Update the Tree so deleted item will be finaly removed from the UI
                    FolderView.UpdateLayout();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }

        }
        #endregion
    }
}
