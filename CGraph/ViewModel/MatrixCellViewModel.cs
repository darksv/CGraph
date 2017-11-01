using System;
using PropertyChanged;

namespace CGraph.ViewModel
{
    [ImplementPropertyChanged]
    public class MatrixCellViewModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Value { get; set; }
        public bool Self => Column == Row;
    }
}
