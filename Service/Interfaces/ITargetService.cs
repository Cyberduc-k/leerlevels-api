using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface ITargetService
{
    /**
     * <summary>
     * Get all targets.
     * </summary>
     * <param name="limit">limits the number of targets returned</param>
     * <param name="page">specifies how many pages of <paramref name="limit"/> should be skipped</param>
     */
    Task<ICollection<Target>> GetAllTargetsAsync(int limit, int page);

    /**
     * <summary>
     * Get all targets filtered by <see cref="Target.Label"/>
     * </summary>
     * <param name="limit">limits the number of targets returned</param>
     * <param name="page">specifies how many pages of <paramref name="limit"/> should be skipped</param>
     * <param name="filter">the filter text to be used</param>
     */
    Task<ICollection<Target>> GetAllTargetsFilteredAsync(int limit, int page, string filter);

    /**
     * <summary>
     * Get a single target by its id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    Task<Target> GetTargetByIdAsync(string targetId);

    /**
     * <summary>
     * Get the number of targets.
     * More effecient version of <code>(await GetAllTargetsAsync(int.MaxValue, 0)).Count()</code>
     * </summary>
     */
    Task<int> GetTargetCountAsync();

    /**
     * <summary>
     * Create a new target.
     * </summary>
     */
    public Task<Target> CreateTarget(Target newTarget);

    /**
     * <summary>
     * Update any property of the target with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task UpdateTarget(string targetId, UpdateTargetDTO changes);

    /**
     * <summary>
     * Delete the target with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task DeleteTarget(string targetId);
}
