using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record TournamentDetailsCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; init; } = null!;

        [Required]
        public DateTime StartDate { get; init; }
        public List<GameCreateDto>? Games { get; init; }
    }
}
