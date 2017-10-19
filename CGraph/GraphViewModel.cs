using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;

namespace CGraph
{
    [ImplementPropertyChanged]
    public class GraphViewModel
    {
        private readonly Random _random = new Random();

        public ObservableCollection<Vertex> Vertices { get; } = new ObservableCollection<Vertex>();
        public ObservableCollection<Edge> Edges { get; } = new ObservableCollection<Edge>();
        public GraphCreatorViewModel GraphCreatorViewModel { get; }

        public GraphViewModel()
        {
            GraphCreatorViewModel = new GraphCreatorViewModel(CreateGraph);
        }
        
        #region Commands
        
        public ICommand DeselectCommand => new RelayCommand(Deselect);
        public ICommand SelectVertexCommand => new RelayCommand<Vertex>(SelectVertex);
        public ICommand DeleteVertexCommand => new RelayCommand<Vertex>(DeleteVertex);
        public ICommand DeleteEdgeCommand => new RelayCommand<Edge>(DeleteEdge);
        public ICommand DeleteRandomVertexCommand => new RelayCommand(DeleteRandomVertex);
        public ICommand SpreadVerticesCommand => new RelayCommand(SpreadVertices);
        public ICommand DeleteGraphCommand => new RelayCommand(DeleteGraph);
        public ICommand DeleteSelectedCommand => new RelayCommand(DeleteSelected);
        public ICommand CreateGraphCommand => new RelayCommand(CreateGraph);

        #endregion

        private void Deselect()
        {
            foreach (var vertex in Vertices)
            {
                vertex.IsSelected = false;
            }
        }

        private void SelectVertex(Vertex vertex)
        {
            return;
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                vertex.IsSelected = true;
                return;
            }

            var previousVertex = Vertices.FirstOrDefault(x => x.IsSelected);
            if (previousVertex == vertex)
            {
                return;
            }

            vertex.IsSelected = true;
            if (previousVertex == null)
            {
                return;
            }

            previousVertex.IsSelected = false;

            if (Edges.Any(e => e.A == previousVertex && e.B == vertex || e.B == previousVertex && e.A == vertex))
            {
                return;
            }

            Edges.Add(new Edge(previousVertex, vertex));
        }

        private void DeleteVertex(Vertex vertex)
        {
            return;
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
            return;
            Edges.Remove(edge);
        }

        private void DeleteRandomVertex()
        {
            return;
            var randomVertex = Vertices.OrderBy(x => Guid.NewGuid())
                .FirstOrDefault();

            if (randomVertex != null)
            {
                DeleteVertex(randomVertex);
            }
        }

        private void SpreadVertices()
        {
            var n = Vertices.Count;

            var center = new Point(125, 125);
            var sortedVertices = Vertices.Select(vertex => new
                {
                    Vertex = vertex,
                    Angle = CalculateAngle(vertex.Position - center)
                })
                .OrderBy(x => x.Angle)
                .Select(x => x.Vertex)
                .ToArray();

            for (int i = 0; i < n; ++i)
            {
                sortedVertices[i].Position = new Point
                {
                    X = center.X + 100 * Math.Cos((double) (i + 1) / n * 2 * Math.PI) - 5.0,
                    Y = center.Y + 100 * Math.Sin((double) (i + 1) / n * 2 * Math.PI) - 5.0
                };
            }
        }

        private void DeleteGraph()
        {
            var dialogResult = MessageBox.Show("Graf zostanie usunięty. Czy chcesz kontynuować?", "Potwierdzenie", MessageBoxButton.YesNo);

            if (dialogResult != MessageBoxResult.Yes)
            {
                return;
            }
            Edges.Clear();
            Vertices.Clear();
        }

        private void DeleteSelected()
        {
            var verticesToDelete = Vertices.Where(x => x.IsSelected)
                .ToArray();

            foreach (var vertex in verticesToDelete)
            {
                DeleteVertex(vertex);
            }
        }

        private void CreateGraph()
        {
            var numberOfVertices = GraphCreatorViewModel.NumberOfVertices;
            var probabilityOfEdgeExistence = GraphCreatorViewModel.ProbabilityOfEdgeExistence;

            var generator = new ConnectedGraphGenerator(numberOfVertices, probabilityOfEdgeExistence);
            var graph = generator.Generate();
            if (graph == null)
            {
                return;
            }

            DisplayGraph(graph);
            SpreadVertices();
        }

        private void DisplayGraph(Graph graph)
        {
            Vertices.Clear();
            Edges.Clear();

            for (int i = 1; i <= graph.NumberOfVertices; ++i)
            {
                Vertices.Add(new Vertex { Name = i.ToString() });
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

        private double CalculateAngle(Vector vec)
        {
            var angle = Math.Atan2(vec.Y, vec.X);
            return angle < 0 ? angle + 2.0 * Math.PI : angle;
        }
    }
}
