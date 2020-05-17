
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
        }
    }

    public class Cluster
    {
        public string Base32Address { get; set; }
        public int IntAddress { get; set; }
        public Data Data { get; set; }
        public TreeItem TreeItem { get; set; }

        public Cluster(int intAddress)
        {
            IntAddress = intAddress;
            Base32Address = IntToBase.DoIt(32, IntAddress);
        }
    }

    public class Data
    {
        public bool IsDirr { get; set; }
        public string Prev { get; set; }
        public string Curr { get; set; }
        public string Next { get; set; }
        public string Content { get; set; }
    }
}
