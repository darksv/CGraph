using System;
using System.Windows.Input;
using PropertyChanged;

namespace CGraph
{
    [ImplementPropertyChanged]
    public class GraphCreatorViewModel
    {
        public int NumberOfVertices { get; set; } = 10;
        public double ProbabilityOfEdgeExistence { get; set; } = 0.35;
        public ICommand CreateCommand { get; }

        public GraphCreatorViewModel(Action onCreate)
        {
            CreateCommand = new RelayCommand(onCreate);
        }
    }
}
