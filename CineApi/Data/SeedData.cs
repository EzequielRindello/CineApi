using CineApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context.Directors.Any()) return;

            var directors = new[]
            {
                new Director { Name = "Christopher Nolan", Nationality = "British" },
                new Director { Name = "Quentin Tarantino", Nationality = "American" },
                new Director { Name = "Martin Scorsese", Nationality = "American" },
                new Director { Name = "Denis Villeneuve", Nationality = "Canadian" },
                new Director { Name = "Guillermo del Toro", Nationality = "Mexican" },
                new Director { Name = "Pedro Almodóvar", Nationality = "Spanish" },
                new Director { Name = "Alejandro González Iñárritu", Nationality = "Mexican" },
                new Director { Name = "Luis Puenzo", Nationality = "Argentinian" },
                new Director { Name = "Juan José Campanella", Nationality = "Argentinian" },
                new Director { Name = "Pablo Trapero", Nationality = "Argentinian" },
                new Director { Name = "Luisa Bemberg", Nationality = "Argentinian" },
                new Director { Name = "Lisandro Alonso", Nationality = "Argentinian" }
            };

            context.Directors.AddRange(directors);
            context.SaveChanges();

            var movies = new[]
            {
                new Movie { Title = "Inception", DirectorId = directors[0].Id, Type = "international",
                    Poster = "https://http2.mlstatic.com/D_NQ_NP_2X_860530-MLA81194764967_122024-F.webp",
                    Description = "A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O." },
                new Movie { Title = "Pulp Fiction", DirectorId = directors[1].Id, Type = "international",
                    Poster = "https://m.media-amazon.com/images/I/718LfFW+tIL._AC_SL1280_.jpg",
                    Description = "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption." },
                new Movie { Title = "The Departed", DirectorId = directors[2].Id, Type = "international",
                    Poster = "https://www.originalfilmart.com/cdn/shop/products/departed_2006_original_film_art_921946c9-5eaa-43a5-9425-2e13cb2de4ac_5000x.jpg?v=1679940744g",
                    Description = "An undercover cop and a police informant play a deadly game of cat and mouse." },
                new Movie { Title = "Blade Runner 2049", DirectorId = directors[3].Id, Type = "international",
                    Poster = "https://postercity.com.ar/wp-content/uploads/2017/10/blade-runner-2049-poster-main-scaled-scaled.jpg",
                    Description = "Young Blade Runner K's discovery of a long-buried secret leads him to track down former Blade Runner Rick Deckard." },
                new Movie { Title = "El Secreto de Sus Ojos", DirectorId = directors[8].Id, Type = "national",
                    Poster = "https://haddockfilms.com/wp-content/uploads/2019/04/El-secreto-poster.jpg",
                    Description = "A retired legal counselor writes a novel hoping to find closure for one of his past unresolved homicide cases." },
                new Movie { Title = "La Historia Oficial", DirectorId = directors[7].Id, Type = "national",
                    Poster = "https://m.media-amazon.com/images/M/MV5BOWI5NDBlYzUtYzE4ZS00MmI2LTgzOGEtNTVjNzZlMDZmMDcyXkEyXkFqcGc@._V1_.jpg",
                    Description = "During the final months of Argentinian military dictatorship, a high school teacher sets out to find out who the mother of her adopted daughter is." },
                new Movie { Title = "Carancho", DirectorId = directors[9].Id, Type = "national",
                    Poster = "https://es.web.img2.acsta.net/r_1920_1080/medias/nmedia/18/78/18/74/19632325.jpg",
                    Description = "A ambulance-chasing lawyer gets romantically involved with an ambulance paramedic and becomes entangled in a web of corruption." },
                new Movie { Title = "Relatos Salvajes", DirectorId = directors[11].Id, Type = "national",
                    Poster = "https://contarte.com.ar/wp-content/uploads/2024/08/Relatos.webp",
                    Description = "Six short stories that explore the extremities of human behavior involving people in distress." }
            };

            context.Movies.AddRange(movies);
            context.SaveChanges();

            var functions = new[]
            {
                new MovieFunction { MovieId = movies[0].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(14,0,0), Price = 2500 },
                new MovieFunction { MovieId = movies[0].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(17,30,0), Price = 3000 },
                new MovieFunction { MovieId = movies[0].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(21,0,0), Price = 3500 },
                new MovieFunction { MovieId = movies[0].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(15,0,0), Price = 2500 },
                new MovieFunction { MovieId = movies[0].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(19,0,0), Price = 3200 },

                new MovieFunction { MovieId = movies[4].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(16,0,0), Price = 2200 },
                new MovieFunction { MovieId = movies[4].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(19,30,0), Price = 2800 },
                new MovieFunction { MovieId = movies[4].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(14,30,0), Price = 2200 },
                new MovieFunction { MovieId = movies[4].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(18,0,0), Price = 2800 },
                new MovieFunction { MovieId = movies[4].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(21,30,0), Price = 3000 },

                new MovieFunction { MovieId = movies[1].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(20,0,0), Price = 3200 },
                new MovieFunction { MovieId = movies[1].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(22,0,0), Price = 3500 },

                new MovieFunction { MovieId = movies[5].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(15,30,0), Price = 2000 },
                new MovieFunction { MovieId = movies[5].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(17,0,0), Price = 2200 },

                new MovieFunction { MovieId = movies[7].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(18,30,0), Price = 2800 },
                new MovieFunction { MovieId = movies[7].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(21,45,0), Price = 3200 },
                new MovieFunction { MovieId = movies[7].Id, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(16,30,0), Price = 2600 }
            };

            context.MovieFunctions.AddRange(functions);
            context.SaveChanges();
        }
    }
}