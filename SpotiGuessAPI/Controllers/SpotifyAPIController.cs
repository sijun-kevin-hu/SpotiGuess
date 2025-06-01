using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SpotifyAPI.Web;

namespace SpotiGuessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyAPIController : ControllerBase
    {
        private ISpotifyClient? spotify;

        public SpotifyAPIController(IHttpContextAccessor httpContextAccessor)
        {
            var accessToken = httpContextAccessor.HttpContext?.Session.GetString("SpotifyAccessToken");
            if (!string.IsNullOrEmpty(accessToken))
            {
                spotify = new SpotifyClient(accessToken);
            }
            else
            {
                spotify = null;
                throw new Exception("No access token");
            }
        }

        // GET: api/spotifyapi/top-tracks
        // Gets User's Top Tracks
        [HttpGet("top-tracks")]
        public async Task<IActionResult> GetTopTracks()
        {
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
        public async Task<IActionResult> GetTopArtists()
        {
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