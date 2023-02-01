namespace Service.Interfaces;

public interface ITargetLinkService
{
    /**
     * <summary>
     * Get all targets that should be completed after this one.
     * </summary>
     */
    public Task<ICollection<string>> GetNextTargets(string targetId);

    /**
     * <summary>
     * Get all targets that should be completed before this one.
     * </summary>
     */
    public Task<ICollection<string>> GetPrevTargets(string targetId);
}
