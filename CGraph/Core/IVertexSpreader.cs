using System.Collections.Generic;
using System.Windows;

namespace CGraph.Core
{
    interface IVertexSpreader
    {
        void Spread(IList<Vertex> vertices, Size area);
    }
}