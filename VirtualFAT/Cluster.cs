using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualFAT
{
    public class Document
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModification { get; set; }
        // Let's link the document to its tree-item
        public TreeItem Parant { get; set; }

        public Document(int id, TreeItem parant)
        {
            Id = id;
            Parant = parant;
            DateCreated = LastModification = DateTime.Now;
        }
    }
    public class TreeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } // i.e. Folder123 or File321
        public ItemType Type { get; set; }  // Folder or File
        public string Tag { get; set; } // Path to this item. I.e. F:\Collage\
        public Cluster FirstCluster { get; set; }
        public List<TreeItem> Childs { get; set; } // Inner Folders or Files
        public Document Document{ get; set; } // To store documnt info like text and attributes
        public TreeItem(int id, ItemType itemType, string name, string tag)
        {
            Id = id;
            Type = itemType;
            Name = name;
            Tag = tag;
            Childs = new List<TreeItem>();
        }

        /// <summary>
        /// Finds a TreeItem by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TreeItem GetTreeItem(int id)
        {
            var stack = new Stack<TreeItem>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (current.Id == id)
                {
                    return current;
                }

                foreach (var child in current.Childs)
                    stack.Push(child);
            }
            return null;
        }

        /// <summary>
        /// Removes a TreeItem 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void RemoveTreeItem(int id)
        {
            foreach (var item in this.Childs)
            {
                if (item.Id == id)
                {
                    this.Childs.Remove(item);
                    return;
                }
                item.RemoveTreeItem(id);
            }
        }

        /// <summary>
        /// Finds a TreeItem by its tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>TreeItem</returns>
        public TreeItem GetTreeItem(string tag)
        {
            var stack = new Stack<TreeItem>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (current.Tag == tag)
                {
                    return current;
                }

                foreach (var child in current.Childs)
                    stack.Push(child);
            }
            return null;
        }


        internal TreeItem GetParantOf(int id)
        {
            var stack = new Stack<TreeItem>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if(current.Childs.Select(x=>x.Id).Distinct().Contains(id))
                {
                    return current;
                }

                foreach (var child in current.Childs)
                    stack.Push(child);
            }
            return null;
        }
    }
    public enum ItemType { drive, folder, file }

}