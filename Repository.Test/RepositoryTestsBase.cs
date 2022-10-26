using System.Linq.Expressions;
using Data;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Xunit;

namespace Repository.Test;

public abstract class RepositoryTestsBase<TRepository, TEntity, TId> : IAsyncDisposable
    where TRepository : IRepository<TEntity, TId>
    where TEntity : class
    where TId : notnull
{
    private readonly Func<TEntity, TId> _getId;
    protected readonly DataContext _context;
    protected TRepository _repository;

    public RepositoryTestsBase(Func<TEntity, TId>? getId = null)
    {
        DbContextOptions opts = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _context = new(opts, false);
        _getId = getId ?? typeof(TEntity).GetProperty("Id")!.GetMethod!.CreateDelegate<Func<TEntity, TId>>();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    protected abstract TEntity CreateMockEntity();
    protected abstract Expression<Func<TEntity, object>> CreateIncludeExpr();
    protected abstract Expression<Func<TEntity, bool>> CreateAnyTrueExpr(TEntity entity);
    protected abstract Expression<Func<TEntity, bool>> CreateAnyFalseExpr();

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
    public async Task Get_All_Including_Async_Should_Return_Async_Enumerable()
    {
        if (CreateIncludeExpr() is Expression<Func<TEntity, object>> includeExpr) {
            await _context.AddRangeAsync(
                CreateMockEntity(),
                CreateMockEntity(),
                CreateMockEntity()
            );
            await _context.SaveChangesAsync();

            IAsyncEnumerable<TEntity> entities = _repository.GetAllIncludingAsync(includeExpr);

            Assert.Equal(3, await entities.CountAsync());
        }
    }

    [Fact]
    public async Task Get_By_Id_Async_Should_Return_Entity()
    {
        if (_getId is not null) {
            TEntity mockEntity = CreateMockEntity();
            await _context.AddAsync(mockEntity);
            await _context.SaveChangesAsync();

            TEntity? entity = await _repository.GetByIdAsync(_getId(mockEntity));

            Assert.NotNull(entity);
            Assert.Equal(_getId(mockEntity), _getId(entity!));
        }
    }

    [Fact]
    public async Task Get_By_Id_Async_Should_Return_Null()
    {
        TEntity? entity = await _repository.GetByIdAsync(default!);

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

        _repository.Remove(entity);
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

    [Fact]
    public async Task Any_Async_Should_Return_True()
    {
        TEntity mockEntity = CreateMockEntity();
        await _context.AddAsync(mockEntity);
        await _context.SaveChangesAsync();

        bool any = await _repository.AnyAsync(CreateAnyTrueExpr(mockEntity));

        Assert.True(any);
    }

    [Fact]
    public async Task Any_Async_Should_Return_False()
    {
        bool any = await _repository.AnyAsync(CreateAnyFalseExpr());

        Assert.False(any);
    }
}
