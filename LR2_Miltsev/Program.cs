using System;
using System.Collections.Generic;

namespace LR2_Miltsev
{

    class Program
    {
        static void Main(string[] args)
        {
            Data dt = new Data("prill_test.txt");
            dt.ShowData();
            NetworkGraph ng = new NetworkGraph();
            ng.CreateNetworkGraph(dt);
            ng.DisplaySortNG();
            Console.WriteLine("Пути в сетевом графе:");
            var l = new List<int>();
            ng.GetInitialNode().DisplayAllWays(l);
        }
    }
}
