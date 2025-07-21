using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Tournament.Client.Clients;
using Tournament.Client.Models;
using Tournament.Shared.Dto;
using Tournament.Shared.Request;

namespace Tournament.Client.Controllers;

public class HomeController : Controller
{
    private readonly ITournamentDetailsClient tournamentDetailsClient;

    public HomeController(ITournamentDetailsClient tournamentDetailsClient)
    {
        this.tournamentDetailsClient = tournamentDetailsClient;
    }

    public async Task<IActionResult> Index()
    {
        var pagedTournaments = await tournamentDetailsClient.GetAsync<PagedResult<TournamentDetailsDto>>("api/tournaments");
        var allTournaments = pagedTournaments.Items;

        // Create a new tournament
        var created = await tournamentDetailsClient.CreateTournamentAsync(new TournamentDetailsCreateDto
        {
            Title = "Axelsson Cup",
            StartDate = DateTime.Today
        });

        // Get by ID
        var fetched = await tournamentDetailsClient.GetByIdAsync(created.Id, includeGames: false);

        // Update tournament
        await tournamentDetailsClient.UpdateTournamentAsync(fetched.Id, new TournamentDetailsUpdateDto
        {
            Title = "Updated Axelsson Cup",
            StartDate = fetched.StartDate
        });

        // Patch title
        await tournamentDetailsClient.PatchTournamentTitleAsync(fetched.Id, "Patched Axelsson Cup");

        // Search tournaments
        var searchResult = await tournamentDetailsClient.SearchTournamentsAsync("Axelsson", null);

        // Delete tournament
        await tournamentDetailsClient.DeleteTournamentAsync(fetched.Id);

        return View(allTournaments); // or pass searchResult or any other list to the view
    }
    
}
