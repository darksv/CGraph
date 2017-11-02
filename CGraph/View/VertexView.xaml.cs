using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public static readonly DependencyProperty ColorProperty
            = DependencyProperty.Register(
                nameof(Color),
                typeof(Color),
                typeof(VertexView),
                new PropertyMetadata(Colors.White, PropertyChangedCallback)
            );

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var self = (VertexView) sender;
            self.BackgroundBrush = new SolidColorBrush(self.Color);

            var color = self.Color;
            var intensity = color.R * 0.299 + color.G * 0.587 + color.B * 0.114;
            var textColor = intensity < 0.5 ? Colors.White : Colors.Black;

            self.TextBrush = new SolidColorBrush(textColor);
        }

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        private static readonly DependencyPropertyKey TextBrushPropertyKey
            = DependencyProperty.RegisterReadOnly(
                nameof(TextBrush),
                typeof(Brush),
                typeof(VertexView),
                new PropertyMetadata(Brushes.Black)
            );

        public static readonly DependencyProperty TextBrushProperty
            = TextBrushPropertyKey.DependencyProperty;

        public Brush TextBrush
        {
            get => (Brush)GetValue(TextBrushProperty);
            private set => SetValue(TextBrushPropertyKey, value);
        }

        private static readonly DependencyPropertyKey BackgroundBrushPropertyKey
            = DependencyProperty.RegisterReadOnly(
                nameof(BackgroundBrush),
                typeof(Brush),
                typeof(VertexView),
                new PropertyMetadata(Brushes.White)
            );

        public static readonly DependencyProperty BackgroundBrushProperty
            = BackgroundBrushPropertyKey.DependencyProperty;

        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            private set => SetValue(BackgroundBrushPropertyKey, value);
        }

        public VertexView()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}