using System.Collections.Generic;

namespace CGraph
{
    class DFSAlgorithm
    {
        private readonly Stack<int> _stack = new Stack<int>();
        private readonly HashSet<int> _visited = new HashSet<int>();
        private int _currentVertex, _verticesCount;

        public void Execute(Graph graph, int srcVertex)
        {
            _stack.Push(srcVertex);
            _currentVertex = srcVertex - 1;
            _verticesCount = graph.NumberOfVertices;
            while (_stack.Count != 0)
            {
                _currentVertex = _stack.Pop();
                if (_visited.Contains(_currentVertex))
                    continue;
                _visited.Add(_currentVertex);

                for (int j = 0; j < graph.NumberOfVertices; j++)
                {
                    if (graph[_currentVertex, j])
                    {
                        _stack.Push(j);
                    }
                }
            }
        }
        public bool IsConnected()
        {
            return (_visited.Count == _verticesCount) ? true : false;
        }
    }
}
