using BlazorServerAuthenticationAndAuthorization.Shared;

namespace BlazorServerAuthenticationAndAuthorization.Authentication;

public class UserAccountService
{
    private readonly List<UserAccount> __userAccountList;
    public UserAccountService()
    {
        __userAccountList = new List<UserAccount>
        {
            new UserAccount{UserName="admin",Password="admin",Role=Constants.ADMINISTRATOR_ROLE},
            new UserAccount{UserName="user",Password="user",Role=Constants.USER_ROLE}
        };
    }

    public UserAccount? GetUserAccountByUserName(string userName) => __userAccountList.FirstOrDefault(x => x.UserName == userName);
}