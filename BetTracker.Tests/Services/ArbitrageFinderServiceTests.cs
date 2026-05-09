using BetTracker.Core.Models;
using BetTracker.Core.Services;
using Moq;

namespace BetTracker.Tests.Services;

/// <summary>
/// Unit tests for ArbitrageFinderService.
/// </summary>
public class ArbitrageFinderServiceTests
{
    private readonly Mock<IBetOfferRepository> _mockBetRepository;
    private readonly Mock<IBetCalculationService> _mockCalculationService;
    private readonly IArbitrageFinderService _service;

    public ArbitrageFinderServiceTests()
    {
        _mockBetRepository = new Mock<IBetOfferRepository>();
        _mockCalculationService = new Mock<IBetCalculationService>();
        _service = new ArbitrageFinderService(_mockBetRepository.Object, _mockCalculationService.Object);
    }

    [Fact]
    public async Task FindArbitrageOpportunitiesAsync_WithProfitablePairs_ReturnsOpportunities()
    {
        // Arrange
        var bets = new List<Offre>
        {
            new Offre
            {
                Id = 1,
                BookmakerId = 1,
                Type = TypeOffre.FreeBet,
                MontantBonus = 100m,
                CoteMinimum = 1.5m,
                Statut = StatutOffre.AFaire,
                DateCreation = DateTime.Now
            },
            new Offre
            {
                Id = 2,
                BookmakerId = 2,
                Type = TypeOffre.FreeBet,
                MontantBonus = 100m,
                CoteMinimum = 1.6m,
                Statut = StatutOffre.AFaire,
                DateCreation = DateTime.Now
            }
        };

        _mockBetRepository.Setup(r => r.GetActiveBetsAsync()).ReturnsAsync(bets);
        _mockCalculationService
            .Setup(s => s.CalculateLayBetEV(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(50m); // Profitable

        // Act
        var opportunities = await _service.FindArbitrageOpportunitiesAsync(minProfitThreshold: 40m);

        // Assert
        Assert.NotEmpty(opportunities);
        Assert.Single(opportunities);
        var opp = opportunities.First();
        Assert.Equal(1, opp.Bookmaker1Id);
        Assert.Equal(2, opp.Bookmaker2Id);
        Assert.Equal(50m, opp.ProfitMargin);
    }

    [Fact]
    public async Task FindArbitrageOpportunitiesAsync_WithUnprofitablePairs_ReturnsEmpty()
    {
        // Arrange
        var bets = new List<Offre>
        {
            new Offre
            {
                Id = 1,
                BookmakerId = 1,
                Type = TypeOffre.FreeBet,
                MontantBonus = 100m,
                CoteMinimum = 1.5m,
                Statut = StatutOffre.AFaire,
                DateCreation = DateTime.Now
            },
            new Offre
            {
                Id = 2,
                BookmakerId = 2,
                Type = TypeOffre.FreeBet,
                MontantBonus = 100m,
                CoteMinimum = 1.6m,
                Statut = StatutOffre.AFaire,
                DateCreation = DateTime.Now
            }
        };

        _mockBetRepository.Setup(r => r.GetActiveBetsAsync()).ReturnsAsync(bets);
        _mockCalculationService
            .Setup(s => s.CalculateLayBetEV(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(10m); // Not profitable

        // Act
        var opportunities = await _service.FindArbitrageOpportunitiesAsync(minProfitThreshold: 40m);

        // Assert
        Assert.Empty(opportunities);
    }

    [Fact]
    public async Task FindArbitrageOpportunitiesAsync_IgnoresSameBoomakerPairs()
    {
        // Arrange
        var bets = new List<Offre>
        {
            new Offre
            {
                Id = 1,
                BookmakerId = 1,
                Type = TypeOffre.FreeBet,
                MontantBonus = 100m,
                CoteMinimum = 1.5m,
                Statut = StatutOffre.AFaire,
                DateCreation = DateTime.Now
            },
            new Offre
            {
                Id = 2,
                BookmakerId = 1, // Same bookmaker
                Type = TypeOffre.FreeBet,
                MontantBonus = 100m,
                CoteMinimum = 1.6m,
                Statut = StatutOffre.AFaire,
                DateCreation = DateTime.Now
            }
        };

        _mockBetRepository.Setup(r => r.GetActiveBetsAsync()).ReturnsAsync(bets);

        // Act
        var opportunities = await _service.FindArbitrageOpportunitiesAsync(minProfitThreshold: 40m);

        // Assert
        Assert.Empty(opportunities);
        _mockCalculationService.Verify(
            s => s.CalculateLayBetEV(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()),
            Times.Never,
            "Should not calculate EV for same bookmaker pairs"
        );
    }

    [Fact]
    public async Task FindArbitrageOpportunitiesAsync_WithNullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ArbitrageFinderService(null!, _mockCalculationService.Object));
    }

    [Fact]
    public async Task FindArbitrageOpportunitiesAsync_WithNullCalculationService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ArbitrageFinderService(_mockBetRepository.Object, null!));
    }
}
