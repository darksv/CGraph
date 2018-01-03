using System;
using System.IO;
using System.Linq;
using CGraph.Core;
using CGraph.Core.Algorithm;

namespace CGraph.Report
{
    internal class TextReportCreator : IReportCreator
    {
        private readonly Graph _graph;
        private readonly ISearchAlgorithm _searchAlgorithm;
        private readonly IVertexColoringAlgorithm _vertexColoringAlgorithm;

        public TextReportCreator(Graph graph, ISearchAlgorithm searchAlgorithm, IVertexColoringAlgorithm vertexColoringAlgorithm)
        {
            _graph = graph;
            _searchAlgorithm = searchAlgorithm;
            _vertexColoringAlgorithm = vertexColoringAlgorithm;
        }

        public void Create(Stream outputStream)
        {
            using (var streamWriter = new StreamWriter(outputStream))
            {
                streamWriter.WriteLine("Autorzy:");
                streamWriter.WriteLine(string.Join(", ", AppInfo.Authors));
                streamWriter.WriteLine();
                streamWriter.WriteLine("Macierz incydencji:");
                for (int i = 0; i < _graph.NumberOfVertices; ++i)
                {
                    var row = Enumerable.Range(0, _graph.NumberOfVertices)
                        .Select(j =>
                        {
                            if (i == j)
                            {
                                return "-";
                            }
                            return _graph[i, j] ? "1" : "0";
                        });
                    streamWriter.WriteLine(string.Join("  ", row));
                }
                streamWriter.WriteLine();

                var isConnected = new DfsConnectivityChecker().IsConnected(_graph);
                streamWriter.WriteLine("Graf jest " + (isConnected ? "spójny" : "niespójny"));
                streamWriter.WriteLine();
                if (isConnected)
                {
                    streamWriter.WriteLine("Ciąg przeszukań (w głąb):");
                    streamWriter.WriteLine(string.Join(", ", _searchAlgorithm.Execute(_graph, 0).Select(x => x + 1)));
                    streamWriter.WriteLine();

                    streamWriter.WriteLine("Wynik kolorowania wierzchołków (algorytm zachłanny):");
                    foreach (var (vertex, color) in _vertexColoringAlgorithm.Execute(_graph))
                    {
                        streamWriter.WriteLine($@"Wierzchołek {vertex + 1} - Kolor {color + 1}");
                    }
                    streamWriter.WriteLine();
                }
            }
        }
    }
}
