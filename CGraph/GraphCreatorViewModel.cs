using System;
using System.Windows;
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
        public bool CanExecute { get; set; } = true;

        public GraphCreatorViewModel(Action onCreate)
        {
            CreateCommand = new RelayCommand(() =>
            {
                CanExecute = false;
                onCreate();
                CanExecute = true;
            });
        }
    }
}
