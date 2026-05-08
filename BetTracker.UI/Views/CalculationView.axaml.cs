using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using BetTracker.UI.ViewModels;

namespace BetTracker.UI.Views;

public partial class CalculationView : UserControl
{
    public CalculationView()
    {
        InitializeComponent();

        // Set DataContext to CalculationViewModel via DI
        if (App.ServiceProvider != null)
        {
            DataContext = App.ServiceProvider.GetRequiredService<CalculationViewModel>();
        }
    }
}
