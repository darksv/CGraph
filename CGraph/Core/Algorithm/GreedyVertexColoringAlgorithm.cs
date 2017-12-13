using System;
using System.Collections.Generic;
using System.Linq;

namespace CGraph.Core.Algorithm
{
    internal class GreedyVertexColoringAlgorithm : IVertexColoringAlgorithm
    {
        public IEnumerable<(int, int)> Execute(Graph graph)
        {
            var numberOfVertices = graph.NumberOfVertices;
            var vertexColor = Enumerable
                .Repeat(-1, numberOfVertices)
                .ToArray();
            vertexColor[0] = 0;
            for (int i = 1; i < numberOfVertices; ++i)
            {
                var colorIsAssigned = Enumerable
                    .Repeat(false, numberOfVertices)
                    .ToArray();
                // Mark colors of neighbours as used
                foreach (var j in graph.GetNeighboursOf(i))
                {
                    var color = vertexColor[j];
                    if (color > -1)
                    {
                        colorIsAssigned[color] = true;
                    }
                }
                // Assign first not assigned color to vertex i
                vertexColor[i] = Array.IndexOf(colorIsAssigned, false);
            }
            return Enumerable.Range(0, numberOfVertices).Select(i => (i, vertexColor[i]));
        }
    }
}
