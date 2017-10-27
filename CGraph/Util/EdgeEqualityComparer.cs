using System.Collections.Generic;
using CGraph.Core;

namespace CGraph.Util
{
    public class EdgeEqualityComparer : IEqualityComparer<Edge>
    {
        public bool Equals(Edge firstName, Edge secondEdge)
        {
            return firstName.Equals(secondEdge);
        }

        public int GetHashCode(Edge edge)
        {
            return edge.GetHashCode();
        }
    }
}