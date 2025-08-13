using CineApi.Entity;
using CineApi.Models.Director;
using CineApi.Models.Function;
using CineApi.Models.LoginDtos;
using CineApi.Models.Movie;
using CineApi.Models.Reservation;

namespace CineApi.Helpers
{
    public class DataMapper
    {
        public static ReservationDto MapToReservationDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                MovieFunctionId = reservation.MovieFunctionId,
                TicketQuantity = reservation.TicketQuantity,
                ReservationDate = reservation.ReservationDate,
                TotalAmount = reservation.TotalAmount,
                User = reservation.User != null ? new UserDto
                {
                    Id = reservation.User.Id,
                    FirstName = reservation.User.FirstName,
                    LastName = reservation.User.LastName,
                    Email = reservation.User.Email,
                    Role = reservation.User.Role.ToString()
                } : null,
                MovieFunction = reservation.MovieFunction != null ? new MovieFunctionDto
                {
                    Id = reservation.MovieFunction.Id,
                    Date = reservation.MovieFunction.Date,
                    Time = reservation.MovieFunction.Time,
                    Price = reservation.MovieFunction.Price,
                    MovieId = reservation.MovieFunction.MovieId,
                    TotalCapacity = reservation.MovieFunction.TotalCapacity,
                    AvailableSeats = reservation.MovieFunction.AvailableSeats,
                    Movie = reservation.MovieFunction.Movie != null ? new MovieDto
                    {
                        Id = reservation.MovieFunction.Movie.Id,
                        Title = reservation.MovieFunction.Movie.Title,
                        Type = reservation.MovieFunction.Movie.Type,
                        Poster = reservation.MovieFunction.Movie.Poster,
                        Description = reservation.MovieFunction.Movie.Description,
                        DirectorId = reservation.MovieFunction.Movie.DirectorId,
                        Director = reservation.MovieFunction.Movie.Director != null ? new DirectorDto
                        {
                            Id = reservation.MovieFunction.Movie.Director.Id,
                            Name = reservation.MovieFunction.Movie.Director.Name,
                            Nationality = reservation.MovieFunction.Movie.Director.Nationality
                        } : null
                    } : null
                } : null
            };
        }

        public static UpdateReservationDto MapToUpdateReservationDto(Reservation reservation)
        {
            return new UpdateReservationDto
            {
                MovieFunctionId = reservation.MovieFunctionId,
                TicketQuantity = reservation.TicketQuantity,
                ReservationDate = reservation.ReservationDate,
                TotalAmount = reservation.TotalAmount,
                User = reservation.User != null ? new UserDto
                {
                    Id = reservation.User.Id,
                    FirstName = reservation.User.FirstName,
                    LastName = reservation.User.LastName,
                    Email = reservation.User.Email,
                    Role = reservation.User.Role.ToString()
                } : null,
                MovieFunction = reservation.MovieFunction != null ? new MovieFunctionDto
                {
                    Id = reservation.MovieFunction.Id,
                    Date = reservation.MovieFunction.Date,
                    Time = reservation.MovieFunction.Time,
                    Price = reservation.MovieFunction.Price,
                    MovieId = reservation.MovieFunction.MovieId,
                    TotalCapacity = reservation.MovieFunction.TotalCapacity,
                    AvailableSeats = reservation.MovieFunction.AvailableSeats
                } : null
            };
        }

        public static MovieDto MapToMovieDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Type = movie.Type,
                Poster = movie.Poster,
                Description = movie.Description,
                DirectorId = movie.DirectorId,
                Director = movie.Director != null ? new DirectorDto
                {
                    Id = movie.Director.Id,
                    Name = movie.Director.Name,
                    Nationality = movie.Director.Nationality
                } : null
            };
        }

        public static MovieFunctionDto MapToMovieDto(MovieFunction function)
        {
            return new MovieFunctionDto
            {
                Id = function.Id,
                Date = function.Date,
                Time = function.Time,
                Price = function.Price,
                MovieId = function.MovieId,
                TotalCapacity = function.TotalCapacity,
                AvailableSeats = function.AvailableSeats,
                Movie = function.Movie != null ? new MovieDto
                {
                    Id = function.Movie.Id,
                    Title = function.Movie.Title,
                    Type = function.Movie.Type,
                    Poster = function.Movie.Poster,
                    Description = function.Movie.Description,
                    DirectorId = function.Movie.DirectorId,
                    Director = function.Movie.Director != null ? new DirectorDto
                    {
                        Id = function.Movie.Director.Id,
                        Name = function.Movie.Director.Name,
                        Nationality = function.Movie.Director.Nationality
                    } : null
                } : null
            };
        }

        public static DirectorDto MapToDirectorDto(Director director)
        {
            return new DirectorDto
            {
                Id = director.Id,
                Name = director.Name,
                Nationality = director.Nationality
            };
        }
    }
}
