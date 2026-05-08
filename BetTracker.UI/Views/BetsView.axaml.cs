using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using BetTracker.UI.ViewModels;

namespace BetTracker.UI.Views;

public partial class BetsView : UserControl
{
    public BetsView()
    {
        InitializeComponent();

        // Set DataContext to BetsViewModel via DI
        if (App.ServiceProvider != null)
        {
            DataContext = App.ServiceProvider.GetRequiredService<BetsViewModel>();

            // Load bets on view initialization
            if (DataContext is BetsViewModel viewModel)
            {
                viewModel.LoadBetsCommand.Execute(null);
            }
        }
    }
}
