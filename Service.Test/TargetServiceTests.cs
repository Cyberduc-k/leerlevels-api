﻿using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;

namespace Service.Test;

public class TargetServiceTests
{
    private readonly Mock<ITargetRepository> _mockRepository;
    private readonly TargetService _service;

    public TargetServiceTests()
    {
        _mockRepository = new();
        _service = new TargetService(new LoggerFactory(), _mockRepository.Object);
    }

    [Fact]
    public async Task Get_All_Targets_Should_return_an_array_of_targets()
    {
        Target[] mockTargets = new[] {
            new Target("1", "Lading concept",
                "Je kan in eigen woorden uitleggen welk effect lading kan hebben.",
                "Lading is een eigenschap die bepaalt hoe een deeltje wordt beïnvloed door een elektrisch of magnetisch veld.",
                "0ouf-xbz7_o",
               "https://s3-us-west-2.amazonaws.com/leerlevels/slide_pngs/2.png", null!),

            new Target("2", "Lading concept",
                  "Je kan in eigen woorden uitleggen welk effect lading kan hebben.",
                  "Lading is een eigenschap die bepaalt hoe een deeltje wordt beïnvloed door een elektrisch of magnetisch veld.",
                  "0ouf-xbz7_o",
                 "https://s3-us-west-2.amazonaws.com/leerlevels/slide_pngs/2.png", null!),
        };

        _mockRepository
            .Setup(r => r.Include(x => x.Mcqs).ThenInclude(m => m.AnswerOptions).GetAllAsync())
            .Returns(mockTargets.ToAsyncEnumerable());

        ICollection<Target> mcqs = await _service.GetAllTargetsAsync(int.MaxValue, 0);

        Assert.Equal(2, mcqs.Count);
    }

    [Fact]
    public async Task Get_Target_By_Id_Should_return_A_Target_Object()
    {
        _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => new Target("1", "Lading concept",
                  "Je kan in eigen woorden uitleggen welk effect lading kan hebben.",
                  "Lading is een eigenschap die bepaalt hoe een deeltje wordt beïnvloed door een elektrisch of magnetisch veld.",
                  "0ouf-xbz7_o",
                 "https://s3-us-west-2.amazonaws.com/leerlevels/slide_pngs/2.png", null!));
        Target target = await _service.GetTargetByIdAsync("1");

        Assert.Equal("1", target.Id);
    }

    [Fact]
    public void Get_Target_By_Id_should_Throws_Not_Found_Exception()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NotFoundException>(() => _service.GetTargetByIdAsync("3"));
    }

    [Fact]
    public void Get_Target_By_Id_should_Throws_null_Exception()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsNotIn(""))).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NullReferenceException>(() => _service.GetTargetByIdAsync(""));
    }
}
