﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record GameDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public DateTime Time { get; init; }
        public int TournamentId { get; init; }
    }
}
