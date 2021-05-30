using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PokemonAPI.ApplicationCore;

namespace PokemonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonManager _pokemonManager;

        public PokemonController(
            ILogger<PokemonController> logger,
            IPokemonManager pokemonManager)
        {
            _logger = logger;
            _pokemonManager = pokemonManager;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var pokemon = await _pokemonManager.GetPokemonAsync(name);

            if (pokemon == null)
            {
                return NotFound();
            }

            return Ok(pokemon);
        }

        [HttpGet("translated/{name}")]
        public async Task<IActionResult> GetTranslated(string name)
        {
            var pokemon = await _pokemonManager.GetFunnyPokemonAsync(name);

            if (pokemon == null)
            {
                return NotFound();
            }

            return Ok(pokemon);
        }
    }
}
