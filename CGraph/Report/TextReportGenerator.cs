using System.IO;
using System.Linq;
using CGraph.Core;
using CGraph.Core.Algorithm;

namespace CGraph.Report
{
    class TextReportGenerator : IReportGenerator
    {
        private readonly Graph _graph;

        public TextReportGenerator(Graph graph)
        {
            _graph = graph;
        }

        public void Generate(StreamWriter output)
        {
            output.WriteLine("Macierz incydencji:");
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
                output.WriteLine(string.Join("  ", row));
            }
            output.WriteLine();
            output.WriteLine("Ciąg przeszukań (przeszukiwanie wgłąb):");
            output.WriteLine(string.Join(", ", new DfsAlgorithm().Execute(_graph, 0).Select(x => x + 1)));
        }
    }
}
