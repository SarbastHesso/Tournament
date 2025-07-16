using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Tournament.Client.Clients;
using Tournament.Client.Models;
using Tournament.Shared.Dto;

namespace Tournament.Client.Controllers;

public class HomeController : Controller
{
    private HttpClient httpClient;
    private const string json = "application/json";
    private readonly ITournamentDetailsClient tournamentDetailsClient;

    public HomeController(ITournamentDetailsClient tournamentDetailsClient)
    {
        //httpClient = new HttpClient();
        //httpClient.BaseAddress = new Uri("https://localhost:7054");
        this.tournamentDetailsClient = tournamentDetailsClient;
    }

    public async Task<IActionResult> Index()
    {
        //var result = await SimpleGetAsync();
        var getAll = await GetWithRequestMessageAsync();
        var create = await PostWithRequestMessageAsync();
        await PatchWithRequestMessageAsync();

        return View();
    }

    //private async Task<IEnumerable<TournamentDetailsDto>> SimpleGetAsync()
    //{
    //    var response = await httpClient.GetAsync("api/tournaments");
    //    response.EnsureSuccessStatusCode();

    //    var res = await response.Content.ReadAsStringAsync();
    //    var tournaments = JsonSerializer.Deserialize<IEnumerable<TournamentDetailsDto>>(res, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

    //    return tournaments!;
    //}

    private async Task<IEnumerable<TournamentDetailsDto>> GetWithRequestMessageAsync()
    {
        //var request = new HttpRequestMessage(HttpMethod.Get, "api/tournaments");
        //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

        //var response = await httpClient.SendAsync(request);
        //response.EnsureSuccessStatusCode();

        //var res = await response.Content.ReadAsStringAsync();
        //var result = JsonSerializer.Deserialize<IEnumerable<TournamentDetailsDto>>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        //return result;

        var tournamentDetailsDto = await tournamentDetailsClient.GetAsync<IEnumerable<TournamentDetailsDto>>("api/tournaments");
        return tournamentDetailsDto;
    }

    private async Task<TournamentDetailsDto> PostWithRequestMessageAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/tournaments");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

        var tournamentDetailsToCreate = new TournamentDetailsCreateDto()
        {
            //Title = "Axelsson Cup",
            //StartDate = DateTime.Today,
        };

        if (!string.IsNullOrWhiteSpace(tournamentDetailsToCreate.Title) && tournamentDetailsToCreate.StartDate != null)
        {
            var jsonTournament = JsonSerializer.Serialize(tournamentDetailsToCreate);

            request.Content = new StringContent(jsonTournament);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(json);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadAsStringAsync();
            var createdTournament = JsonSerializer.Deserialize<TournamentDetailsDto>(res, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var location = response.Headers.Location;

            return createdTournament;
        } else
        {
            return null;
        }
       
    }

    private async Task PatchWithRequestMessageAsync()
    {
        var patchDocument = new JsonPatchDocument<TournamentDetailsUpdateDto>();
        patchDocument.Replace(t => t.Title, "Aryo");

        var serializedPatchDocument = Newtonsoft.Json.JsonConvert.SerializeObject(patchDocument);

        var request = new HttpRequestMessage(HttpMethod.Patch, "api/tournaments/30");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

        request.Content = new StringContent(serializedPatchDocument);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(json);

        var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

    }
}
