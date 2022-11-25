using Model;

namespace Repository.Interfaces;

public interface IGivenAnswerOptionRepository : IRepository<GivenAnswerOption, (string answerId, AnswerKind kind)>
{
}
