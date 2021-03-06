﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CGraph.Core
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

        public int NumberOfVertices => _numberOfVertices;

        public void Random(double edgeExistenceProbability, Func<double> randomProvider)
        {
            for (int i = 0; i < _numberOfVertices; ++i)
            {
                for (int j = i + 1; j < _numberOfVertices; ++j)
                {
                    var randomValue = randomProvider();
                    if (randomValue < edgeExistenceProbability)
                    {
                        _adjacencyMatrix[i, j] = true;
                        _adjacencyMatrix[j, i] = true;
                    }
                }
            }
        }

        public IEnumerable<int> GetNeighboursOf(int vertex)
        {
            return Enumerable.Range(0, NumberOfVertices).Where(i => this[vertex, i]);
        }
    }
}