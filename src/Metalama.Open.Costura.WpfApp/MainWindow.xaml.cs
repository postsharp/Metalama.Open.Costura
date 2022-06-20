// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

namespace Metalama.Open.Costura.WpfApp;

/// <summary>
///     Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.Loaded += ( _, _ ) => { this.Close(); };
    }
}