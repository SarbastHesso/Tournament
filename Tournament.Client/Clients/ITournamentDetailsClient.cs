
namespace Tournament.Client.Clients
{
    public interface ITournamentDetailsClient
    {
        Task<T> GetAsync<T>(string path, string contentType = "application/json");
    }
}