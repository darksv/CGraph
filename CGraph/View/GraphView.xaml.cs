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
        private Vertex _vertex = null;
        private Canvas _canvas = null;
        private Vector _offset;

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            if (_mouseDown)
            {
                _vertex.Position = args.GetPosition(_canvas) - _offset;
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
            var control = (DependencyObject) args.OriginalSource;
            var vertexControl = FindParent<VertexView>(control);
            if (vertexControl == null)
            {
                return;
            }

            var relativePos = args.GetPosition(vertexControl);
            _offset = new Vector(
                relativePos.X - vertexControl.Width / 2,
                relativePos.Y - vertexControl.Height / 2
            );

            _vertex = (Vertex) vertexControl.DataContext;
            _canvas = FindParent<Canvas>(control);

            if (_canvas != null)
            {
                _mouseDown = true;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            _mouseDown = false;
        }
    }
}