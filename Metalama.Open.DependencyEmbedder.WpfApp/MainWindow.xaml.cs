using System.Windows;

namespace Caravela.Open.DependencyEmbedder.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) => { Close(); };
        }
    }
}