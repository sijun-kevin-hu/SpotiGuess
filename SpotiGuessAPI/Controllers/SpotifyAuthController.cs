using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using Microsoft.AspNetCore.Http;

namespace SpotiGuessAPI
{
    [ApiController]
    [Route("api/auth")]
    public class SpotifyAuthController : ControllerBase
    {
        private readonly string clientId = "2a9264709c004ef096a93aa1fd2992df";
        private readonly string clientSecret = "ed8aae92e0cd420ca4ab8af6d7b0143d";
        private readonly System.Uri redirectUri = new System.Uri("http://127.0.0.1:5207/api/auth/callback");
        private const string OAuthStateKey = "OAuthState";

        [HttpGet("login")]
        public IActionResult Login()
        {
            // DO NOT DO THIS IN PRODUCTION
            var state = "my-very-secret-state";

            HttpContext.Session.SetString(OAuthStateKey, state);

            var loginRequest = new LoginRequest(redirectUri, clientId, LoginRequest.ResponseType.Code)
            {
                Scope = new[] { Scopes.UserLibraryRead, Scopes.UserTopRead, Scopes.UserFollowRead },
                State = state
            };

            // Redirect user to uri
            var uri = loginRequest.ToUri();
            return Redirect(uri.ToString());
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            var storedState = HttpContext.Session.GetString(OAuthStateKey);

            if (string.IsNullOrEmpty(state) || state != storedState)
            {
                return BadRequest("Invalid state parameter.");
            }

            HttpContext.Session.Remove(OAuthStateKey);

            try
            {
                var response = await new OAuthClient().RequestToken(
                    new AuthorizationCodeTokenRequest(clientId, clientSecret, code, redirectUri)
                );

                try
                {
                    HttpContext.Session.SetString("SpotifyAccessToken", response.AccessToken);
                    HttpContext.Session.SetString("SpotifyRefreshToken", response.RefreshToken);
                    HttpContext.Session.SetString("SpotifyTokenExpireAt", DateTimeOffset.Now.AddSeconds(response.ExpiresIn).ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error setting session: {ex.Message}");
                }

                return Redirect("/api/spotifyapi/top-tracks");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve access token: {ex.Message}");
            }
            
        }
    }
}