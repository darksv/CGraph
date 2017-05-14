using System;
using System.Collections.Generic;
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

        #region Commands
        
        public ICommand DeselectCommand => new RelayCommand(Deselect);
        public ICommand SelectVertexCommand => new RelayCommand<Vertex>(SelectVertex);
        public ICommand DeleteVertexCommand => new RelayCommand<Vertex>(DeleteVertex);
        public ICommand DeleteEdgeCommand => new RelayCommand<Edge>(DeleteEdge);
        public ICommand DeleteRandomVertexCommand => new RelayCommand(DeleteRandomVertex);
        public ICommand SpreadVerticesCommand => new RelayCommand(SpreadVertices);
        public ICommand CreateComplementaryGraphCommand => new RelayCommand(CreateComplementaryGraph);
        public ICommand CreateRandomEdgesCommand => new RelayCommand(
            () => CreateRandomEdges(_random.Next(0, Vertices.Count * (Vertices.Count - 1) / 2)));
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

        private void DeleteRandomVertex()
        {
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
                    X = center.X + 100 * Math.Cos((double) (i + 1) / n * 2 * Math.PI) - 2.5,
                    Y = center.Y + 100 * Math.Sin((double) (i + 1) / n * 2 * Math.PI) - 2.5
                };
            }
        }

        private void CreateComplementaryGraph()
        {
            var complementaryEdges = CreateClique()
                .Except(Edges, new EdgeEqualityComparer())
                .ToArray();

            Edges.Clear();
            foreach (var edge in complementaryEdges)
            {
                Edges.Add(edge);
            }
        }

        private void CreateRandomEdges(int numberOfEdges)
        {
            var allEdges = CreateClique()
                .OrderBy(x => Guid.NewGuid())
                .Take(numberOfEdges);

            Edges.Clear();
            foreach (var edge in allEdges)
            {
                Edges.Add(edge);
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
            var dialog = new GraphCreatorWindow();
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            Vertices.Clear();
            for (int i = 0; i < dialog.NumberOfVertices; ++i)
            {
                Vertices.Add(new Vertex());
            }
            CreateRandomEdges(dialog.NumberOfEdges);
            SpreadVertices();
        }

        private IEnumerable<Edge> CreateClique()
        {
            for (int i = 0; i < Vertices.Count; ++i)
            {
                for (int j = i + 1; j < Vertices.Count; ++j)
                {
                    yield return new Edge(Vertices[i], Vertices[j]);
                }
            }
        }

        private double CalculateAngle(Vector vec)
        {
            var angle = Math.Atan2(vec.Y, vec.X);
            return angle < 0 ? angle + 2.0 * Math.PI : angle;
        }

        public void MakeEdges()
        {
            Edges.Clear();

            foreach (var edge in CreateClique())
            {
                Edges.Add(edge);
            }
        }
    }
}
