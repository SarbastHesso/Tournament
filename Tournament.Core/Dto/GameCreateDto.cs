using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record GameCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; init; }

        [Required]
        public DateTime Time { get; init; }

        [Required]
        public int TournamentId { get; init; }

    }
}
