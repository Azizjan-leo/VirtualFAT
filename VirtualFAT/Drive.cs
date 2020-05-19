
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
            Write(Clusters[0].TreeItem, FakeOS.Volume.Name, true);
        }

        public static void Write(TreeItem treeItem, string data, bool isDirr)
        {
            if (isDirr)
            {
                for (int i = 0; i < Clusters.Length; i++)
                {
                    if (Clusters[i].Data == null)
                    {
                        Clusters[i].Data = new Data(null, Clusters[i].HexAddress, data);
                        Clusters[i].TreeItem = treeItem;
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
                if (Clusters[i].Data == null)
                {
                    Clusters[i].Data = new Data(temp, Clusters[i].HexAddress, words[w++]);
                    temp = Clusters[i].Data;
                    Clusters[i].TreeItem = treeItem;
                    treeItem.FirstCluster = Clusters[i];
                    point = i;
                    break;
                }
            }
            for (int i = point; i < Clusters.Length && w < words.Length; i++)
            {
                if(Clusters[i].Data == null)
                {
                    Clusters[i].Data = new Data(temp, Clusters[i].HexAddress, words[w++]);
                    temp.Next = Clusters[i].Data.Curr;
                    temp = Clusters[i].Data;
                }
            }

            //    for (int i = 0; i < Clusters.Length; i++)
            //    {
            //        var tmp = Clusters[i];
            //    }
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
            HexAddress = IntAddress.ToString("X2");
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

        public Data(Data prev, string curr, string content)
        {
            Prev = prev?.Curr;
            Curr = curr;
            Content = content;
            IsDirr = false;
        }
    }
}
