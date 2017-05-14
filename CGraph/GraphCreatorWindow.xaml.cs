using System.Windows;

namespace CGraph
{
    public partial class GraphCreatorWindow : Window
    {
        private readonly GraphCreatorViewModel _graphCreator;

        public int NumberOfVertices => _graphCreator.NumberOfVertices;
        public int NumberOfEdges => _graphCreator.NumberOfEdges;

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
