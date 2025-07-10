using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Request;
using Tournament.Core.Responses;
using Tournament.Shared.Dto;


namespace Service.Contracts;

public interface IGameService
{
    Task<PagedResult<GameDto>> GetAllAsync(PagedRequest request, int? tournamenId, bool trackChanges, int page = 1, int pageSize = 10);
    Task<ApiBaseResponse> GetByIdAsync(int id, bool trackChanges);
    Task<GameDto> CreateAsync(GameCreateDto createDto);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, GameUpdateDto updateDto);
    Task PatchAsync(int id, JsonPatchDocument<GameUpdateDto> patchDoc);
    Task<PagedResult<GameDto>> SearchAsync(PagedRequest request, string? title, DateTime? date, bool trackChanges, int page = 1, int pageSize = 10);
}
