using System.Collections.Generic;

namespace VirtualFAT
{
    public class TreeItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }  // Disk or Folder or File

        public List<TreeItem>  Childs { get; set; } // Inner Folders or Files
    }
}