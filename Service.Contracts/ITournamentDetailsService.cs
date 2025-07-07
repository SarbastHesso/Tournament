using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Service.Contracts;

public interface ITournamentDetailsService
{
    Task<PagedResult<TournamentDetailsDto>> GetAllAsync(bool includeGames, bool trackChanges, int page = 1, int pageSize = 10);
    Task<TournamentDetailsDto> GetByIdAsync(int id , bool includeGames, bool trackChanges);
    Task<TournamentDetailsDto> CreateAsync(TournamentDetailsCreateDto createDto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, TournamentDetailsUpdateDto updatedDto);
    Task PatchAsync(int id, JsonPatchDocument<TournamentDetailsUpdateDto> patchDoc);
    Task<PagedResult<TournamentDetailsDto>> SearchAsync(string? title, DateTime? date, bool includeGames, bool trackChanges, int page = 1, int pageSize = 10);


}
