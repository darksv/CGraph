﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CGraph.Core;
using CGraph.Core.Spreader;
using CGraph.Util;
using PropertyChanged;

namespace CGraph.ViewModel
{
    [ImplementPropertyChanged]
    public class GraphViewModel
    {
        public ObservableCollection<Vertex> Vertices { get; } = new ObservableCollection<Vertex>();
        public ObservableCollection<Edge> Edges { get; } = new ObservableCollection<Edge>();
        public IEnumerable<MatrixCellViewModel> AdjacencyMatrix { get; private set; }
        public int NumberOfVertices { get; set; }
        public ICommand DeselectCommand => new RelayCommand(Deselect);
        public ICommand SelectCommand => new RelayCommand<Selectable>(Select);
        public ICommand DeleteVertexCommand => new RelayCommand<Vertex>(DeleteVertex);
        public ICommand DeleteEdgeCommand => new RelayCommand<Edge>(DeleteEdge);

        private void Deselect()
        {
            foreach (var vertex in Vertices)
            {
                vertex.IsSelected = false;
            }
        }

        private void Select(Selectable selectable)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                foreach (var vertex in Vertices)
                {
                    vertex.IsSelected = false;
                }

                foreach (var edge in Edges)
                {
                    edge.IsSelected = false;
                }
            }

            selectable.IsSelected = true;
        }

        private void DeleteVertex(Vertex vertex)
        {
            if (!Vertices.Remove(vertex))
            {
                return;
            }

            var incidentEdges = Edges.Where(x => x.A == vertex || x.B == vertex)
                .ToArray();

            foreach (var incidentEdge in incidentEdges)
            {
                Edges.Remove(incidentEdge);
            }
        }

        private void DeleteEdge(Edge edge)
        {
            Edges.Remove(edge);
        }

        private void CreateFromStructure(Graph graph)
        {
            Vertices.Clear();
            Edges.Clear();

            for (int i = 1; i <= graph.NumberOfVertices; ++i)
            {
                Vertices.Add(new Vertex { Id = i });
            }

            for (int i = 0; i < graph.NumberOfVertices; ++i)
            {
                for (int j = i + 1; j < graph.NumberOfVertices; ++j)
                {
                    if (graph[i, j])
                    {
                        Edges.Add(new Edge(Vertices[i], Vertices[j]));
                    }
                }
            }
        }

        public void Show(Graph graph, SpreadMode spreadMode)
        {
            CreateFromStructure(graph);
            AdjacencyMatrix = MakeMe(graph).ToArray();
            Spread(spreadMode);
        }

        private IEnumerable<MatrixCellViewModel> MakeMe(Graph graph)
        {
            for (int i = 0; i < graph.NumberOfVertices; ++i)
            {
                for (int j = 0; j < graph.NumberOfVertices; ++j)
                {
                    yield return new MatrixCellViewModel
                    {
                        Value = graph[i, j],
                        Row = i,
                        Column = j
                    };
                }
            }
        }

        public void Spread(SpreadMode mode)
        {
            if (_spreaders.TryGetValue(mode, out var spreader))
            {
                spreader.Spread(Vertices, new Size(250, 250));
            }
        }

        private readonly Dictionary<SpreadMode, IVertexSpreader> _spreaders =
            new Dictionary<SpreadMode, IVertexSpreader>
            {
                [SpreadMode.Randomly] = new RandomVertexSpreader(),
                [SpreadMode.OnCircle] = new CircleVertexSpreader()
            };
    }
}