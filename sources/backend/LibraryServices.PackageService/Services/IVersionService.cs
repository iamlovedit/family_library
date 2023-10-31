using LibraryServices.Domain.Models.Dynamo;
using LibraryServices.Infrastructure.Repository;

namespace LibraryServices.PackageService.Services
{
    public interface IVersionService : IServiceBase<PackageVersion>
    {
    }
    public class VersionService : ServiceBase<PackageVersion>, IVersionService
    {
        public VersionService(IRepositoryBase<PackageVersion> dbContext) : base(dbContext)
        {
        }
    }
}
