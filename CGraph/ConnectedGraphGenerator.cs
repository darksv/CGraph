namespace CGraph
{

    public class ConnectedGraphGenerator : IGraphGenerator
    {
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
            var checker = new DFSAlgorithm();
            
            for (int i = 0; i < 10000; ++i)
            {
                graph.Random(_probabilityOfEdgeExistence);
                checker.Execute(graph, 1, _numberOfVertices);
                if (checker.IsConnected())
                {
                    return graph;
                }
            }
            
            return null;
        }
    }
}