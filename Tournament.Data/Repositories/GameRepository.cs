using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Interfaces;
using Tournament.Data.Data;
using Tournament.Data.Repositories;

public class GameRepository : RepositoryBase<Game>, IGameRepository
{

    public GameRepository(TournamentApiContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Game>> GetAllAsync(int? tournamentId, bool trackChanges = false)
    {
        var tournamets = tournamentId.HasValue 
            ? FindAll(trackChanges).Where(t => t.TournamentId == tournamentId) 
            : FindAll(trackChanges);
        return await tournamets.ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id, bool trackChanges)
    {
        return await FindByCondition(g => g.Id.Equals(id), trackChanges).FirstOrDefaultAsync(); 
    }

    public async Task<IEnumerable<Game>> SearchAsync(string? title, DateTime? date, bool trackChanges = false)
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

        return await query.ToListAsync();
    }

}
