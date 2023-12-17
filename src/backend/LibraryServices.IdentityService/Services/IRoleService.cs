using LibraryServices.Domain.Models.Identity;
using LibraryServices.Infrastructure.Repository;

namespace LibraryServices.IdentityService.Services
{
    public interface IRoleService : IServiceBase<Role>
    {

    }

    public class RoleService : ServiceBase<Role>, IRoleService
    {
        public RoleService(IRepositoryBase<Role> dbContext) : base(dbContext)
        {
        }
    }
}
