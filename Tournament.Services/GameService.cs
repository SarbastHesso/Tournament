using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Interfaces;

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

    public async Task<PagedResult<GameDto>> GetAllAsync(int? tournamenId, bool trackChanges= false, int page = 1, int pageSize = 10)
    {
        if (pageSize > 100)
        {
            pageSize= 100;
        }

        var games = await _unitOfWork.GameRepository.GetAllAsync(tournamenId, trackChanges);

        var totalItems = games.Count();

        var pagedGames = games
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var dtos = _mapper.Map<IEnumerable<GameDto>>(pagedGames);

        return new PagedResult<GameDto>
        {
            Items = dtos,
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = page
        };
    }

    public async Task<GameDto> GetByIdAsync(int id, bool trackChanges=false)
    {
        var game = await _unitOfWork.GameRepository.GetByIdAsync(id, trackChanges);

        if (game == null) throw new KeyNotFoundException($"Game with id {id} not found");

        var dto = _mapper.Map<GameDto>(game);

        return dto;
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

    public async Task<PagedResult<GameDto>> SearchAsync(string? title, DateTime? date, bool trackChanges = false, int page = 1, int pageSize = 10)
    {
        if (pageSize > 100)
        {
            pageSize = 100;
        }
        var tournaments = await _unitOfWork.GameRepository.SearchAsync(title, date, trackChanges);

        var totalItems = tournaments.Count();

        var pagedTournaments = tournaments
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var dtos = _mapper.Map<IEnumerable<GameDto>>(pagedTournaments);

        return new PagedResult<GameDto>
        {
            Items = dtos,
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = page
        };

    }
}
