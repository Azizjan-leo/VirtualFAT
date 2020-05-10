using System;
using System.Collections.Generic;

namespace VirtualFAT
{
    public class TreeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } // i.e. Folder123 or File321
        public ItemType Type { get; set; }  // Folder or File
        public string Tag { get; set; } // Path to this item. I.e. F:\Collage\

        public List<TreeItem> Childs { get; set; } // Inner Folders or Files

        public TreeItem(int id, ItemType itemType, string name, string tag)
        {
            Id = id;
            Type = itemType;
            Name = name;
            Tag = tag;
            Childs = new List<TreeItem>();
        }

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
    }
    public enum ItemType { Disk, Folder, File }

}