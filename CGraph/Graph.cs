using System;

namespace CGraph
{
    public class Graph
    {
        private readonly int _numberOfVertices;
        private readonly bool[,] _adjacencyMatrix;

        public Graph(int numberOfVertices)
        {
            _numberOfVertices = numberOfVertices;
            _adjacencyMatrix = new bool[numberOfVertices, numberOfVertices];
        }

        public bool this[int i, int j] => _adjacencyMatrix[i, j];

        public void Random(double edgeExistenceProbability)
        {
            var random = new Random();

            for (int i = 0; i < _numberOfVertices; ++i)
            {
                for (int j = i + 1; j < _numberOfVertices; ++j)
                {
                    var randomValue = random.NextDouble();
                    if (randomValue < edgeExistenceProbability)
                    {
                        _adjacencyMatrix[i, j] = true;
                        _adjacencyMatrix[j, i] = true;
                    }
                }
            }
        }
    }
}
