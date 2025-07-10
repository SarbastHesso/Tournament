using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Interfaces;
using Tournament.Core.Request;
using Tournament.Core.Responses;
using Tournament.Shared.Dto;


namespace Tournament.Services;

public class GameService: IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GameService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<GameDto>> GetAllAsync(PagedRequest request, int? tournamenId, bool trackChanges= false, int page = 1, int pageSize = 10)
    {
        if (request.PageSize > 100)
        {
            request.PageSize = 100;
        }

        var pagedGames = await _unitOfWork.GameRepository.GetAllAsync(request, tournamenId, trackChanges);

        var itemsDto = _mapper.Map<IEnumerable<GameDto>>(pagedGames.Items);

        return new PagedResult<GameDto>
        {
            Items = itemsDto,
            TotalItems = pagedGames.TotalItems,
            PageSize = pagedGames.PageSize,
            CurrentPage = pagedGames.CurrentPage
        };
    }

    public async Task<ApiBaseResponse> GetByIdAsync(int id, bool trackChanges=false)
    {
        var game = await _unitOfWork.GameRepository.GetByIdAsync(id, trackChanges);

        //if (game == null) throw new KeyNotFoundException($"Game with id {id} not found");
        if (game == null)
        {
            return new GameNotFoundResponse(id);
        }

        var dto = _mapper.Map<GameDto>(game);

        return new ApiOkResponse<GameDto>(dto);
    }

    async Task<GameDto> IGameService.CreateAsync(GameCreateDto createDto)
    {
        var game = _mapper.Map<Game>(createDto);
        _unitOfWork.GameRepository.Create(game);
        await _unitOfWork.CompleteAsync();
        var gameDto = _mapper.Map<GameDto>(game);
        return gameDto;
    }

    async Task IGameService.DeleteAsync(int id)
    {
        var existingGame = await _unitOfWork.GameRepository.GetByIdAsync(id, trackChanges:false);
        if (existingGame == null) throw new KeyNotFoundException($"Game with id {id} not found");
        _unitOfWork.GameRepository.Delete(existingGame);
        await _unitOfWork.CompleteAsync();
    }

    async Task IGameService.UpdateAsync(int id, GameUpdateDto updateDto)
    {
        var existingGame = await _unitOfWork.GameRepository.GetByIdAsync(id, trackChanges:true);
        if (existingGame == null) throw new KeyNotFoundException($"Game with id {id} not found");

        _mapper.Map(updateDto, existingGame);   
        await _unitOfWork.CompleteAsync();
        
    }

    public async Task PatchAsync(int id, JsonPatchDocument<GameUpdateDto> patchDoc)
    {
        var entity = await _unitOfWork.GameRepository
            .GetByIdAsync(id, trackChanges: true);

        if (entity == null)
            throw new KeyNotFoundException($"Game with id {id} not found");

        // Map the tracked entity to a DTO
        var dto = _mapper.Map<GameUpdateDto>(entity);

        // Apply the patch
        patchDoc.ApplyTo(dto);

        // Optional: validate DTO here if needed
        // You can expose a validation method or move this back to controller if necessary

        // Map patched DTO back to the tracked entity
        _mapper.Map(dto, entity);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<PagedResult<GameDto>> SearchAsync(PagedRequest request, string? title, DateTime? date, bool trackChanges = false, int page = 1, int pageSize = 10)
    {
        if (request.PageSize > 100)
        {
            request.PageSize = 100;
        }
        var pagedGames = await _unitOfWork.GameRepository.SearchAsync(request, title, date, trackChanges);

        var itemsDto = _mapper.Map<IEnumerable<GameDto>>(pagedGames.Items);

        return new PagedResult<GameDto>
        {
            Items = itemsDto,
            TotalItems = pagedGames.TotalItems,
            PageSize = pagedGames.PageSize,
            CurrentPage = pagedGames.CurrentPage
        };

    }
}
