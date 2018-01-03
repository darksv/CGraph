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

        public TextReportCreator(Graph graph, ISearchAlgorithm searchAlgorithm)
        {
            _graph = graph;
            _searchAlgorithm = searchAlgorithm;
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
                if (isConnected)
                {
                    streamWriter.WriteLine("Ciąg przeszukań (przeszukiwanie wgłąb):");
                    streamWriter.WriteLine(string.Join(", ", _searchAlgorithm.Execute(_graph, 0).Select(x => x + 1)));
                }
            }
        }
    }
}
