using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class AnswerOptionRepository : Repository<AnswerOption, string>, IAnswerOptionRepository
{
    public AnswerOptionRepository(DataContext context) : base(context, context.AnswerOptions)
    {
    }
}
