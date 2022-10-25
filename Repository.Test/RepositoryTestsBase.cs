using Data;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Xunit;

namespace Repository.Test;

public abstract class RepositoryTestsBase<TRepository, TEntity> : IAsyncDisposable
    where TRepository : IRepository<TEntity>
    where TEntity : class
{
    private readonly Func<TEntity, string> _getId;
    protected readonly DataContext _context;
    protected TRepository _repository;

    public RepositoryTestsBase()
    {
        DbContextOptions opts = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _context = new(opts, false);
        _getId = typeof(TEntity).GetProperty("Id")!.GetMethod!.CreateDelegate<Func<TEntity, string>>();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    protected abstract TEntity CreateMockEntity();

    [Fact]
    public async Task Get_All_Async_Should_Return_Async_Enumerable()
    {
        await _context.AddRangeAsync(
            CreateMockEntity(),
            CreateMockEntity(),
            CreateMockEntity()
        );
        await _context.SaveChangesAsync();

        IAsyncEnumerable<TEntity> entities = _repository.GetAllAsync();

        Assert.Equal(3, await entities.CountAsync());
    }

    [Fact]
    public async Task Get_By_Id_Async_Should_Return_Entity()
    {
        TEntity mockEntity = CreateMockEntity();
        await _context.AddAsync(mockEntity);
        await _context.SaveChangesAsync();

        TEntity? entity = await _repository.GetByIdAsync(_getId(mockEntity));

        Assert.NotNull(entity);
        Assert.Equal(_getId(mockEntity), _getId(entity!));
    }

    [Fact]
    public async Task Get_By_Id_Async_Should_Return_Null()
    {
        TEntity? entity = await _repository.GetByIdAsync("INVALID");

        Assert.Null(entity);
    }

    [Fact]
    public async Task Insert_Async_Should_Increment_Count()
    {
        TEntity entity = CreateMockEntity();

        await _repository.InsertAsync(entity);
        await _context.SaveChangesAsync();

        Assert.Equal(1, await _context.Set<TEntity>().CountAsync());
    }

    [Fact]
    public async Task Remove_Async_Should_Decrement_Count()
    {
        TEntity entity = CreateMockEntity();
        await _context.AddRangeAsync(CreateMockEntity(), entity);
        await _context.SaveChangesAsync();

        await _repository.RemoveAsync(_getId(entity));
        await _context.SaveChangesAsync();

        Assert.Equal(1, await _context.Set<TEntity>().CountAsync());
    }

    [Fact]
    public async Task Save_Changes_Should_Modify_Database()
    {
        await _context.AddRangeAsync(CreateMockEntity(), CreateMockEntity());

        Assert.Equal(0, await _context.Set<TEntity>().CountAsync());

        await _repository.SaveChanges();

        Assert.Equal(2, await _context.Set<TEntity>().CountAsync());
    }
}
