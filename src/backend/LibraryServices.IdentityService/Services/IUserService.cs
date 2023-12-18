using LibraryServices.Domain.Models.Identity;
using LibraryServices.Infrastructure.Repository;

namespace LibraryServices.IdentityService.Services
{
    public interface IUserService : IServiceBase<User>
    {
        Task<List<Role>> GetUserRolesAsync(long userId);
    }

    public class UserService : ServiceBase<User>, IUserService
    {
        public UserService(IRepositoryBase<User> dbContext) : base(dbContext)
        {
        }

        public async Task<List<Role>> GetUserRolesAsync(long userId)
        {
            var user = await DAL.DbContext.Queryable<User>()
                  .Includes(u => u.Roles)
                  .InSingleAsync(userId);
            return user?.Roles!;
        }
    }
}
