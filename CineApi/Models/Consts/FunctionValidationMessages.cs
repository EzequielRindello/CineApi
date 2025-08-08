namespace CineApi.Models.Consts
{
    public class FunctionValidationMessages
    {
        public static string MovieFunctionNotFound()
        {
            return "Movie function not found";
        }

        public static string ReservationNotFound()
        {
            return "Reservation not found";
        }

        public static string AvailableSeats(int seats)
        {
            return $"Only {seats} seats available";
        }

        public static string PastFunction()
        {
            return "Cannot reserve tickets for past functions";
        }

        public static string MaxTicketsExceeded(int requested, int maxPerUser, int existing)
        {
            var remaining = maxPerUser - existing;
            return $"Cannot reserve {requested} tickets. " +
                   $"Maximum {maxPerUser} tickets per user per function. " +
                   $"You already have {existing} tickets reserved. " +
                   $"You can only reserve {remaining} more ticket(s) for this function.";
        }

        public static string MaxTicketsExceededOnUpdate(int requested, int maxPerUser, int existing)
        {
            var remaining = maxPerUser - existing;
            return $"Cannot update to {requested} tickets. " +
                   $"Maximum {maxPerUser} tickets per user per function. " +
                   $"You have {existing} other tickets reserved for this function. " +
                   $"You can only reserve up to {remaining} ticket(s) for this reservation.";
        }

        public static string FailedToCreate()
        {
            return "Failed to create reservation";
        }

        public static string CannotCancelOneHourBefore()
        {
            return "Cannot cancel reservations less than 1 hour before the function";
        }

        public static string ErrorCreatingFunction()
        {
            return "Error creating function";
        }

        public static string ErrorUpdatingFunction()
        {
            return "Error updating function";
        }
    }
}
