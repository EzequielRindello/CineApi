using CineApi.Data;
using CineApi.Entity;
using CineApi.Models;
using Microsoft.EntityFrameworkCore;
using static CineApi.Models.Reservations;

namespace CineApi.Services
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateReservationAsync(CreateReservationDto request, int userId);
        Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(int userId);
        Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
        Task<ReservationDto?> GetReservationByIdAsync(int id);
        Task<UpdateReservationDto> UpdateReservationAsync(int id, UpdateReservationDto updateReservationDto, int userId);
        Task<bool> CancelReservationAsync(int id, int userId);
    }

    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;
        private const int MAX_TICKETS_PER_USER = 4;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto request, int userId)
        {
            var movieFunction = await _context.MovieFunctions
                .Include(mf => mf.Reservations)
                .FirstOrDefaultAsync(mf => mf.Id == request.MovieFunctionId);

            if (movieFunction == null)
                throw new ArgumentException("Movie function not found");

            var availableSeats = movieFunction.AvailableSeats;
            if (availableSeats < request.TicketQuantity)
                throw new InvalidOperationException($"Only {availableSeats} seats available");

            var functionDateTime = movieFunction.Date.Date + movieFunction.Time;
            if (functionDateTime <= DateTime.Now)
                throw new InvalidOperationException("Cannot reserve tickets for past functions");

            var existingTicketsForFunction = await _context.Reservations
                .Where(r => r.UserId == userId && r.MovieFunctionId == request.MovieFunctionId)
                .SumAsync(r => r.TicketQuantity);

            var totalTicketsAfterReservation = existingTicketsForFunction + request.TicketQuantity;

            if (totalTicketsAfterReservation > MAX_TICKETS_PER_USER)
            {
                var remainingTickets = MAX_TICKETS_PER_USER - existingTicketsForFunction;
                throw new InvalidOperationException(
                    $"Cannot reserve {request.TicketQuantity} tickets. " +
                    $"Maximum {MAX_TICKETS_PER_USER} tickets per user per function. " +
                    $"You already have {existingTicketsForFunction} tickets reserved. " +
                    $"You can only reserve {remainingTickets} more ticket(s) for this function.");
            }

            var totalAmount = movieFunction.Price * request.TicketQuantity;

            var reservation = new Reservation
            {
                UserId = userId,
                MovieFunctionId = request.MovieFunctionId,
                TicketQuantity = request.TicketQuantity,
                ReservationDate = DateTime.UtcNow,
                TotalAmount = totalAmount
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return await GetReservationByIdAsync(reservation.Id) ?? throw new InvalidOperationException("Failed to create reservation");
        }

        public async Task<UpdateReservationDto> UpdateReservationAsync(int id, UpdateReservationDto updateReservationDto, int userId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.MovieFunction)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (reservation == null)
                throw new ArgumentException("Reservation not found");

            var existingTicketsForFunction = await _context.Reservations
                .Where(r => r.UserId == userId && r.MovieFunctionId == reservation.MovieFunctionId && r.Id != id)
                .SumAsync(r => r.TicketQuantity);

            var totalTicketsAfterUpdate = existingTicketsForFunction + updateReservationDto.TicketQuantity;

            if (totalTicketsAfterUpdate > MAX_TICKETS_PER_USER)
            {
                var remainingTickets = MAX_TICKETS_PER_USER - existingTicketsForFunction;
                throw new InvalidOperationException(
                    $"Cannot update to {updateReservationDto.TicketQuantity} tickets. " +
                    $"Maximum {MAX_TICKETS_PER_USER} tickets per user per function. " +
                    $"You have {existingTicketsForFunction} other tickets reserved for this function. " +
                    $"You can only reserve up to {remainingTickets} ticket(s) for this reservation.");
            }

            reservation.TicketQuantity = updateReservationDto.TicketQuantity;
            reservation.TotalAmount = reservation.MovieFunction.Price * updateReservationDto.TicketQuantity;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            return MapToUpdateDto(reservation);
        }

        public async Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(int userId)
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.MovieFunction)
                .ThenInclude(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return reservations.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.MovieFunction)
                .ThenInclude(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return reservations.Select(MapToDto);
        }

        public async Task<ReservationDto?> GetReservationByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.MovieFunction)
                .ThenInclude(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reservation != null ? MapToDto(reservation) : null;
        }

        public async Task<bool> CancelReservationAsync(int id, int userId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.MovieFunction)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null || reservation.UserId != userId)
                return false;

            var functionDateTime = reservation.MovieFunction.Date.Date + reservation.MovieFunction.Time;
            if (functionDateTime <= DateTime.Now.AddHours(1))
                throw new InvalidOperationException("Cannot cancel reservations less than 1 hour before the function");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ReservationDto MapToDto(Reservation reservation)
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

        private static UpdateReservationDto MapToUpdateDto(Reservation reservation)
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
    }
}