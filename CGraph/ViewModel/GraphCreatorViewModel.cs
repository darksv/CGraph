using System;
using System.Windows.Input;
using CGraph.Util;
using PropertyChanged;

namespace CGraph.ViewModel
{
    [ImplementPropertyChanged]
    public class GraphCreatorViewModel
    {
        public int NumberOfVertices { get; set; } = 10;
        public double ProbabilityOfEdgeExistence { get; set; } = 0.0;
        public bool ConnectedOnly { get; set; } = true;
        public ICommand CreateCommand { get; }
        public bool CanExecute { get; set; } = true;
        public GraphCreatorViewModel(Action onCreate, Action onCancel)
        {
            CreateCommand = new RelayCommand(() =>
            {
                if (CanExecute)
                {
                    CanExecute = false;
                    onCreate();
                    CanExecute = true;
                }
                else
                {
                    onCancel();
                    CanExecute = true;
                }
            });
        }
    }
}