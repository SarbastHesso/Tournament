using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Interfaces;
using Tournament.Core.Request;
using Tournament.Data.Data;
using Tournament.Data.Repositories;

public class GameRepository : RepositoryBase<Game>, IGameRepository
{

    public GameRepository(TournamentApiContext context) : base(context)
    {

    }

    public async Task<PagedResult<Game>> GetAllAsync(PagedRequest request, int? tournamentId, bool trackChanges = false)
    {
        var query = tournamentId.HasValue 
            ? FindAll(trackChanges).Where(t => t.TournamentId == tournamentId) 
            : FindAll(trackChanges);
        return await GetPagedAsync(query, request.Page, request.PageSize);
    }

    public async Task<Game?> GetByIdAsync(int id, bool trackChanges)
    {
        return await FindByCondition(g => g.Id.Equals(id), trackChanges).FirstOrDefaultAsync(); 
    }

    public async Task<PagedResult<Game>> SearchAsync(PagedRequest request, string? title, DateTime? date, bool trackChanges = false)
    {
        var query = FindAll(trackChanges);

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(t => t.Title.ToLower().Contains(title.ToLower()));
        }

        if (date.HasValue)
        {
            query = query.Where(t => t.Time.Date == date.Value.Date);
        }

        return await GetPagedAsync(query, request.Page, request.PageSize);
    }

}
