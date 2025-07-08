using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Request;

namespace Tournament.Core.Interfaces;

public interface IGameRepository
{
    Task<PagedResult<Game>> GetAllAsync(PagedRequest request, int? tournamentId, bool trackChanges=false);
    Task<Game?> GetByIdAsync(int id, bool trackChanges);
    void Create(Game game);
    void Delete(Game game);
    Task<PagedResult<Game>> SearchAsync(PagedRequest request, string? title, DateTime? date, bool trackChanges = false);
}
