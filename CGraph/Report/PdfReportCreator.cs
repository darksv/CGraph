using System.IO;
using System.Linq;
using CGraph.Core;
using CGraph.Core.Algorithm;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace CGraph.Report
{
    internal class PdfReportCreator : IReportCreator
    {
        private readonly Graph _graph;
        private readonly ISearchAlgorithm _searchAlgorithm;

        public PdfReportCreator(Graph graph, ISearchAlgorithm searchAlgorithm)
        {
            _graph = graph;
            _searchAlgorithm = searchAlgorithm;
        }

        public void Create(Stream outputStream)
        {
            var document = new Document();
            DefineStyles(document);

            var section = document.AddSection();
            var header = section.AddParagraph("Autorzy: ");
            header.Format.Font.Size = 24;
            header.Format.Alignment = ParagraphAlignment.Right;

            foreach (var name in new[] {})
            {
                var p = section.AddParagraph(name);
                p.Format.Alignment = ParagraphAlignment.Right;
            }

            header = section.AddParagraph("Macierz incydencji:");
            header.Format.Font.Size = 18;

            var numberOfVertices = _graph.NumberOfVertices;

            var table = section.AddTable();
            table.Columns = new Columns(
                Enumerable
                    .Repeat(0, numberOfVertices + 1)
                    .Select(_ => new Unit(20.0, UnitType.Point))
                    .ToArray()
            );
            table.Rows = new Rows
            {
                Height = new Unit(20, UnitType.Point)
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

            header = section.AddParagraph("Przeszukiwanie:");
            header.Format.Font.Size = 18;

            section.AddParagraph(string.Join(", ", _searchAlgorithm.Execute(_graph, 0).Select(x => x + 1)));

            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always)
            {
                Document = document
            };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(outputStream);
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