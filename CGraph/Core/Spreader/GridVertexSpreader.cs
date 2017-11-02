using System;
using System.Collections.Generic;
using System.Windows;

namespace CGraph.Core.Spreader
{
    class GridVertexSpreader : IVertexSpreader
    {
        public void Spread(IList<Vertex> vertices, Size area)
        {
            var columns = (int) Math.Ceiling(Math.Sqrt(vertices.Count));
            var rows = columns;

            // Reduce the number of rows when after that
            // we are still able to fit all the vertices
            if (columns * (rows - 1) >= vertices.Count)
            {
                rows--;
            }
            
            var columnWidth = area.Width / columns;
            var rowHeight = area.Height / rows;

            for (int index = 0; index < vertices.Count; ++index)
            {
                var i = index % columns;
                var j = index / columns;

                // Add half of the distance between rows/cols 
                // to center vertices on the grid
                vertices[index].Position = new Point
                {
                    X = (i + 0.5) * columnWidth,
                    Y = (j + 0.5) * rowHeight,
                };
            }
        }
    }
}