using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Request;

namespace Tournament.Core.Interfaces;

public interface ITournamentDetailsRepository
{
    Task<PagedResult<TournamentDetails>> GetAllAsync(PagedRequest request, bool includeGames, bool trackChanges);
    Task<TournamentDetails?> GetByIdAsync(int id, bool includeGames, bool trackChanges);
    void Create(TournamentDetails tournamentDetails);
    void Delete(TournamentDetails tournamentDetails);
    Task<PagedResult<TournamentDetails>> SearchAsync(PagedRequest request, string? title, DateTime? date, bool includeGames, bool trackChanges);
}
