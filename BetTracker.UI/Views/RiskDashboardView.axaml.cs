using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using BetTracker.UI.ViewModels;

namespace BetTracker.UI.Views;

public partial class RiskDashboardView : UserControl
{
    public RiskDashboardView()
    {
        InitializeComponent();

        // Set DataContext to RiskDashboardViewModel via DI
        if (App.ServiceProvider != null)
        {
            DataContext = App.ServiceProvider.GetRequiredService<RiskDashboardViewModel>();
        }
    }
}
