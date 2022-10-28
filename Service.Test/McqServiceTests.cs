using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;

namespace Service.Test;

public class McqServiceTests
{
    private readonly Mock<IMcqRepository> _mockRepository;
    private readonly McqService _service;

    public McqServiceTests()
    {
        _mockRepository = new();
        _service = new McqService(new LoggerFactory(), _mockRepository.Object);
    }

    [Fact]
    public async Task Get_All_Mcqs_Should_return_an_array_of_mcqs_with_answer_options()
    {
        Mcq[] mockMcqs = new[] {
            new Mcq(
                "1",
                null!,
                "Wat kun je zeggen over de lading van twee voorwerpen die elkaar aantrekken?",
                "Voorwerpen met gelijksoortige lading (beide positief of beide negatief) stoten elkaar af. Voorwerpen met tegengestelde lading (een positief, een negatief) trekken elkaar aan.",
                true,
                null!
            ),
            new Mcq(
                "2",
                null!,
                "Wat kun je zeggen?",
                "elkaar aan.",
                true,
                null!
            ),
        };

        _mockRepository.Setup(r => r.Include(x => x.AnswerOptions).GetAllAsync()).Returns(mockMcqs.ToAsyncEnumerable());

        ICollection<Mcq> mcqs = await _service.GetAllMcqsAsync();

        Assert.Equal(2, mcqs.Count);
    }

    [Fact]
    public async Task Get_Mcq_By_Id_Should_return_A_Mcq_Object()
    {
        _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => new Mcq("1", null!, "Wat kun je zeggen?", "elkaar aan.", true, null!));
        Mcq mcq = await _service.GetMcqByIdAsync("1");

        Assert.Equal("1", mcq.Id);
    }

    [Fact]
    public void Get_Mcq_By_Id_should_Throws_Not_Found_Exception()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NotFoundException>(() => _service.GetMcqByIdAsync("3"));
    }

    [Fact]
    public void Get_Mcq_By_Id_should_Throws_null_Exception()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsNotIn(""))).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NullReferenceException>(() => _service.GetMcqByIdAsync(""));
    }
}
