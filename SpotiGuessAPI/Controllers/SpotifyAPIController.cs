using System.ComponentModel.DataAnnotations;
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
            [FromQuery] PersonalizationTopRequest.TimeRange? timeRange,
            [FromQuery] int limit
        )
        {   
            if (!timeRange.HasValue)
            {
                return BadRequest("Parameter 'timeRange' is required.");
            }

            if (limit <= 0 || limit > 50)
            {
                return BadRequest("Parameter 'limit' must be between 1 and 50");
            }

            var spotify = GetSpotifyClient();
            if (spotify == null)
            {
                return Unauthorized("User not authenticated. Please log in.");
            }
            try
            {
                var request = new PersonalizationTopRequest
                {
                    Limit = limit,
                    TimeRangeParam = timeRange
                };

                var tracks = await spotify.Personalization.GetTopTracks(request);

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