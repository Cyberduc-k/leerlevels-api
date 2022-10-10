using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Data.Test;

public class TargetContextTests
{
    [Fact]
    public void example_test()
    {
        DbContextOptions<TargetContext> opts = new DbContextOptionsBuilder<TargetContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        using TargetContext context = new(opts);

        context.Database.EnsureCreated();

        Assert.Equal("Lading concept", context.Targets.First().Label);
    }
}
