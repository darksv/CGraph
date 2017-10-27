using System.Windows;
using PropertyChanged;

namespace CGraph.Core
{
    [ImplementPropertyChanged]
    public class Vertex
    {
        public string Name { get; set; }
        [AlsoNotifyFor(nameof(Center))]
        public Point Position { get; set; }
        public double Size { get; set; } = 10;
        public bool IsSelected { get; set; }
        public Point Center => Position - new Vector(Size / 2, Size / 2);
        public int ZIndex => 1;
    }
}