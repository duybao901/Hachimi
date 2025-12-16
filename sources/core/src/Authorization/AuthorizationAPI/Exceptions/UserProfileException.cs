using AuthorizationApi.Exceptions;

namespace AuthorizationAPI.Exceptions;

public static class UserProfileException
{
    public class UserProfileNotFoundWithUserIdException : BadRequestException        
    {
        public UserProfileNotFoundWithUserIdException(Guid userId)
            : base($"A user with the userId {userId} not exists.")
        {
        }
    }

}
