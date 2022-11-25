using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class GivenAnswerOptionRepository : Repository<GivenAnswerOption, (string answerId, AnswerKind kind)>, IGivenAnswerOptionRepository
{
    public GivenAnswerOptionRepository(DataContext context) : base(context, context.GivenAnswerOptions)
    {
    }
}
