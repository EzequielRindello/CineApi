namespace CineApi.Models.Consts
{
    public class MovieValidationMessage
    {
        public static string DirectorNotfound()
        {
            return "Director not found";
        }

        public static string MovieNotfound()
        {
            return "Movie Not Found";
        }

        public static string ErrorCreatingMovie()
        {
            return "Error creating movie";
        }

        public static string ErrorUpdatingMovie()
        {
            return "Error updating movie";
        }

        public static string ErrorDeletingMovie()
        {
            return "Error deleting movie";
        }

        public static string MovieIdMismatch()
        {
            return "Movie ID mismatch";
        }
    }
}
