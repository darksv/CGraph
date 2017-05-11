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
        public ObservableCollection<Vertex> Vertices { get; } = new ObservableCollection<Vertex>();
        public ObservableCollection<Edge> Edges { get; } = new ObservableCollection<Edge>();

        public ICommand SelectVertexCommand => new RelayCommand<Vertex>(vertex =>
        {
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

            if (Edges.Any(e => e.A == previousVertex && e.B == vertex ||
                               e.B == previousVertex && e.A == vertex))
            {
                return;
            }

            Edges.Add(new Edge(previousVertex, vertex));
        });

        // ReSharper disable once UnusedMember.Global
        public ICommand RemoveVertexCommand => new RelayCommand<Vertex>(vertex =>
        {
            if (!Vertices.Remove(vertex))
            {
                return;
            }

            var incidentEdges = Edges
                .Where(x => x.A == vertex || x.B == vertex)
                .ToArray();

            foreach (var incidentEdge in incidentEdges)
            {
                Edges.Remove(incidentEdge);
            }
        });

        // ReSharper disable once UnusedMember.Global
        public ICommand RemoveEdgeCommand => new RelayCommand<Edge>(edge =>
        {
            Edges.Remove(edge);
        });

        public ICommand SpreadVerticesCommand => new RelayCommand(() =>
        {
            var n = Vertices.Count;

            var center = new Point(125, 125);
            var sortedVertices = Vertices
                .Select(vertex => new
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
        });

        public ICommand CreateComplementaryCommand => new RelayCommand(() =>
        {
            var complementaryEdges = CreateClique()
                .Except(Edges, new EdgeEqualityComparer())
                .ToArray();

            Edges.Clear();
            foreach (var edge in complementaryEdges)
            {
                Edges.Add(edge);
            }
        });

        public ICommand ClearCommand => new RelayCommand(() =>
        {
            var dialogResult = MessageBox.Show(
                "Graf zostanie usunięty. Czy chcesz kontynuować?",
                "Potwierdzenie",
                MessageBoxButton.YesNo);

            if (dialogResult != MessageBoxResult.Yes)
            {
                return;
            }
            Edges.Clear();
            Vertices.Clear();
        });

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
        
//        public void MakeVertices(int k)
//        {
//            Vertices.Clear();
//            for (int i = 0; i < k; ++i)
//            {
//                Vertices.Add(new Vertex
//                {
//                    Name = $"Wierzcholek {i}"
//                });
//            }
//        }

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
