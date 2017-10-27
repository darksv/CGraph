using System.Windows;
using CGraph.ViewModel;

namespace CGraph.View
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