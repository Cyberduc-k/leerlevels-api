using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class McqRepository : Repository<Mcq, string>, IMcqRepository
{
    public McqRepository(DataContext context) : base(context, context.Mcqs)
    {
    }
}
