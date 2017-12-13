using System.Collections.Generic;

namespace CGraph.Core
{
    internal interface IVertexColoringAlgorithm
    {
        IEnumerable<(int, int)> Execute(Graph graph);
    }
}
