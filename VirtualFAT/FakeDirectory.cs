using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualFAT
{
    /*
     * Emulates directories
     */
    public static class FakeOS
    {
        static int Ids = 0;
        static int DocIds = 0;
        //
        public static TreeItem Volume { get; set; } = new TreeItem(Ids++,ItemType.drive,"Артемий", "Артемий:\\");
        public static List<Document> Docs { get; set; } = new List<Document>();

        public static TreeItem AddDirectory(string name, ItemType itemType, int parantId)
        {
            // Find parant
            TreeItem parant = Volume.GetTreeItem(parantId);
            // Create new TreeItem
            var child = new TreeItem(Ids++, itemType, name, parant.Tag + name);
            // Create document if it is a file
            if (itemType == ItemType.file)
            {
                var doc = new Document(DocIds++, child);
                child.Document = doc;
                Docs.Add(doc);
            }
            // Add new diractory to the parant
            parant.Childs.Add(child);
            // Return created child
            return child;
        }

        /// <summary>
        /// Returns nested dirs for the given directory
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static IEnumerable<TreeItem> GetDirectories(string tag)
        {
            // Find parant
            TreeItem parant = Volume.GetTreeItem(tag);
            // Crate a list for children names
            List<TreeItem> dirs = new List<TreeItem>();

            // Look over each path
            foreach (var treeItem in parant.Childs)
            {
                dirs.Add(treeItem);
            }

            // Return the list of dirs that was found inside the parant item
            return dirs;
        }

    }
}
