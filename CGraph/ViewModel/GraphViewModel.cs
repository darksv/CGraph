using System.Collections.Generic;
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
        public Vertex[] Vertices { get; private set; }
        public Edge[] Edges { get; private set; }
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
            Vertices = Vertices.Where(v => !ReferenceEquals(v, vertex)).ToArray();
            Edges = Edges.Where(e => e.A != vertex && e.B != vertex).ToArray();
        }

        private void DeleteEdge(Edge edge)
        {
            Edges = Edges.Where(x => !ReferenceEquals(x, edge)).ToArray();
        }

        private void CreateFromStructure(Graph graph)
        {
            Vertices = CreateVertices(graph).ToArray();
            Edges = CreateEdges(graph).ToArray();
        }

        private static IEnumerable<Vertex> CreateVertices(Graph graph)
        {
            return Enumerable
                .Range(1, graph.NumberOfVertices)
                .Select(x => new Vertex {Id = x});
        }

        private IEnumerable<Edge> CreateEdges(Graph graph)
        {
            for (int i = 0; i < graph.NumberOfVertices; ++i)
            {
                for (int j = i + 1; j < graph.NumberOfVertices; ++j)
                {
                    if (graph[i, j])
                    {
                        yield return new Edge(Vertices[i], Vertices[j]);
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