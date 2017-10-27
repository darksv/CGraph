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
                var margin = vertex.Size / 2.0;

                vertex.Position = new Point
                {
                    X = _random.NextDouble(margin, area.Width - margin),
                    Y = _random.NextDouble(margin, area.Height - margin),
                };
            }
        }
    }
}