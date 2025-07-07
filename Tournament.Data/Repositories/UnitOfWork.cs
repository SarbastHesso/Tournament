using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Interfaces;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TournamentApiContext _context;
        public ITournamentDetailsRepository TournamentDetailsRepository { get; }
        public IGameRepository GameRepository { get; }

        public UnitOfWork(TournamentApiContext context)
        {
            _context = context;
            TournamentDetailsRepository = new TournamentDetailsRepository(context);
            GameRepository = new GameRepository(context);
        }

        async Task IUnitOfWork.CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
