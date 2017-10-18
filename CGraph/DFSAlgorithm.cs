using System.Collections.Generic;

namespace CGraph
{
    class DFSAlgorithm
    {
        private Stack<int> _stack = new Stack<int>();
        private List<int> _visited = new List<int>();
        private int _currentVertex, _verticesCount;

        public void Execute(Graph g, int srcVertex, int verticesCount)
        {
            _stack.Push(srcVertex);
            _currentVertex = srcVertex;
            _verticesCount = verticesCount;
            while (_stack.Count != 0)
            {
                _currentVertex = _stack.Pop();
                if (_visited.Contains(_currentVertex))
                    continue;
                _visited.Add(_currentVertex);

                for (int j = 0; j < verticesCount; j++)
                {
                    if (g[_currentVertex, j])
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
