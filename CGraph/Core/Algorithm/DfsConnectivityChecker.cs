using System.Linq;

namespace CGraph.Core.Algorithm
{
    class DfsConnectivityChecker : IConnectivityChecker
    {
        public bool IsConnected(Graph graph)
        {
            var visitedVertices = new DfsAlgorithm().Execute(graph, 0).Count();
            return visitedVertices == graph.NumberOfVertices;
        }
    }
}