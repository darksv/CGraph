using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CGraph.Core;
using CGraph.Core.Algorithm;
using CGraph.Core.Generator;
using CGraph.Report;
using CGraph.Util;
using Microsoft.Win32;
using PropertyChanged;
using DistinctColors = CGraph.Core.Colors;

namespace CGraph.ViewModel
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
        private bool _isGenerating = false;
        private Graph _graph = null;
        private IGraphImageProvider _graphImageProvider;
        public GraphCreatorViewModel GraphCreator { get; }
        public GraphViewModel Graph { get; } = new GraphViewModel();
        public bool IsRandomlySelected { get; set; } = true;
        public bool IsOnCircleSelected { get; set; }
        public bool IsGenerating { get; set; } = false;
        public bool IsConnected { get; set; } = false;
        public IEnumerable<int> SearchSequence { get; set; }
        public ICommand CreateReportCommand { get; set; }

        public MainViewModel(IGraphImageProvider graphImageProvider)
        {
            GraphCreator = new GraphCreatorViewModel(StartGenerating, StopGenerating);
            SpreadVerticesCommand = new RelayCommand(() => Graph.Spread(GetSpreadMode()));
            CreateReportCommand = new RelayCommand(CreateReport);
            _graphImageProvider = graphImageProvider;
        }

        public ICommand SpreadVerticesCommand { get; }
        public ICommand GenerateCommand => new RelayCommand(StartGenerating);

        public void CreateReport()
        {
            if (_graph == null)
            {
                MessageBox.Show("Nie wygenerowano grafu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "Plik tekstowy|*.txt|Plik PDF|*.pdf"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            IReportCreator reportCreator = null;
            switch (Path.GetExtension(dialog.FileName).ToLower())
            {
                case ".pdf":
                    reportCreator = new PdfReportCreator(_graph, new DfsAlgorithm(), _graphImageProvider, new GreedyVertexColoringAlgorithm());
                    break;
                default:
                    reportCreator = new TextReportCreator(_graph, new DfsAlgorithm(), new GreedyVertexColoringAlgorithm());
                    break;
            }

            using (var outputStream = new FileStream(dialog.FileName, FileMode.OpenOrCreate))
            {
                outputStream.Position = 0;
                outputStream.SetLength(0);
                reportCreator.Create(outputStream);
            }
        }

        private void StartGenerating()
        {
            _isGenerating = true;

            if (GraphCreator.ConnectedOnly)
            {
                do
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GenerateAny();
                        GraphCreator.ProbabilityOfEdgeExistence =
                            Math.Min(GraphCreator.ProbabilityOfEdgeExistence + 0.001, 1.0);
                    }, DispatcherPriority.Background);
                } while (!IsConnected && _isGenerating);
            }
            else
            {
                GenerateAny();
            }

            _isGenerating = false;
        }

        private void StopGenerating()
        {
            _isGenerating = false;
        }

        private void GenerateAny()
        {
            var numberOfVertices = GraphCreator.NumberOfVertices;
            var probabilityOfEdgeExistence = GraphCreator.ProbabilityOfEdgeExistence;

            var generator = new ConnectedGraphGenerator(numberOfVertices, probabilityOfEdgeExistence);
            _graph = generator.Generate();
            if (_graph == null)
            {
                return;
            }

            Graph.Show(_graph, GetSpreadMode());
            IsConnected = new DfsConnectivityChecker().IsConnected(_graph);
            SearchSequence = new DfsAlgorithm().Execute(_graph, 0).Select(x => x + 1);

            if (!IsConnected)
            {
                return;
            }

            var colors = DistinctColors.DistinctColors.OrderBy(x => Guid.NewGuid()).Take(numberOfVertices).ToArray();
            foreach (var (vertex, color) in new GreedyVertexColoringAlgorithm().Execute(_graph))
            {
                Graph.Vertices[vertex].Color = colors[color];
            }
        }

        private SpreadMode GetSpreadMode()
        {
            return IsOnCircleSelected ? SpreadMode.OnCircle : SpreadMode.Randomly;
        }
    }
}