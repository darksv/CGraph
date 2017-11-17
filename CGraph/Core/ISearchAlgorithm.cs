using System.Collections.Generic;

namespace CGraph.Core.Algorithm
{
    interface ISearchAlgorithm
    {
        IEnumerable<int> Execute(Graph graph, int startVertex);
    }
}