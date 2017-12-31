using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CGraph.Core;

namespace CGraph.View
{
    public partial class MainWindow : Window, IGraphImageProvider
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public ImageSource Capture()
        {
            return RenderToBitmap(GraphView);
        }

        private RenderTargetBitmap RenderToBitmap(UIElement element)
        {
            var renderTarget = new RenderTargetBitmap((int)element.RenderSize.Width,
                (int)element.RenderSize.Height, 96, 96, PixelFormats.Default);

            var sourceBrush = new VisualBrush(element);
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                var rect = new Rect(
                    new Size(element.RenderSize.Width, element.RenderSize.Height)
                );
                drawingContext.DrawRectangle(sourceBrush, null, rect);
            }
            renderTarget.Render(drawingVisual);
            return renderTarget;
        }
    }
}