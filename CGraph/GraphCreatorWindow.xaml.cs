using System.Windows;

namespace CGraph
{
    public partial class GraphCreatorWindow : Window
    {
        private readonly GraphCreatorViewModel _graphCreator;

        public int NumberOfVertices => _graphCreator.NumberOfVertices;
        public double ProbabilityOfEdgeExistence => _graphCreator.ProbabilityOfEdgeExistence;

        public GraphCreatorWindow()
        {
            InitializeComponent();

            _graphCreator = new GraphCreatorViewModel(() =>
            {
                DialogResult = true;
            }, Close);
            DataContext = _graphCreator;
        }
    }
}
