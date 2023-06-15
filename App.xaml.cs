using System.Windows;

namespace ModbusTCPApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            MainWindow view = new MainWindow();
            MainWindowViewModel viewModel = new MainWindowViewModel();
            view.DataContext = viewModel;

            view.ShowDialog();
        }
    }
}