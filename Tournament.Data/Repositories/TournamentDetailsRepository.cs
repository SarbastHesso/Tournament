using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Interfaces;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class TournamentDetailsRepository : RepositoryBase<TournamentDetails>, ITournamentDetailsRepository
{

    public TournamentDetailsRepository(TournamentApiContext context): base(context)
    {
    }

    public async Task<IEnumerable<TournamentDetails>> GetAllAsync(bool includeGames=false, bool trackChanges = false)
    {
        return includeGames 
            ? await FindAll(trackChanges).Include(t => t.Games).ToListAsync() 
            : await FindAll(trackChanges).ToListAsync();
    }

    public async Task<TournamentDetails?> GetByIdAsync(int id, bool includeGames, bool trackChanges = false)
    {
        return includeGames
            ? await FindByCondition(t => t.Id.Equals(id), trackChanges).Include(t => t.Games).FirstOrDefaultAsync()
            : await FindByCondition(t => t.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TournamentDetails>> SearchAsync(string? title, DateTime? date, bool includeGames=false, bool trackChanges = false)
    {
        //return includeGames
        //    ? await FindByCondition(t => t.Title.ToLower().Contains(title.ToLower()), trackChanges).Include(t => t.Games).ToListAsync()
        //    : await FindByCondition(t => t.Title.ToLower().Contains(title.ToLower()), trackChanges).ToListAsync();
        var query = FindAll(trackChanges);

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(t =>  t.Title.ToLower().Contains(title.ToLower()));
        }

        if (date.HasValue)
        {
            query = query.Where(t => t.StartDate.Date == date.Value.Date);
        }

        if (includeGames)
        {
            query = query.Include(t => t.Games);
        }

        return await query.ToListAsync();
    }

}

