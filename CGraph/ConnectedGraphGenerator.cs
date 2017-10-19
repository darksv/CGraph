using System;

namespace CGraph
{

    public class ConnectedGraphGenerator : IGraphGenerator
    {
        private static readonly Random _random = new Random();
        private readonly int _numberOfVertices;
        private readonly double _probabilityOfEdgeExistence;

        public ConnectedGraphGenerator(int numberOfVertices, double probabilityOfEdgeExistence)
        {
            _numberOfVertices = numberOfVertices;
            _probabilityOfEdgeExistence = probabilityOfEdgeExistence;
        }

        public Graph Generate()
        {
            var graph = new Graph(_numberOfVertices);
            graph.Random(_probabilityOfEdgeExistence, _random.NextDouble);
            return graph;
        }
    }
}