using CineApi.Data;
using CineApi.Entity;
using CineApi.Helpers;
using CineApi.Interfaces;
using CineApi.Models.Consts;
using CineApi.Models.Reservation;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;
        private const int MAX_TICKETS_PER_USER = 4;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationDto> CreateReservation(CreateReservationDto request, int userId)
        {
            var movieFunction = await _context.MovieFunctions
                .Include(mf => mf.Reservations)
                .FirstOrDefaultAsync(mf => mf.Id == request.MovieFunctionId)
                ?? throw new ArgumentException(FunctionValidationMessages.MovieFunctionNotFound());

            var availableSeats = movieFunction.AvailableSeats;
            if (availableSeats < request.TicketQuantity)
                throw new InvalidOperationException(FunctionValidationMessages.AvailableSeats(availableSeats));

            var functionDateTime = movieFunction.Date.Date + movieFunction.Time;
            if (functionDateTime <= DateTime.Now)
                throw new InvalidOperationException(FunctionValidationMessages.PastFunction());

            var existingTicketsForFunction = await _context.Reservations
                .Where(r => r.UserId == userId && r.MovieFunctionId == request.MovieFunctionId)
                .SumAsync(r => r.TicketQuantity);

            var totalTicketsAfterReservation = existingTicketsForFunction + request.TicketQuantity;

            if (totalTicketsAfterReservation > MAX_TICKETS_PER_USER)
            {
                throw new InvalidOperationException(
                    FunctionValidationMessages.MaxTicketsExceeded(
                        request.TicketQuantity,
                        MAX_TICKETS_PER_USER,
                        existingTicketsForFunction
                    )
                );
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

            return await GetReservationById(reservation.Id) ?? throw new InvalidOperationException(FunctionValidationMessages.FailedToCreate());
        }

        public async Task<UpdateReservationDto> UpdateReservation(int id, UpdateReservationDto updateReservationDto, int userId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.MovieFunction)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId)
                ?? throw new ArgumentException(FunctionValidationMessages.ReservationNotFound());

            var existingTicketsForFunction = await _context.Reservations
                .Where(r => r.UserId == userId && r.MovieFunctionId == reservation.MovieFunctionId && r.Id != id)
                .SumAsync(r => r.TicketQuantity);

            var totalTicketsAfterUpdate = existingTicketsForFunction + updateReservationDto.TicketQuantity;

            if (totalTicketsAfterUpdate > MAX_TICKETS_PER_USER)
            {
                throw new InvalidOperationException(
                    FunctionValidationMessages.MaxTicketsExceededOnUpdate(
                        updateReservationDto.TicketQuantity,
                        MAX_TICKETS_PER_USER,
                        existingTicketsForFunction
                    )
                );
            }

            reservation.TicketQuantity = updateReservationDto.TicketQuantity;
            reservation.TotalAmount = reservation.MovieFunction.Price * updateReservationDto.TicketQuantity;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            return DataMapper.MapToUpdateReservationDto(reservation);
        }

        public async Task<IEnumerable<ReservationDto>> GetUserReservations(int userId)
        {
            var reservations = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.User)
                .Include(r => r.MovieFunction)
                .ThenInclude(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .AsSplitQuery()
                .ToListAsync();

            return reservations.Select(DataMapper.MapToReservationDto);
        }

        public async Task<IEnumerable<ReservationDto>> GetAllReservations()
        {
            var reservations = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.User)
                .Include(r => r.MovieFunction)
                .ThenInclude(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .OrderByDescending(r => r.ReservationDate)
                .AsSplitQuery()
                .ToListAsync();

            return reservations.Select(DataMapper.MapToReservationDto);
        }

        public async Task<ReservationDto?> GetReservationById(int id)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.User)
                .Include(r => r.MovieFunction)
                .ThenInclude(mf => mf.Movie)
                .ThenInclude(m => m.Director)
                .AsSplitQuery()
                .FirstOrDefaultAsync(r => r.Id == id);

            return reservation != null ? DataMapper.MapToReservationDto(reservation) : null;
        }

        public async Task<bool> DeleteReservation(int id, int userId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.MovieFunction)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null || reservation.UserId != userId)
                return false;

            var functionDateTime = reservation.MovieFunction.Date.Date + reservation.MovieFunction.Time;
            if (functionDateTime <= DateTime.Now.AddHours(1))
                throw new InvalidOperationException(FunctionValidationMessages.CannotCancelOneHourBefore());

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}