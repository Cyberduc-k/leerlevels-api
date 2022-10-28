using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;

namespace Service.Test;

public class SetServiceTests
{
    private readonly Mock<ISetRepository> _setRepository;
    private readonly SetService _service;

    public SetServiceTests()
    {
        _setRepository = new();
        _service = new SetService(new LoggerFactory(), _setRepository.Object);
    }

    [Fact]
    public async Task Get_All_Sets_Should_return_an_array_of_Sets()
    {
        Set[] mockSets = new[] {
            new Set("1", null!, null!),
            new Set("2", null!, null!),
        };

        _setRepository.Setup(r => r.Include(x => x.Targets).Include(x => x.Users).GetAllAsync()).Returns(mockSets.ToAsyncEnumerable());

        ICollection<Set> sets = await _service.GetAllSetsAsync();

        Assert.Equal(2, sets.Count);
    }

    [Fact]
    public async Task Get_Set_By_Id_Should_return_A_Set_Object()
    {
        _setRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => new Set("1", null!, null!));
        Set set = await _service.GetSetByIdAsync("1");

        Assert.Equal("1", set.Id);
    }

    [Fact]
    public void Get_Set_By_Id_should_Throws_Not_Found_Exception()
    {
        _setRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NotFoundException>(() => _service.GetSetByIdAsync("3"));
    }

    [Fact]
    public void Get_Set_By_Id_should_Throws_null_Exception()
    {
        _setRepository.Setup(r => r.GetByIdAsync(It.IsNotIn(""))).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NullReferenceException>(() => _service.GetSetByIdAsync(""));
    }
}
