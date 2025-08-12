namespace CineApi.Models.Reservation
{
    public class CreateReservationDto
    {
        public required int MovieFunctionId { get; set; }
        public required int TicketQuantity { get; set; }
    }
}
