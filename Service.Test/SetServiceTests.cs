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
            new Set("1", "Set 1", new List<Target>()),
            new Set("2", "Set 2", new List<Target>()),
        };

        int limit = int.MaxValue;
        int page = 0;

        _setRepository.Setup(r => r
            .Include(s => s.Targets)
            .ThenInclude(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .OrderBy(s => s.Name)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllAsync()).Returns(mockSets.ToAsyncEnumerable());

        ICollection<Set> sets = await _service.GetAllSetsAsync(limit, page);

        Assert.Equal(2, sets.Count);
    }

    [Fact]
    public async Task Get_Set_By_Id_Should_return_A_Set_Object()
    {
        string setId = "1";
        Set setEntity = new() {
            Id = setId,
            Targets = new List<Target>(),
            Users = new List<User>(),
        };

        _setRepository.Setup(r => r.Include(x => x.Targets)
            .ThenInclude(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions).Include(x => x.Group).GetByAsync(x => x.Id == setId)).ReturnsAsync(setEntity);

        Set set = await _service.GetSetByIdAsync(setEntity.Id);

        Assert.Equal(setEntity.Id, set.Id);
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
