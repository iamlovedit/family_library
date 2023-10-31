using LibraryServices.Domain.Models.FamilyLibrary;
using LibraryServices.Infrastructure.Repository;

namespace LibraryServices.FamilyService.Services;

public interface IFamilyService : IServiceBase<Family>
{
}

public class FamilyService : ServiceBase<Family>, IFamilyService
{
    public FamilyService(IRepositoryBase<Family> dbContext) : base(dbContext)
    {
    }
}