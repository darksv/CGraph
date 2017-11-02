using System;
using PropertyChanged;

namespace CGraph.Core
{
    [ImplementPropertyChanged]
    public class Edge : Selectable
    {
        public Vertex A { get; }
        public Vertex B { get; }
        public int ZIndex => 0;

        public Edge(Vertex v1, Vertex v2)
        {
            A = v1;
            B = v2;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Edge);
        }

        public bool Equals(Edge other)
        {
            return other != null &&
                   (Equals(A, other.A) && Equals(B, other.B) || Equals(A, other.B) && Equals(B, other.A));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var a = A.GetHashCode();
                var b = B.GetHashCode();

                var hashCode = 0;
                hashCode = (hashCode * 397) ^ Math.Min(a, b);
                hashCode = (hashCode * 397) ^ Math.Max(a, b);
                return hashCode;
            }
        }
    }
}