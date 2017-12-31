using System.Windows.Media;

namespace CGraph.Core
{
    public interface IGraphImageProvider
    {
        ImageSource Capture();
    }
}
