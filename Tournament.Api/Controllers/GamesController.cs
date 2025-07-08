using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Tournament.Core.Request;
using Tournament.Shared.Dto;



[Route("api/tournaments/{tournamentId}/games")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public GameController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GameDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(
        [FromRoute] int tournamentId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
        )
    {
        var request = new PagedRequest {Page = page, PageSize = pageSize };
        var pagedResult = await _serviceManager.GameService.GetAllAsync(request, tournamentId, trackChanges:false, page, pageSize);
        if (!pagedResult.Items.Any())
        {
            return NotFound();
        }
        return Ok(pagedResult);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameDto>> GetGame(int id)
    {
        var gameDto = await _serviceManager.GameService.GetByIdAsync(id, trackChanges: false);
        if (gameDto == null)
            return NotFound();

        return Ok(gameDto);
    }


    [HttpPost]
    public async Task<ActionResult<GameDto>> PostGame([FromRoute] int tournamentId, [FromBody] GameCreateDto gameCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var newGame = new GameCreateDto
        {
            Title = gameCreateDto.Title,
            Time = gameCreateDto.Time,
            TournamentId = tournamentId
        };
        var gameDto = await _serviceManager.GameService.CreateAsync(newGame);
        return CreatedAtAction(nameof(GetGame), new {tournamentId= gameDto.TournamentId, id = gameDto.Id }, gameDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutGame(int id, [FromBody] GameUpdateDto gameUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _serviceManager.GameService.UpdateAsync(id, gameUpdateDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGame(int id)
    {
        try
        {
            await _serviceManager.GameService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PatchGame(int id, [FromBody] JsonPatchDocument<GameUpdateDto> patchDoc)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (patchDoc == null)
            return BadRequest();

        try
        {
            await _serviceManager.GameService.PatchAsync(id, patchDoc);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GameDto>>> SearchTournamentsByTitle(
            [FromQuery] string? title,
            [FromQuery] DateTime? date,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
            )
    {
        if (string.IsNullOrWhiteSpace(title) && !date.HasValue)
            return BadRequest("At least one filter (title or date) must be provided.");

        var request = new PagedRequest { Page = page, PageSize = pageSize };

        var pagedResult = await _serviceManager.GameService.SearchAsync(request, title, date, trackChanges: false);

        if (!pagedResult.Items.Any())
        {
            return NotFound();
        }

        return Ok(pagedResult);
    }
}
