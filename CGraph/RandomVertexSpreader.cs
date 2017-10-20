using System;
using System.Collections.Generic;
using System.Windows;

namespace CGraph
{
    class RandomVertexSpreader : IVertexSpreader
    {
        private static readonly Random _random = new Random();

        public void Spread(IList<Vertex> vertices, Size area)
        {
            foreach (var vertex in vertices)
            {
                var vertexRadius = (int)(vertex.Size / 2);
                vertex.Position = new Point
                {
                    X = _random.Next(vertexRadius, (int)area.Width - vertexRadius),
                    Y = _random.Next(vertexRadius, (int)area.Height - vertexRadius),
                };
            }
        }
    }
}