using Microsoft.Extensions.Logging;
using MyFavouritesEntities;

namespace MyFavouritesBusiness;
public class UserService
{
    public Tuple<bool, string[]> ValidateUserName(string userName)
    {
        return Tuple.Create(true, Array.Empty<string>());
    }
}

