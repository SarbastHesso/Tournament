using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using Tournament.Shared.Dto;
using Tournament.Shared.Request;

namespace Tournament.Client.Clients;

public class TournamentDetailsClient : ITournamentDetailsClient
{
    private readonly HttpClient client;
    private const string json = "application/json";

    public TournamentDetailsClient(HttpClient client)
    {
        this.client = client;
        client.BaseAddress = new Uri("https://localhost:7054");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
        client.Timeout = TimeSpan.FromSeconds(5);
    }

    public async Task<T> GetAsync<T>(string path, string contentType = json)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return result!;
    }

    public async Task<TournamentDetailsDto> GetByIdAsync(int id, bool includeGames)
    {
        var path = $"api/tournaments/{id}?includeGames={includeGames.ToString().ToLower()}";
        return await GetAsync<TournamentDetailsDto>(path);
    }

    public async Task<TournamentDetailsDto> CreateTournamentAsync(TournamentDetailsCreateDto tournamentToCreate)
    {
        var jsonTournament = JsonSerializer.Serialize(tournamentToCreate);
        var request = new HttpRequestMessage(HttpMethod.Post, "api/tournaments")
        {
            Content = new StringContent(jsonTournament, Encoding.UTF8, json)
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var res = await response.Content.ReadAsStringAsync();
        var createdTournament = JsonSerializer.Deserialize<TournamentDetailsDto>(res, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return createdTournament!;
    }

    public async Task UpdateTournamentAsync(int id, TournamentDetailsUpdateDto updateDto)
    {
        var jsonUpdate = JsonSerializer.Serialize(updateDto);
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/tournaments/{id}")
        {
            Content = new StringContent(jsonUpdate, Encoding.UTF8, json)
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task PatchTournamentTitleAsync(int id, string newTitle)
    {
        var patchDocument = new JsonPatchDocument<TournamentDetailsUpdateDto>();
        patchDocument.Replace(t => t.Title, newTitle);

        var serializedPatch = Newtonsoft.Json.JsonConvert.SerializeObject(patchDocument);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/tournaments/{id}")
        {
            Content = new StringContent(serializedPatch, Encoding.UTF8, json)
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteTournamentAsync(int id)
    {
        var response = await client.DeleteAsync($"api/tournaments/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<TournamentDetailsDto>> SearchTournamentsAsync(string? title, DateTime? date, bool includeGames = false)
    {
        var query = $"api/tournaments/search?includeGames={includeGames.ToString().ToLower()}";

        if (!string.IsNullOrWhiteSpace(title))
            query += $"&title={Uri.EscapeDataString(title)}";

        if (date.HasValue)
            query += $"&date={date.Value:yyyy-MM-dd}";

        var paged = await GetAsync<PagedResult<TournamentDetailsDto>>(query);
        return paged.Items;
    }
}
