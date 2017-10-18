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
        public ICommand CancelCommand { get; }

        public GraphCreatorViewModel(Action onCreate, Action onCancel)
        {
            CreateCommand = new RelayCommand(onCreate);
            CancelCommand = new RelayCommand(onCancel);
        }
    }
}
