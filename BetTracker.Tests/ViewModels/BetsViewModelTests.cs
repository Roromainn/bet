using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using BetTracker.Core.Models;
using BetTracker.Core.Services;
using BetTracker.Data.Repositories;
using BetTracker.UI.ViewModels;

namespace BetTracker.Tests.ViewModels;

/// <summary>
/// Unit tests for BetsViewModel using mocked repositories and services.
/// Tests async operations and error handling.
/// </summary>
public class BetsViewModelTests
{
    private readonly Mock<IBetRepository> _mockBetRepository;
    private readonly Mock<IBetCalculationService> _mockCalculationService;
    private readonly BetsViewModel _viewModel;

    public BetsViewModelTests()
    {
        _mockBetRepository = new Mock<IBetRepository>();
        _mockCalculationService = new Mock<IBetCalculationService>();
        _viewModel = new BetsViewModel(_mockBetRepository.Object, _mockCalculationService.Object);
    }

    [Fact]
    public async Task LoadBets_WithValidBets_PopulatesCollection()
    {
        // Arrange
        var activeBets = new List<Offre>
        {
            new Offre
            {
                Id = 1,
                Type = TypeOffre.FreeBet,
                Statut = StatutOffre.AFaire,
                BookmakerId = 1,
                Bookmaker = new Bookmaker { Id = 1, Nom = "Betclic", UrlLogo = "", Actif = true, DateActivation = DateTime.Now }
            },
            new Offre
            {
                Id = 2,
                Type = TypeOffre.RemboursementFreeBet,
                Statut = StatutOffre.AFaire,
                BookmakerId = 2,
                Bookmaker = new Bookmaker { Id = 2, Nom = "Winamax", UrlLogo = "", Actif = true, DateActivation = DateTime.Now }
            }
        };

        _mockBetRepository
            .Setup(r => r.GetActiveBetsAsync())
            .ReturnsAsync(activeBets);

        // Act
        await _viewModel.LoadBetsCommand.ExecuteAsync(null);

        // Assert
        Assert.Equal(2, _viewModel.Bets.Count);
        Assert.False(_viewModel.IsLoading);
        Assert.Null(_viewModel.ErrorMessage);
    }

    [Fact]
    public async Task LoadBets_WithException_SetsErrorMessage()
    {
        // Arrange
        _mockBetRepository
            .Setup(r => r.GetActiveBetsAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        await _viewModel.LoadBetsCommand.ExecuteAsync(null);

        // Assert
        Assert.NotNull(_viewModel.ErrorMessage);
        Assert.Contains("Error loading bets", _viewModel.ErrorMessage);
        Assert.False(_viewModel.IsLoading);
    }

    [Fact]
    public async Task DeleteBet_WithValidId_RemovesFromCollection()
    {
        // Arrange
        var bet = new Offre
        {
            Id = 1,
            Type = TypeOffre.FreeBet,
            Statut = StatutOffre.AFaire,
            BookmakerId = 1,
            Bookmaker = new Bookmaker { Id = 1, Nom = "Betclic", UrlLogo = "", Actif = true, DateActivation = DateTime.Now }
        };

        _viewModel.Bets.Add(new BetItemViewModel(bet, _mockCalculationService.Object));

        _mockBetRepository
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        _mockBetRepository
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _viewModel.DeleteBetCommand.ExecuteAsync(1);

        // Assert
        Assert.Empty(_viewModel.Bets);
        Assert.Null(_viewModel.ErrorMessage);
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
