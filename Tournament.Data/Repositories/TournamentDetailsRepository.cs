using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Interfaces;
using Tournament.Core.Request;
using Tournament.Data.Data;


namespace Tournament.Data.Repositories;

public class TournamentDetailsRepository : RepositoryBase<TournamentDetails>, ITournamentDetailsRepository
{

    public TournamentDetailsRepository(TournamentApiContext context): base(context)
    {
    }

    public async Task<PagedResult<TournamentDetails>> GetAllAsync(PagedRequest request, bool includeGames=false, bool trackChanges = false)
    {

        var query = FindAll(trackChanges);
        if (includeGames)
        {
            query = query.Include(t => t.Games);
        }
        return await GetPagedAsync(query, request.Page, request.PageSize);

    }

    public async Task<TournamentDetails?> GetByIdAsync(int id, bool includeGames, bool trackChanges = false)
    {
        return includeGames
            ? await FindByCondition(t => t.Id.Equals(id), trackChanges).Include(t => t.Games).FirstOrDefaultAsync()
            : await FindByCondition(t => t.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
    }

    public async Task<PagedResult<TournamentDetails>> SearchAsync(PagedRequest request, string? title, DateTime? date, bool includeGames = false, bool trackChanges = false)
    {

        var query = FindAll(trackChanges);

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(t => t.Title.ToLower().Contains(title.ToLower()));
        }

        if (date.HasValue)
        {
            query = query.Where(t => t.StartDate.Date == date.Value.Date);
        }

        if (includeGames)
        {
            query = query.Include(t => t.Games);
        }

        return await GetPagedAsync(query, request.Page, request.PageSize);
    }

}

