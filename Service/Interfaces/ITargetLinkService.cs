namespace Service.Interfaces;

public interface ITargetLinkService
{
    public Task<ICollection<string>> GetNextTargets(string targetId);
    public Task<ICollection<string>> GetPrevTargets(string targetId);
}
