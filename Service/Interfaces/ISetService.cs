using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface ISetService
{
    /**
     * <summary>
     * Get all sets.
     * </summary>
     * <param name="limit">limits the number of sets returned</param>
     * <param name="page">specifies how many pages of <paramref name="limit"/> should be skipped</param>
     */
    Task<ICollection<Set>> GetAllSetsAsync(int limit, int page);

    /**
     * <summary>
     * Get all sets filtered by <see cref="Set.Name"/>
     * </summary>
     * <param name="limit">limits the number of sets returned</param>
     * <param name="page">specifies how many pages of <paramref name="limit"/> should be skipped</param>
     * <param name="filter">the filter text to be used</param>
     */
    Task<ICollection<Set>> GetAllSetsFilteredAsync(int limit, int page, string filter);

    /**
     * <summary>
     * Get a single set by its id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    Task<Set> GetSetByIdAsync(string setId);

    /**
     * <summary>
     * Create a new set.
     * </summary>
     */
    public Task<Set> CreateSet(Set newSet);

    /**
     * <summary>
     * Update the <see cref="Set.Name"/> of the set with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task UpdateSet(string setId, UpdateSetDTO changes);

    /**
     * <summary>
     * Delete the set with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task DeleteSet(string setId);
}
