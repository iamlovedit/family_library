using LibraryServices.Domain.Models.FamilyParameter;
using LibraryServices.Infrastructure.Repository;

namespace LibraryServices.ParameterService.Services;

public interface IParameterService:IServiceBase<Parameter>
{
    
}

public class ParameterService : ServiceBase<Parameter>, IParameterService
{
    public ParameterService(IRepositoryBase<Parameter> dbContext) : base(dbContext)
    {
    }
}