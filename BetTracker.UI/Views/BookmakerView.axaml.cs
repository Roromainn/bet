using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using BetTracker.UI.ViewModels;

namespace BetTracker.UI.Views;

public partial class BookmakerView : UserControl
{
    public BookmakerView()
    {
        InitializeComponent();

        // Set DataContext to BookmakerViewModel via DI
        if (App.ServiceProvider != null)
        {
            DataContext = App.ServiceProvider.GetRequiredService<BookmakerViewModel>();

            // Load bookmakers on view initialization
            if (DataContext is BookmakerViewModel viewModel)
            {
                viewModel.LoadBookmakersCommand.Execute(null);
            }
        }
    }
}
