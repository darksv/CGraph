using System;
using PropertyChanged;

namespace CGraph.ViewModel
{
    [ImplementPropertyChanged]
    public class MatrixCellViewModel
    {
        public bool Value { get; set; }
    }
}
