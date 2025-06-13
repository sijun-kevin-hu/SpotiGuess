using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;

namespace SpotiGuessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyAPIController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SpotifyAPIController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        private ISpotifyClient? GetSpotifyClient()
        {
            var accessToken = httpContextAccessor.HttpContext?.Session.GetString("SpotifyAccessToken");
            return !string.IsNullOrEmpty(accessToken) ? new SpotifyClient(accessToken) : null;
        }

        // GET: api/spotifyapi/top-tracks
        // Gets User's Top Tracks
        [HttpGet("top-tracks")]
        public async Task<IActionResult> GetTopTracks(
            [FromQuery] string timeRange = "medium_term",
            [FromQuery] int limit = 20
        )
        {
            var spotify = GetSpotifyClient();

            if (spotify == null)
            {
                return Unauthorized("User not authenticated. Please log in.");
            }
            try
            {
                var tracks = await spotify.Personalization.GetTopTracks();

                if (tracks != null)
                {
                    return Ok(tracks.Items);
                }
                else
                {
                    return NotFound("Could not retrieve track.");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An unexpected error occurred: {e.Message}");
            }
        }

        // GET: api/spotifyapi/top-artists
        // Gets User's Top Artists
        [HttpGet("top-artists")]
        public async Task<IActionResult> GetTopArtists(
            [FromQuery] string timeRange = "medium_term",
            [FromQuery] int limit = 20
        )
        {
            var spotify = GetSpotifyClient();

            if (spotify == null)
            {
                return Unauthorized("User not authenticated. Please log in");
            }

            try
            {
                var artists = await spotify.Personalization.GetTopArtists();

                if (artists != null)
                {
                    return Ok(artists.Items);
                }
                else
                {
                    return NotFound("Could not retrieve artists.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
        
    
}