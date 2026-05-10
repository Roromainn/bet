using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using BetTracker.Core.Models;
using BetTracker.Data.Repositories;
using BetTracker.UI.ViewModels;

namespace BetTracker.Tests.ViewModels;

/// <summary>
/// Unit tests for BookmakerViewModel using mocked repositories.
/// Tests async operations, validation, and confirmation dialog flows.
/// </summary>
public class BookmakerViewModelTests
{
    private readonly Mock<IBookmakerRepository> _mockBookmakerRepository;
    private readonly BookmakerViewModel _viewModel;

    public BookmakerViewModelTests()
    {
        _mockBookmakerRepository = new Mock<IBookmakerRepository>();
        _viewModel = new BookmakerViewModel(_mockBookmakerRepository.Object);
    }

    [Fact]
    public async Task LoadBookmakers_WithValidBookmakers_PopulatesCollection()
    {
        // Arrange
        var activeBookmakers = new List<Bookmaker>
        {
            new Bookmaker { Id = 1, Nom = "Bet365", UrlLogo = "https://example.com/logo1.png", Actif = true, DateActivation = DateTime.Now },
            new Bookmaker { Id = 2, Nom = "DraftKings", UrlLogo = "https://example.com/logo2.png", Actif = true, DateActivation = DateTime.Now }
        };

        _mockBookmakerRepository
            .Setup(r => r.GetActiveAsync())
            .ReturnsAsync(activeBookmakers);

        // Act
        await _viewModel.LoadBookmakersCommand.ExecuteAsync(null);

        // Assert
        Assert.Equal(2, _viewModel.Bookmakers.Count);
        Assert.False(_viewModel.IsLoading);
        Assert.Null(_viewModel.ErrorMessage);
    }

    [Fact]
    public async Task LoadBookmakers_WithException_SetsErrorMessage()
    {
        // Arrange
        _mockBookmakerRepository
            .Setup(r => r.GetActiveAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        await _viewModel.LoadBookmakersCommand.ExecuteAsync(null);

        // Assert
        Assert.NotNull(_viewModel.ErrorMessage);
        Assert.Contains("Failed to load bookmakers", _viewModel.ErrorMessage);
        Assert.False(_viewModel.IsLoading);
    }

    [Fact]
    public async Task AddBookmaker_WithValidInputs_AddsToCollection()
    {
        // Arrange
        _viewModel.NewBookmakerName = "FanDuel";
        _viewModel.NewBookmakerLogoUrl = "https://example.com/fanduel.png";

        var newBookmaker = new Bookmaker
        {
            Id = 1,
            Nom = "FanDuel",
            UrlLogo = "https://example.com/fanduel.png",
            Actif = true,
            DateActivation = DateTime.Now
        };

        _mockBookmakerRepository
            .Setup(r => r.AddAsync(It.IsAny<Bookmaker>()))
            .Returns(Task.CompletedTask);

        _mockBookmakerRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _viewModel.AddBookmakerCommand.ExecuteAsync(null);

        // Assert
        Assert.Single(_viewModel.Bookmakers);
        Assert.Empty(_viewModel.NewBookmakerName);
        Assert.Empty(_viewModel.NewBookmakerLogoUrl);
    }

    [Fact]
    public async Task AddBookmaker_WithEmptyName_SetsValidationError()
    {
        // Arrange
        _viewModel.NewBookmakerName = "";
        _viewModel.NewBookmakerLogoUrl = "https://example.com/logo.png";

        // Act
        await _viewModel.AddBookmakerCommand.ExecuteAsync(null);

        // Assert
        Assert.NotNull(_viewModel.NameError);
        Assert.Contains("required", _viewModel.NameError);
        Assert.Empty(_viewModel.Bookmakers);
    }

    [Fact]
    public async Task AddBookmaker_WithInvalidLogoUrl_SetsValidationError()
    {
        // Arrange
        _viewModel.NewBookmakerName = "ValidName";
        _viewModel.NewBookmakerLogoUrl = "not a url";

        // Act
        await _viewModel.AddBookmakerCommand.ExecuteAsync(null);

        // Assert
        Assert.NotNull(_viewModel.LogoUrlError);
        Assert.Contains("valid URL", _viewModel.LogoUrlError);
        Assert.Empty(_viewModel.Bookmakers);
    }

    [Fact]
    public void ConfirmDelete_ShowsDialogWithMessage()
    {
        // Arrange
        var bookmaker = new Bookmaker
        {
            Id = 1,
            Nom = "Bet365",
            UrlLogo = "https://example.com/logo.png",
            Actif = true,
            DateActivation = DateTime.Now
        };
        var bookmakerViewModel = new BookmakerItemViewModel(bookmaker);

        // Act
        _viewModel.ConfirmDeleteCommand.Execute(bookmakerViewModel);

        // Assert
        Assert.True(_viewModel.ShowDeleteConfirm);
        Assert.Contains("Bet365", _viewModel.ConfirmationMessage);
        Assert.Equal(bookmakerViewModel, _viewModel.SelectedBookmakerForDelete);
    }

    [Fact]
    public async Task ExecuteDelete_WithValidBookmaker_RemovesFromCollection()
    {
        // Arrange
        var bookmaker = new Bookmaker
        {
            Id = 1,
            Nom = "Bet365",
            UrlLogo = "https://example.com/logo.png",
            Actif = true,
            DateActivation = DateTime.Now
        };

        var bookmakerViewModel = new BookmakerItemViewModel(bookmaker);
        _viewModel.Bookmakers.Add(bookmakerViewModel);

        _mockBookmakerRepository
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        _mockBookmakerRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Set up confirmation state
        _viewModel.SelectedBookmakerForDelete = bookmakerViewModel;
        _viewModel.ShowDeleteConfirm = true;

        // Act
        await _viewModel.ExecuteDeleteCommand.ExecuteAsync(null);

        // Assert
        Assert.Empty(_viewModel.Bookmakers);
        Assert.Null(_viewModel.ErrorMessage);
        Assert.False(_viewModel.ShowDeleteConfirm);
    }

    [Fact]
    public void CancelDelete_HidesDialog()
    {
        // Arrange
        _viewModel.ShowDeleteConfirm = true;
        _viewModel.ConfirmationMessage = "Test message";

        // Act
        _viewModel.CancelDeleteCommand.Execute(null);

        // Assert
        Assert.False(_viewModel.ShowDeleteConfirm);
        Assert.Null(_viewModel.SelectedBookmakerForDelete);
        Assert.Null(_viewModel.ConfirmationMessage);
    }

    [Fact]
    public void ClearError_ClearsErrorMessage()
    {
        // Arrange
        _viewModel.ErrorMessage = "Some error message";

        // Act
        _viewModel.ClearErrorCommand.Execute(null);

        // Assert
        Assert.Null(_viewModel.ErrorMessage);
    }
}
