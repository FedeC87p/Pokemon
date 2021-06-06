using Microsoft.Extensions.Logging;
using Moq;
using PokemonAPI.ApplicationCore;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.ApplicationCore
{
    public class DescriptionFactoryTest
    {
        readonly ILogger<TranslatedDescriptionFactory> _logger;
        readonly Mock<ITransationService> _transationService;
        ITranslatedDescriptionFactory _descriptionFactory;

        public DescriptionFactoryTest()
        {
            _logger = new Mock<ILogger<TranslatedDescriptionFactory>>().Object;
            _transationService = new Mock<ITransationService>();

            _descriptionFactory = new TranslatedDescriptionFactory(_logger, _transationService.Object);
        }

        [Fact]
        public async Task ShouldGetYodaTranslate_WhenIsLeggendary()
        {
            var dto = new PokemonAPI.DomainEntity.PokemonDto { IsLeggendary = true };


            var result = await _descriptionFactory.GetTranslatedDescriptionAsync(dto);


            _transationService.Verify(i => i.GetYodaMessageAsync(dto.Description));
        }

        [Fact]
        public async Task ShouldGetYodaTranslate_WhenHabitatCave()
        {
            var dto = new PokemonAPI.DomainEntity.PokemonDto { Habitat = "cave" };


            var result = await _descriptionFactory.GetTranslatedDescriptionAsync(dto);


            _transationService.Verify(i => i.GetYodaMessageAsync(dto.Description));
        }

        [Fact]
        public async Task ShouldGetYodaTranslate_WhenHabitatCaveAndLeggendary()
        {
            var dto = new PokemonAPI.DomainEntity.PokemonDto { IsLeggendary = true, Habitat = "cave" };


            var result = await _descriptionFactory.GetTranslatedDescriptionAsync(dto);


            _transationService.Verify(i => i.GetYodaMessageAsync(dto.Description));
        }

        [Fact]
        public async Task ShouldGetShakespeareTranslate_WhenNotHabitatCaveAndNotLeggendary()
        {
            var dto = new PokemonAPI.DomainEntity.PokemonDto { IsLeggendary = false, Habitat = "notcave" };


            var result = await _descriptionFactory.GetTranslatedDescriptionAsync(dto);


            _transationService.Verify(i => i.GetShakespeareMessageAsync(dto.Description));
        }

    }
}
