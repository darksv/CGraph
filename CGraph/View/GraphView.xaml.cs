using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CGraph.Core;

namespace CGraph.View
{
    public partial class GraphView : UserControl
    {
        public GraphView()
        {
            InitializeComponent();

            MouseLeftButtonUp += OnMouseLeftButtonUp;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseMove += OnMouseMove;
        }

        private bool _mouseDown = false;
        private VertexView _vertex = null;
        private Canvas _canvas = null;

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            if (_mouseDown)
            {
                ((Vertex) _vertex.DataContext).Position = args.GetPosition(_canvas);
            }
        }

        private T FindParent<T>(DependencyObject child) 
            where T : DependencyObject
        {
            while (child != null)
            {
                child = VisualTreeHelper.GetParent(child);
                if (child is T parent)
                {
                    return parent;
                }
            }

            return null;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            _mouseDown = true;
            var obj = (DependencyObject) args.OriginalSource;
            _vertex = FindParent<VertexView>(obj);
            _canvas = FindParent<Canvas>(obj);
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            _mouseDown = false;
        }
    }
}