using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Interfaces;

public interface ITournamentDetailsRepository
{
    Task<IEnumerable<TournamentDetails>> GetAllAsync(bool includeGames, bool trackChanges);
    Task<TournamentDetails?> GetByIdAsync(int id, bool includeGames, bool trackChanges);
    void Create(TournamentDetails tournamentDetails);
    void Delete(TournamentDetails tournamentDetails);
    Task<IEnumerable<TournamentDetails>> SearchAsync(string? title, DateTime? date, bool includeGames, bool trackChanges);
}
