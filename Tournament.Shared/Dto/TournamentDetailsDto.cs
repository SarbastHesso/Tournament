﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tournament.Shared.Dto;

public record TournamentDetailsDto
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public IEnumerable<GameDto>? Games { get; init; }

}
