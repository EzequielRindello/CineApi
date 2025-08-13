using CineApi.Models.Reservation;

namespace CineApi.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateReservation(CreateReservationDto request, int userId);
        Task<IEnumerable<ReservationDto>> GetUserReservations(int userId);
        Task<IEnumerable<ReservationDto>> GetAllReservations();
        Task<ReservationDto?> GetReservationById(int id);
        Task<UpdateReservationDto> UpdateReservation(int id, UpdateReservationDto updateReservationDto, int userId);
        Task<bool> DeleteReservation(int id, int userId);
    }
}
