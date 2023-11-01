using LibraryServices.Domain.Models.Dynamo;
using LibraryServices.Infrastructure.Repository;

namespace LibraryServices.PackageService.Services
{
    public interface IPackageService : IServiceBase<Package>
    {
        Task<Package> GetPackageDetailById(string id);
    }

    public class PackageService : ServiceBase<Package>, IPackageService
    {
        public PackageService(IRepositoryBase<Package> dbContext) : base(dbContext)
        {
        }

        public async Task<Package> GetPackageDetailById(string id)
        {
            return await DAL.DbContext.Queryable<Package>()
                 .Includes(p => p.Versions)
                 .InSingleAsync(id);
        }
    }
}
