// Copyright (c) SharpCrafters s.r.o. All rights reserved.
// This project is not open source. Please see the LICENSE.md file in the repository root for details.

namespace Metalama.Open.DependencyEmbedder.WpfApp;

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