using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(TournamentApiContext context)
        {
            if (context.Tournaments.Any())
                return;

            var faker = new Faker();

            var tournaments = new List<TournamentDetails>();

            for (int i = 0; i < 25; i++)
            {
                var startDate = faker.Date.Future().Date;
                var endDate = startDate.AddMonths(3);

                var tournament = new TournamentDetails
                {
                    Title = faker.Company.CompanyName() + " Cup",
                    StartDate = startDate,
                    Games = new List<Game>()
                };

                for (int j = 0; j < faker.Random.Int(3, 7); j++)
                {
                    var gameTime = faker.Date.Between(startDate, endDate);

                    tournament.Games.Add(new Game
                    {
                        Title = $"Game {j + 1}",
                        Time = gameTime,
                        // TournamentId will be set by EF Core when saving
                    });
                }

                tournaments.Add(tournament);
            }

            context.Tournaments.AddRange(tournaments);
            await context.SaveChangesAsync();

            //var tournaments = new List<TournamentDetails>
            //{
            //    new TournamentDetails
            //    {
            //        Title = "Axelssons Cup 2025",
            //        StartDate = DateTime.UtcNow.Date,
            //        Games = new List<Game>
            //        {
            //            new Game { Title = "Quarterfinal 1", Time = DateTime.UtcNow.AddDays(1), TournamentId = 1 },
            //            new Game { Title = "Quarterfinal 2", Time = DateTime.UtcNow.AddDays(2), TournamentId = 1 }
            //        }
            //    },
            //    new TournamentDetails
            //    {
            //        Title = "Aros Cup 2025",
            //        StartDate = DateTime.UtcNow.Date.AddMonths(1),
            //        Games = new List<Game>
            //        {
            //            new Game { Title = "Semifinal 1", Time = DateTime.UtcNow.AddMonths(1).AddDays(1), TournamentId = 2 },
            //            new Game { Title = "Semifinal 2", Time = DateTime.UtcNow.AddMonths(1).AddDays(2), TournamentId = 2 }
            //        }
            //    }
            //};

            //context.Tournaments.AddRange(tournaments);
            //await context.SaveChangesAsync();
        }
    }
}

