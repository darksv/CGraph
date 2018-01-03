using System;
using CGraph.Core;
using CGraph.Core.Algorithm;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Table = MigraDoc.DocumentObjectModel.Tables.Table;

namespace CGraph.Report
{
    internal class PdfReportCreator : IReportCreator
    {
        private readonly Graph _graph;
        private readonly ISearchAlgorithm _searchAlgorithm;
        private readonly IGraphImageProvider _graphImageProvider;
        private IVertexColoringAlgorithm _vertexColoringAlgorithm;

        public PdfReportCreator(Graph graph, ISearchAlgorithm searchAlgorithm, IGraphImageProvider graphImageProvider, IVertexColoringAlgorithm vertexColoringAlgorithm)
        {
            _graph = graph;
            _searchAlgorithm = searchAlgorithm;
            _graphImageProvider = graphImageProvider;
            _vertexColoringAlgorithm = vertexColoringAlgorithm;
        }

        public void Create(Stream outputStream)
        {
            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = CreateDocument();
            renderer.RenderDocument();
            renderer.Save(outputStream, false);
        }

        private Document CreateDocument()
        {
            var document = new Document();
            DefineStyles(document);

            var section = document.AddSection();
            var header = section.AddParagraph("Autorzy: ");
            header.Format.Font.Size = 24;
            header.Format.Alignment = ParagraphAlignment.Right;

            foreach (var name in AppInfo.Authors)
            {
                var p = section.AddParagraph(name);
                p.Format.Alignment = ParagraphAlignment.Right;
            }

            section.AddImage(CreateImage());

            header = section.AddParagraph("Macierz incydencji:");
            header.Format.Font.Size = 18;
            section.Add(BuildIncidencyMatrix());

            var isConnected = new DfsConnectivityChecker().IsConnected(_graph);

            header = section.AddParagraph("Spójność:");
            header.Format.Font.Size = 18;
            section.AddParagraph("Graf jest " + (isConnected ? "spójny" : "niespójny"));

            if (isConnected)
            {
                header = section.AddParagraph("Ciąg przeszukań (przeszukiwanie w głąb):");
                header.Format.Font.Size = 18;
                section.AddParagraph(string.Join(", ", _searchAlgorithm.Execute(_graph, 0).Select(x => x + 1)));

                header = section.AddParagraph("Wynik kolorowania wierzchołków (algorytm zachłanny):");
                header.Format.Font.Size = 18;
                foreach (var (vertex, color) in _vertexColoringAlgorithm.Execute(_graph))
                {
                    section.AddParagraph($@"Wierzchołek {vertex + 1} - Kolor {color + 1}");
                }
            }

            return document;
        }

        private string CreateImage()
        {
            using (var stream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource) _graphImageProvider.Capture()));
                encoder.Save(stream);
                return "base64:" + Convert.ToBase64String(stream.ToArray());
            }
        }

        private Table BuildIncidencyMatrix()
        {
            var numberOfVertices = _graph.NumberOfVertices;
            var table = new Table
            {
                Columns = new Columns(
                    Enumerable
                        .Repeat(0, numberOfVertices + 1)
                        .Select(_ => new Unit(20.0, UnitType.Point))
                        .ToArray()
                ),
                Rows = new Rows
                {
                    Height = new Unit(20, UnitType.Point)
                }
            };

            var headerRow = table.AddRow();
            for (int i = 0; i <= numberOfVertices; ++i)
            {
                var p = headerRow.Cells[i];
                p.Style = "TableHeader";
                p.AddParagraph(i == 0 ? string.Empty : i.ToString());
            }

            for (int i = 0; i < numberOfVertices; ++i)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph((i + 1).ToString()).Style = "TableHeader";

                for (int j = 0; j < numberOfVertices; ++j)
                {
                    var content = i == j
                        ? "-"
                        : (_graph[i, j] ? "1" : "0");

                    row.Cells[j + 1].AddParagraph(content).Format.Alignment = ParagraphAlignment.Center;
                }
            }

            return table;
        }
        
        private void DefineStyles(Document document)
        {
            document.DefaultPageSetup.RightMargin = 50;
            document.DefaultPageSetup.LeftMargin = 50;

            var style = document.Styles["Normal"];
            style.Font.Name = "Arial";
            style.Font.Size = 12;

            style = document.Styles.AddStyle("TableHeader", "Normal");
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        }
    }
}