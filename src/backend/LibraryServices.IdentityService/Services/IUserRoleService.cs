using LibraryServices.Domain.Models.Identity;
using LibraryServices.Infrastructure.Repository;

namespace LibraryServices.IdentityService.Services
{
    public interface IUserRoleService : IServiceBase<UserRole>
    {

    }

    public class UserRoleService : ServiceBase<UserRole>, IUserRoleService
    {
        public UserRoleService(IRepositoryBase<UserRole> dbContext) : base(dbContext)
        {
        }
    }
}
