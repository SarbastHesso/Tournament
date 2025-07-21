using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Tournament.Core.Request;
using Tournament.Shared.Dto;



namespace Service.Contracts;

public interface ITournamentDetailsService
{
    Task<PagedResult<TournamentDetailsDto>> GetAllAsync(PagedRequest request, bool includeGames, bool trackChanges);
    Task<TournamentDetailsDto> GetByIdAsync(int id , bool includeGames, bool trackChanges);
    Task<TournamentDetailsDto> CreateAsync(TournamentDetailsCreateDto createDto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, TournamentDetailsUpdateDto updatedDto);
    Task PatchAsync(int id, JsonPatchDocument<TournamentDetailsUpdateDto> patchDoc);
    Task<PagedResult<TournamentDetailsDto>> SearchAsync(PagedRequest request, string? title, DateTime? date, bool includeGames, bool trackChanges);


}
