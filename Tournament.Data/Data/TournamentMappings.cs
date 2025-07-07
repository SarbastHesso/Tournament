using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public class TournamentMappings: Profile
{
    public TournamentMappings() 
    {
        CreateMap<TournamentDetails, TournamentDetailsDto>()
            .ForMember(dest => dest.EndDate, opts => opts
            .MapFrom(src => src.StartDate.AddMonths(3)));
        CreateMap<TournamentDetailsCreateDto, TournamentDetails>();
        CreateMap<TournamentDetailsUpdateDto, TournamentDetails>()
            .ForAllMembers(opts => opts
            .Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<TournamentDetails, TournamentDetailsUpdateDto>();
        CreateMap<Game, GameDto>();
        CreateMap<GameCreateDto, Game>();
        CreateMap<GameUpdateDto, Game>().ReverseMap();
    }
}
