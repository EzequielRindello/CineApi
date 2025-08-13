namespace CineApi.Models.Consts
{
    public class ReservationValidationMessages
    {
        public static string InvalidToken()
        {
            return "Invalid token";
        }

        public static string InternalServerError()
        {
            return "Internal server error";
        }

        public static string ReservationNotFound()
        {
            return "Reservation not found";
        }

        public static string OnlyViewOwnReservations()
        {
            return "You can only view your own reservations";
        }

        public static string ReservationCancelledSuccessfully()
        {
            return "Reservation cancelled successfully";
        }
    }
}
