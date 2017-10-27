using System.Windows;
using System.Windows.Controls;

namespace CGraph.View
{
    public partial class VertexView : UserControl
    {
        public static readonly DependencyProperty IsSelectedProperty
            = DependencyProperty.Register(
                nameof(IsSelected),
                typeof(bool),
                typeof(VertexView),
                new PropertyMetadata(false)
            );

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty LabelProperty
            = DependencyProperty.Register(
                nameof(Label),
                typeof(string),
                typeof(VertexView),
                new PropertyMetadata(string.Empty)
            );

        public string Label
        {
            get => (string) GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public VertexView()
        {
            InitializeComponent();
        }
    }
}