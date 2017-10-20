using System.Windows;

namespace CGraph
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _main = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _main;
        }
    }
}
