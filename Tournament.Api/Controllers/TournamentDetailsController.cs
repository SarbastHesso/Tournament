
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Tournament.Core.Request;
using Tournament.Shared.Dto;


namespace Tournament.Api.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TournamentDetailsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TournamentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<TournamentDetailsDto>>> GetAllTournamentDetails(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeGames = false
            )
        {
            var request = new PagedRequest { Page = page, PageSize = pageSize };
            var pagedResult = await _serviceManager.TournamentDetailsService.GetAllAsync(request, includeGames, trackChanges: false);
            if (!pagedResult.Items.Any())
            {
                return NotFound();
            }

            return Ok(pagedResult);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TournamentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TournamentDetailsDto>> GetTournamentDetails(int id, [FromQuery] bool includeGames)
        {

            var tournamentDto = await _serviceManager.TournamentDetailsService.GetByIdAsync(id, includeGames, trackChanges:false);

            if (tournamentDto == null)
            {
                return NotFound();
            }

            return Ok(tournamentDto);
        }


        // PUT: api/TournamentDetails/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]  
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]   
        public async Task<IActionResult> PutTournamentDetails(int id, [FromBody] TournamentDetailsUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _serviceManager.TournamentDetailsService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        // POST: api/TournamentDetails
        [HttpPost]
        [ProducesResponseType(typeof(TournamentDetailsDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostTournamentDetails([FromBody] TournamentDetailsCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tournamentDto = await _serviceManager.TournamentDetailsService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetTournamentDetails), new { id = tournamentDto.Id }, tournamentDto);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            try
            {
                await _serviceManager.TournamentDetailsService.DeleteAsync(id);
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
        public async Task<IActionResult> PatchTournamentDetails(int id, [FromBody] JsonPatchDocument<TournamentDetailsUpdateDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (patchDoc == null)
                return BadRequest();

            try
            {
                await _serviceManager.TournamentDetailsService.PatchAsync(id, patchDoc);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<TournamentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TournamentDetailsDto>>> SearchTournamentsByTitle(
            [FromQuery] string? title,
            [FromQuery] DateTime? date,
            [FromQuery] bool includeGames,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
            )
        {
            if (string.IsNullOrWhiteSpace(title) && !date.HasValue)
                return BadRequest("At least one filter (title or date) must be provided.");
            var request = new PagedRequest { Page = page, PageSize = pageSize };
            var pagedResult = await _serviceManager.TournamentDetailsService.SearchAsync(request, title, date, includeGames, trackChanges: false);

            if (pagedResult == null)
            {
                return NotFound();
            }

            return Ok(pagedResult);
        }
    }
}

