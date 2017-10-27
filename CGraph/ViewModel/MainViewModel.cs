﻿using System.Windows.Input;
using CGraph.Core;
using CGraph.Core.Algorithm;
using CGraph.Core.Generator;
using CGraph.Util;
using PropertyChanged;

namespace CGraph.ViewModel
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
        public GraphCreatorViewModel GraphCreator { get; }
        public GraphViewModel Graph { get; } = new GraphViewModel();
        public bool IsRandomlySelected { get; set; } = true;
        public bool IsOnCircleSelected { get; set; }
        public bool IsGenerating { get; set; } = false;
        public bool IsConnected { get; set; } = false;

        public MainViewModel()
        {
            GraphCreator = new GraphCreatorViewModel(Generate);
            SpreadVerticesCommand = new RelayCommand(() => Graph.Spread(GetSpreadMode()));
        }

        public ICommand SpreadVerticesCommand { get; }
        public ICommand GenerateCommand => new RelayCommand(Generate, CanGenerate);

        private bool CanGenerate()
        {
            return !IsGenerating;
        }

        private void Generate()
        {
            var numberOfVertices = GraphCreator.NumberOfVertices;
            var probabilityOfEdgeExistence = GraphCreator.ProbabilityOfEdgeExistence;

            var generator = new ConnectedGraphGenerator(numberOfVertices, probabilityOfEdgeExistence);
            var graph = generator.Generate();
            if (graph == null)
            {
                return;
            }

            Graph.Show(graph, GetSpreadMode());

            var algorithm = new DfsAlgorithm();
            algorithm.Execute(graph, 1);
            IsConnected = algorithm.IsConnected();
        }

        private SpreadMode GetSpreadMode()
        {
            return IsOnCircleSelected ? SpreadMode.OnCircle : SpreadMode.Randomly;
        }
    }
}