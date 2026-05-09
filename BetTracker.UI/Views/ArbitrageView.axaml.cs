using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using BetTracker.UI.ViewModels;

namespace BetTracker.UI.Views;

public partial class ArbitrageView : UserControl
{
    public ArbitrageView()
    {
        InitializeComponent();

        // Set DataContext to ArbitrageViewModel via DI
        if (App.ServiceProvider != null)
        {
            DataContext = App.ServiceProvider.GetRequiredService<ArbitrageViewModel>();
        }
    }
}
