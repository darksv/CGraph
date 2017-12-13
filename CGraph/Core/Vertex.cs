using System.Windows;
using System.Windows.Media;
using PropertyChanged;

namespace CGraph.Core
{
    [ImplementPropertyChanged]
    public class Vertex : Selectable
    {
        public int? Id { get; set; }
        [AlsoNotifyFor(nameof(Center))]
        public Point Position { get; set; }
        public double Size { get; set; } = 10;
        public Point Center => Position - new Vector(Size / 2, Size / 2);
        public int ZIndex => 1;
        public Color Color { get; set; } = Colors.White;
    }
}