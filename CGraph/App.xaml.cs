using System.Windows;
using CGraph.View;
using CGraph.ViewModel;

namespace CGraph
{
    public partial class App : Application
    {
        public App()
        {
            var window = new MainWindow();
            var viewModel = new MainViewModel(window);
            window.DataContext = viewModel;
            MainWindow = window;
        }

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            MainWindow?.ShowDialog();
        }
    }
}