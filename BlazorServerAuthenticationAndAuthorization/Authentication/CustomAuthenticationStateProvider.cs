using BlazorServerAuthenticationAndAuthorization.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BlazorServerAuthenticationAndAuthorization.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>(Constants.USER_SESSION_KEY);
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
            if (userSession == null)
                return await Task.FromResult(new AuthenticationState(_anonymous));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role,userSession.Role)
                }, Constants.CUSTOM_AUTH));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));


        }
        catch (Exception _)
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }
    public async Task UpdateAuthenticationState(UserSession? userSession)
    {
        ClaimsPrincipal claimsPrincipal;
        if (userSession == null)
        {
            await _sessionStorage.DeleteAsync(Constants.USER_SESSION_KEY);
            claimsPrincipal = _anonymous;
        }
        else
        {
            await _sessionStorage.SetAsync(Constants.USER_SESSION_KEY, userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role,userSession.Role)
                }));
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}
