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
                new Director { Id = 1, Name = "Christopher Nolan", Nationality = "British" },
                new Director { Id = 2, Name = "Quentin Tarantino", Nationality = "American" },
                new Director { Id = 3, Name = "Martin Scorsese", Nationality = "American" },
                new Director { Id = 4, Name = "Denis Villeneuve", Nationality = "Canadian" },
                new Director { Id = 5, Name = "Guillermo del Toro", Nationality = "Mexican" },
                new Director { Id = 6, Name = "Pedro Almodóvar", Nationality = "Spanish" },
                new Director { Id = 7, Name = "Alejandro González Iñárritu", Nationality = "Mexican" },
                new Director { Id = 8, Name = "Luis Puenzo", Nationality = "Argentinian" },
                new Director { Id = 9, Name = "Juan José Campanella", Nationality = "Argentinian" },
                new Director { Id = 10, Name = "Pablo Trapero", Nationality = "Argentinian" },
                new Director { Id = 11, Name = "Luisa Bemberg", Nationality = "Argentinian" },
                new Director { Id = 12, Name = "Lisandro Alonso", Nationality = "Argentinian" }
            };

            var movies = new[]
            {
                new Movie { Id = 1, Title = "Inception", DirectorId = 1, Type = "international",
                    Poster = "https://http2.mlstatic.com/D_NQ_NP_2X_860530-MLA81194764967_122024-F.webp",
                    Description = "A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O." },
                new Movie { Id = 2, Title = "Pulp Fiction", DirectorId = 2, Type = "international",
                    Poster = "https://m.media-amazon.com/images/I/718LfFW+tIL._AC_SL1280_.jpg",
                    Description = "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption." },
                new Movie { Id = 3, Title = "The Departed", DirectorId = 3, Type = "international",
                    Poster = "https://www.originalfilmart.com/cdn/shop/products/departed_2006_original_film_art_921946c9-5eaa-43a5-9425-2e13cb2de4ac_5000x.jpg?v=1679940744g",
                    Description = "An undercover cop and a police informant play a deadly game of cat and mouse." },
                new Movie { Id = 4, Title = "Blade Runner 2049", DirectorId = 4, Type = "international",
                    Poster = "https://postercity.com.ar/wp-content/uploads/2017/10/blade-runner-2049-poster-main-scaled-scaled.jpg",
                    Description = "Young Blade Runner K's discovery of a long-buried secret leads him to track down former Blade Runner Rick Deckard." },
                new Movie { Id = 5, Title = "El Secreto de Sus Ojos", DirectorId = 9, Type = "national",
                    Poster = "https://haddockfilms.com/wp-content/uploads/2019/04/El-secreto-poster.jpg",
                    Description = "A retired legal counselor writes a novel hoping to find closure for one of his past unresolved homicide cases." },
                new Movie { Id = 6, Title = "La Historia Oficial", DirectorId = 8, Type = "national",
                    Poster = "https://m.media-amazon.com/images/M/MV5BOWI5NDBlYzUtYzE4ZS00MmI2LTgzOGEtNTVjNzZlMDZmMDcyXkEyXkFqcGc@._V1_.jpg",
                    Description = "During the final months of Argentinian military dictatorship, a high school teacher sets out to find out who the mother of her adopted daughter is." },
                new Movie { Id = 7, Title = "Carancho", DirectorId = 10, Type = "national",
                    Poster = "https://es.web.img2.acsta.net/r_1920_1080/medias/nmedia/18/78/18/74/19632325.jpg",
                    Description = "A ambulance-chasing lawyer gets romantically involved with an ambulance paramedic and becomes entangled in a web of corruption." },
                new Movie { Id = 8, Title = "Relatos Salvajes", DirectorId = 12, Type = "national",
                    Poster = "https://contarte.com.ar/wp-content/uploads/2024/08/Relatos.webp",
                    Description = "Six short stories that explore the extremities of human behavior involving people in distress." }
            };

            var functions = new[]
            {
                new MovieFunction { Id = 1, MovieId = 1, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(14,0,0), Price = 2500 },
                new MovieFunction { Id = 2, MovieId = 1, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(17,30,0), Price = 3000 },
                new MovieFunction { Id = 3, MovieId = 1, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(21,0,0), Price = 3500 },
                new MovieFunction { Id = 4, MovieId = 1, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(15,0,0), Price = 2500 },
                new MovieFunction { Id = 5, MovieId = 1, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(19,0,0), Price = 3200 },
                new MovieFunction { Id = 6, MovieId = 5, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(16,0,0), Price = 2200 },
                new MovieFunction { Id = 7, MovieId = 5, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(19,30,0), Price = 2800 },
                new MovieFunction { Id = 8, MovieId = 5, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(14,30,0), Price = 2200 },
                new MovieFunction { Id = 9, MovieId = 5, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(18,0,0), Price = 2800 },
                new MovieFunction { Id = 10, MovieId = 5, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(21,30,0), Price = 3000 },
                new MovieFunction { Id = 11, MovieId = 2, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(20,0,0), Price = 3200 },
                new MovieFunction { Id = 12, MovieId = 2, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(22,0,0), Price = 3500 },
                new MovieFunction { Id = 13, MovieId = 6, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(15,30,0), Price = 2000 },
                new MovieFunction { Id = 14, MovieId = 6, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(17,0,0), Price = 2200 },
                new MovieFunction { Id = 15, MovieId = 8, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(18,30,0), Price = 2800 },
                new MovieFunction { Id = 16, MovieId = 8, Date = DateTime.SpecifyKind(new DateTime(2025,6,26), DateTimeKind.Utc), Time = new TimeSpan(21,45,0), Price = 3200 },
                new MovieFunction { Id = 17, MovieId = 8, Date = DateTime.SpecifyKind(new DateTime(2025,6,27), DateTimeKind.Utc), Time = new TimeSpan(16,30,0), Price = 2600 }
            };

            context.Directors.AddRange(directors);
            context.Movies.AddRange(movies);
            context.MovieFunctions.AddRange(functions);
            context.SaveChanges();
        }
    }
}
