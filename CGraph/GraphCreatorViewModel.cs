using System;
using System.Windows.Input;
using PropertyChanged;

namespace CGraph
{
    [ImplementPropertyChanged]
    public class GraphCreatorViewModel
    {
        public int NumberOfVertices { get; set; }
        public int NumberOfEdges { get; set; }
        public int MaximumNumberOfEdges => NumberOfVertices * (NumberOfVertices - 1) / 2;
        public bool CanCreate => NumberOfEdges <= MaximumNumberOfEdges;
        public ICommand CreateCommand { get; }
        public ICommand CancelCommand { get; }

        public GraphCreatorViewModel(Action onCreate, Action onCancel)
        {
            CreateCommand = new RelayCommand(onCreate);
            CancelCommand = new RelayCommand(onCancel);
        }
    }
}
