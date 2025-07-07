using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Interfaces
{
    public interface IUnitOfWork
    {
        ITournamentDetailsRepository TournamentDetailsRepository { get; }
        IGameRepository GameRepository { get; }

        Task CompleteAsync();
    }
}
