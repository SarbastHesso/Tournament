
using Tournament.Shared.Dto;

namespace Tournament.Client.Clients
{
    public interface ITournamentDetailsClient
    {
        Task<T> GetAsync<T>(string path, string contentType = "application/json");
        Task<TournamentDetailsDto> GetByIdAsync(int id, bool includeGames);
        Task<TournamentDetailsDto> CreateTournamentAsync(TournamentDetailsCreateDto tournamentToCreate);
        Task UpdateTournamentAsync(int id, TournamentDetailsUpdateDto updateDto);
        Task PatchTournamentTitleAsync(int id, string newTitle);
        Task DeleteTournamentAsync(int id);
        Task<IEnumerable<TournamentDetailsDto>> SearchTournamentsAsync(string? title, DateTime? date, bool includeGames = false);
    }

}