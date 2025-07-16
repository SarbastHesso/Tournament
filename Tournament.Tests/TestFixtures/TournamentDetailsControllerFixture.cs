using Moq;
using Service.Contracts;
using System;
using Tournament.Api.Controllers;

namespace Tournament.Tests.TestFixtures;

public class TournamentDetailsControllerFixture : IDisposable
{
    public Mock<IServiceManager> _serviceManagerMock { get; }
    public Mock<ITournamentDetailsService> _tournamentDetailsServiceMock { get; }

    public TournamentDetailsControllerFixture()
    {
        _tournamentDetailsServiceMock = new Mock<ITournamentDetailsService>();
        _serviceManagerMock = new Mock<IServiceManager>();

        _serviceManagerMock
            .Setup(sm => sm.TournamentDetailsService)
            .Returns(_tournamentDetailsServiceMock.Object);
    }

    public TournamentDetailsController CreateController()
    {
        return new TournamentDetailsController(_serviceManagerMock.Object);
    }

    public void Dispose()
    {
        // No resources to dispose yet
    }
}
