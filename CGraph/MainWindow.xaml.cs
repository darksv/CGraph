using System.Windows;

namespace CGraph
{
    public partial class MainWindow : Window
    {
        private readonly GraphViewModel _graph = new GraphViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _graph;
        }
    }
}
