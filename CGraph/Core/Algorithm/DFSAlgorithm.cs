using System.Collections.Generic;

namespace CGraph.Core.Algorithm
{
    class DfsAlgorithm : ISearchAlgorithm
    {
        public IEnumerable<int> Execute(Graph graph, int startVertex)
        {
            var visited = new HashSet<int>();
            var remaining = new Stack<int>();
            remaining.Push(startVertex);

            while (remaining.Count > 0)
            {
                var currentVertex = remaining.Pop();
                if (visited.Contains(currentVertex))
                {
                    continue;
                }

                visited.Add(currentVertex);
                yield return currentVertex;

                for (int j = 0; j < graph.NumberOfVertices; j++)
                {
                    if (graph[currentVertex, j])
                    {
                        remaining.Push(j);
                    }
                }
            }
        }
    }
}