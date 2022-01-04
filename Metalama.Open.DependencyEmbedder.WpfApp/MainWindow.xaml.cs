using System.Windows;

namespace Metalama.Open.DependencyEmbedder.WpfApp
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