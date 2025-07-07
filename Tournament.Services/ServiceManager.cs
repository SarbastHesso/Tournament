using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services;

public class ServiceManager: IServiceManager
{
    private readonly Lazy<ITournamentDetailsService> tournamentDetailsService;
    private readonly Lazy<IGameService> gameService;

    public ITournamentDetailsService TournamentDetailsService => tournamentDetailsService.Value;
    public IGameService GameService => gameService.Value;
    public ServiceManager(Lazy<ITournamentDetailsService> tournamentDetailsservice, Lazy<IGameService> gameservice)
    {
        tournamentDetailsService = tournamentDetailsservice;
        gameService = gameservice;
    }
}
