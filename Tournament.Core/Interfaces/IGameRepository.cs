using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Interfaces;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync(int? tournamentId, bool trackChanges=false);
    Task<Game?> GetByIdAsync(int id, bool trackChanges);
    void Create(Game game);
    void Delete(Game game);
    Task<IEnumerable<Game>> SearchAsync(string? title, DateTime? date, bool trackChanges = false);
}
