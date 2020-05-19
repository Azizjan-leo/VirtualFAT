
using System.Collections.Generic;

namespace VirtualFAT
{
    public static class Drive
    {
        public static int Capacity { get; set; } // In MB
        public static int AllocationUnitSize { get; set; } // In KB
        public static string VolumeLabel { get; set; } // Just name of the drive
        public static Cluster[] Clusters{ get; set; }
        public static void Format(int capacity, int allocUnitSize, string vLabel)
        {
            Capacity = capacity;
            AllocationUnitSize = allocUnitSize;
            VolumeLabel = vLabel;
            FakeOS.Volume.Name = vLabel;
            FakeOS.Volume.Tag = vLabel += ":\\";
            int countOfClusters = (Capacity * 1000000) / (AllocationUnitSize * 1000);
            Clusters = new Cluster[countOfClusters];

            AllocClusters();
        }

        private static void AllocClusters()
        {
            for (int i = 0; i < Clusters.Length; i++)
            {
                Clusters[i] = new Cluster(i);
            }
    
            Clusters[0].TreeItem = FakeOS.Volume;
            Write(null, Clusters[0].TreeItem, FakeOS.Volume.Name, true);
        }
       
        public static void RemoveInnerItem(TreeItem treeItem)
        {

            for (int i = 1; i < Clusters.Length; i++)
            {
                if (Clusters[i].TreeItem == treeItem) // So it is
                {
                    Clusters[i].TreeItem = null;
                    var next = Clusters[i].Data.Next;
                    Clusters[i].Data = null;

                    int flag = i + 1;
                    while (!string.IsNullOrEmpty(next))
                    {
                        for (int j = flag; ; j++)
                        {
                            if (Clusters[j]?.Data.Curr == next)
                            {
                                next = Clusters[j].Data.Next;
                                Clusters[j].Data = null;
                                flag = j + 1;
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }
        public static void Remove(TreeItem treeItem)
        {
            if (treeItem.FirstCluster.Data.IsDirr)
            {
                foreach (var item in treeItem.FirstCluster.Data.Dirs)
                {
                    for (int i = 1; i < Clusters.Length; i++)
                    {
                        if(Clusters[i]?.Data?.Curr == item)
                        {
                            if (Clusters[i].Data.IsDirr)
                                Remove(Clusters[i].TreeItem);
                            else
                                RemoveInnerItem(Clusters[i].TreeItem);
                        }
                    }
                }
                RemoveInnerItem(treeItem);
            }
        }

        public static void Write(TreeItem parant, TreeItem treeItem, string data, bool isDirr)
        {
            if (isDirr)
            {
                for (int i = 0; i < Clusters.Length; i++)
                {
                    if (Clusters[i].Data == null)
                    {
                        Clusters[i].Data = new Data(null, Clusters[i].HexAddress, data, isDirr);
                        Clusters[i].TreeItem = treeItem;
                        
                        parant?.FirstCluster.Data.Dirs.Add(Clusters[i].HexAddress);
                        
                        treeItem.FirstCluster = Clusters[i];
                        return;
                    }
                }
            }
            string[] words = data.Split(' ');
            Data temp = null;
            int w = 0;
            int point = 1;
            for (int i = 1; i < Clusters.Length; i++)
            {
                if (Clusters[i].Data == null) // So it is empty and avalable to store our first cluster
                {
                    Clusters[i].Data = new Data(temp, Clusters[i].HexAddress, words[w++], isDirr);
                    temp = Clusters[i].Data;
                    Clusters[i].TreeItem = treeItem;
                    parant?.FirstCluster.Data.Dirs.Add(Clusters[i].HexAddress);
                    treeItem.FirstCluster = Clusters[i];
                    point = ++i;
                    break;
                }
            }
            for (int i = point; i < Clusters.Length && w < words.Length; i++)
            {
                if(Clusters[i].Data == null)
                {
                    Clusters[i].Data = new Data(temp, Clusters[i].HexAddress, words[w++],isDirr);
                    temp.Next = Clusters[i].Data.Curr;
                    temp = Clusters[i].Data;
                }
            }
        }
    }

    public class Cluster
    {
        public string HexAddress { get; set; }
        public int IntAddress { get; set; }
        public Data Data { get; set; }
        public TreeItem TreeItem { get; set; }

        public Cluster(int intAddress)
        {
            IntAddress = intAddress;
            HexAddress = IntToBase.DoIt(32, intAddress);// IntAddress.ToString("X2");
            Data = null;
        }
    }

    public class Data
    {
        public bool IsDirr { get; set; }
        public string Prev { get; set; }
        public string Curr { get; set; }
        public string Next { get; set; }
        public string Content { get; set; }
        public List<string> Dirs { get; set; }

        public Data(Data prev, string curr, string content, bool isDirr)
        {
            Prev = prev?.Curr;
            Curr = curr;
            Content = content;
            IsDirr = isDirr;
            Dirs = new List<string>();
        }
    }
}
