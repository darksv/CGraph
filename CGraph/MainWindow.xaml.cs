using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CGraph
{
    public partial class MainWindow : Window
    {
        private readonly GraphViewModel _graph = new GraphViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _graph;
            _graph.MakeEdges();
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            if (args.ChangedButton != MouseButton.Left || args.ClickCount != 2)
            {
                return;
            }
            args.Handled = true;

            _graph.Vertices.Add(new Vertex
            {
                Position = args.GetPosition((Canvas) sender)
            });
        }
    }
}
