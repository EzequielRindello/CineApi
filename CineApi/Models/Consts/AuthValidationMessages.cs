namespace CineApi.Models.Consts
{
    public class AuthValidationMessages
    {
        public static string InvalidCredentials()
        {
            return "Invalid credentials";
        }

        public static string UserAlreadyExists()
        {
            return "User already exists";
        }

        public static string EmailAlreadyUsed()
        {
            return "Email already in use";
        }

        public static string InternalServerError()
        {
            return "Internal server error";
        }

        public static string UserNotFound()
        {
            return "User not found";
        }

        public static string OnlyViewOwnProfile()
        {
            return "You can only view your own profile";
        }

        public static string UserDeletedSuccessfully()
        {
            return "User deleted successfully";
        }
    }
}
