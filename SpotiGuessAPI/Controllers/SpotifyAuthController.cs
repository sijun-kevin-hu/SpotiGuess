using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using DotNetEnv;

namespace SpotiGuessAPI
{
    [ApiController]
    [Route("api/auth")]
    public class SpotifyAuthController : ControllerBase
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly System.Uri redirectUri;
        private const string OAuthStateKey = "OAuthState";

        public SpotifyAuthController()
        {
            clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID")
                ?? throw new InvalidOperationException("Missing environment variable: SPOTIFY_CLIENT_ID");

            clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET")
                ?? throw new InvalidOperationException("Missing environment variable: SPOTIFY_CLIENT_SECRET");

            var redirectUriStr = Environment.GetEnvironmentVariable("SPOTIFY_REDIRECT_URI")
                ?? throw new InvalidOperationException("Missing environment variable: SPOTIFY_REDIRECT_URI");

            redirectUri = new System.Uri(redirectUriStr);
        }
        
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