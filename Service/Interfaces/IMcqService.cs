using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IMcqService
{
    /**
     * <summary>
     * Get all mcqs (multiple choice questions).
     * </summary>
     * <param name="limit">limits the number of mcqs returned</param>
     * <param name="page">specifies how many pages of <paramref name="limit"/> should be skipped</param>
     */
    Task<ICollection<Mcq>> GetAllMcqsAsync(int limit, int page);

    /**
     * <summary>
     * Get all mcqs filtered by <see cref="Mcq.QuestionText"/>
     * </summary>
     * <param name="limit">limits the number of mcqs returned</param>
     * <param name="page">specifies how many pages of <paramref name="limit"/> should be skipped</param>
     * <param name="filter">the filter text to be used</param>
     */
    Task<ICollection<Mcq>> GetAllMcqsFilteredAsync(int limit, int page, string filter);

    /**
     * <summary>
     * Get a single mcq by its id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    Task<Mcq> GetMcqByIdAsync(string mcqId);

    /**
     * <summary>
     * Create a new mcq.
     * </summary>
     */
    public Task<Mcq> CreateMcq(Mcq newMcq);

    /**
     * <summary>
     * Update any property of the mcq with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task UpdateMcq(string mcqId, UpdateMcqDTO changes);

    /**
     * <summary>
     * Delete the mcq with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task DeleteMcq(string mcqId);
}
