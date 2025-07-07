using AutoMapper;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Services;

public class TournamentDetailsService: ITournamentDetailsService
{
    private IUnitOfWork _unitOfWork;
    private readonly IMapper  _mapper;
    public TournamentDetailsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;   
    }

    public async Task<PagedResult<TournamentDetailsDto>> GetAllAsync(bool includeGames = false, bool trackChanges = false, int page = 1, int pageSize = 10)
    {
        if ( pageSize > 100)
        {
            pageSize = 100;
        }

        var tournaments = await _unitOfWork.TournamentDetailsRepository.GetAllAsync(includeGames, trackChanges);

        var totalItems = tournaments.Count();

        var pagedTournaments = tournaments
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var dtos = _mapper.Map<IEnumerable<TournamentDetailsDto>>(pagedTournaments);

        return new PagedResult<TournamentDetailsDto>
        {
            Items = dtos,
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = page
        };
    }

    public async Task<TournamentDetailsDto> GetByIdAsync(int id, bool includeGames = false, bool trackChanges = false)
    {
        var tournament = await _unitOfWork.TournamentDetailsRepository.GetByIdAsync(id, includeGames, trackChanges);
        if (tournament == null)
            throw new KeyNotFoundException($"Tournament with id {id} not found");

        var dto = _mapper.Map<TournamentDetailsDto>(tournament);
        return dto;
    }

    public async Task<TournamentDetailsDto> CreateAsync(TournamentDetailsCreateDto createDto)
    {
        var tournament = _mapper.Map<TournamentDetails>(createDto);
        _unitOfWork.TournamentDetailsRepository.Create(tournament);
        await _unitOfWork.CompleteAsync();
        var tournamentDto = _mapper.Map<TournamentDetailsDto>(tournament);
        return tournamentDto;
    }

    public async Task DeleteAsync(int id)
    {
        var existingTournament = await _unitOfWork.TournamentDetailsRepository.GetByIdAsync(id, includeGames: false, trackChanges: false);
        if (existingTournament == null)
            throw new KeyNotFoundException($"Tournament with id {id} not found");

        _unitOfWork.TournamentDetailsRepository.Delete(existingTournament);
        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateAsync(int id, TournamentDetailsUpdateDto updateDto)
    {
        var existingTournament = await _unitOfWork.TournamentDetailsRepository.GetByIdAsync(id, includeGames: false, trackChanges: true);
        if (existingTournament == null)
            throw new KeyNotFoundException($"Tournament with id {id} not found");

        _mapper.Map(updateDto, existingTournament);
        await _unitOfWork.CompleteAsync();
    }
   
    public async Task PatchAsync(int id, JsonPatchDocument<TournamentDetailsUpdateDto> patchDoc)
    {
        var entity = await _unitOfWork.TournamentDetailsRepository
            .GetByIdAsync(id,includeGames:false, trackChanges: true);

        if (entity == null)
            throw new KeyNotFoundException($"Tournament with id {id} not found");

        // Map the tracked entity to a DTO
        var dto = _mapper.Map<TournamentDetailsUpdateDto>(entity);

        // Apply the patch
        patchDoc.ApplyTo(dto);

        // Optional: validate DTO here if needed
        // You can expose a validation method or move this back to controller if necessary

        // Map patched DTO back to the tracked entity
        _mapper.Map(dto, entity);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<PagedResult<TournamentDetailsDto>> SearchAsync(string? title, DateTime? date, bool includeGames = false, bool trackChanges = false, int page = 1, int pageSize = 10)
    { 
        if (pageSize > 100)
        {
            pageSize = 100;
        }
        var tournaments = await _unitOfWork.TournamentDetailsRepository.SearchAsync(title, date, includeGames, trackChanges);

        var totalItems = tournaments.Count();

        var pagedTournaments = tournaments
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var dtos = _mapper.Map<IEnumerable<TournamentDetailsDto>>(pagedTournaments);

        return new PagedResult<TournamentDetailsDto>
        {
            Items = dtos,
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = page
        };

    }

}