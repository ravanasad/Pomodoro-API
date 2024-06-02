using Application.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public sealed class UserTaskRepository : EfRepositoryBase<UserTask>, IUserTaskRepository
{
    public UserTaskRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}

