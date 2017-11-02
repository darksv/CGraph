using PropertyChanged;

namespace CGraph.Core
{
    [ImplementPropertyChanged]
    public abstract class Selectable
    {
        public bool IsSelected { get; set; }
    }
}
