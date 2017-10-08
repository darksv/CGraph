using System.Windows;
using PropertyChanged;

namespace CGraph
{
    [ImplementPropertyChanged]
    public class Vertex
    {
        public string Name { get; set; }

        public bool IsSelected { get; set; }

        [AlsoNotifyFor(nameof(Center))]
        public Point Position { get; set; }

        public Point Center => Position - new Vector(5.0, 5.0);

        public int ZIndex => 1;
    }
}
