using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CGraph
{
    class CircleVertexSpreader : IVertexSpreader
    {
        public void Spread(IList<Vertex> vertices, Size area)
        {
            var n = vertices.Count;

            var center = new Point(area.Width / 2, area.Height / 2);
            var sortedVertices = vertices.Select(vertex => new
                {
                    Vertex = vertex,
                    Angle = CalculateAngle(vertex.Position - center)
                })
                .OrderBy(x => x.Angle)
                .Select(x => x.Vertex)
                .ToArray();

            for (int i = 0; i < n; ++i)
            {
                var radiusX = (area.Width - sortedVertices[i].Size) / 2;
                var radiusY = (area.Height - sortedVertices[i].Size) / 2;

                sortedVertices[i].Position = new Point
                {
                    X = center.X + radiusX * Math.Cos((double)(i + 1) / n * 2 * Math.PI),
                    Y = center.Y + radiusY * Math.Sin((double)(i + 1) / n * 2 * Math.PI)
                };
            }
        }

        private double CalculateAngle(Vector vec)
        {
            var angle = Math.Atan2(vec.Y, vec.X);
            return angle < 0 ? angle + 2.0 * Math.PI : angle;
        }
    }
}
