using LibraryServices.Domain.Models.FamilyParameter;
using LibraryServices.Infrastructure.Repository;

namespace LibraryServices.ParameterService.Services;

public interface IParameterDefinitionService:IServiceBase<ParameterDefinition>
{
    
}

public class ParameterDefinitionService : ServiceBase<ParameterDefinition>, IParameterDefinitionService
{
    public ParameterDefinitionService(IRepositoryBase<ParameterDefinition> dbContext) : base(dbContext)
    {
    }
}