using LibraryServices.Domain.Models.FamilyLibrary;
using LibraryServices.Infrastructure;
using LibraryServices.Infrastructure.Repository;
using SqlSugar;
using SqlSugar.Extensions;
using System.Linq.Expressions;

namespace LibraryServices.FamilyService.Services;

public interface IFamilyService : IServiceBase<Family>
{
    Task<IList<FamilyCategory>> GetCategoryTreeAsync(int? rootId);
    Task<Family> GetFamilyDetails(long id);
    Task<PageData<Family>> GetFamilyPageAsync(Expression<Func<Family, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFields = null);
}

public class FamilyService : ServiceBase<Family>, IFamilyService
{
    public FamilyService(IRepositoryBase<Family> dbContext) : base(dbContext)
    {
    }

    public async Task<Family> GetFamilyDetails(long id)
    {
        return await DAL.DbContext.Queryable<Family>().
               Includes(f => f.Category).
               Includes(f => f.Symbols, s => s.Parameters, s => s.DisplayUnitType).
               InSingleAsync(id);
    }
    public async Task<PageData<Family>> GetFamilyPageAsync(Expression<Func<Family, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderByFields = null)
    {
        RefAsync<int> totalCount = 0;
        var list = await DAL.DbContext.Queryable<Family>()
            .Includes(f => f.Uploader)
            .Includes(f => f.Category)
            .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageListAsync(pageIndex, pageSize, totalCount);
        var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
        return new PageData<Family>(pageIndex, pageCount, totalCount, pageSize, list);
    }
    public async Task<IList<FamilyCategory>> GetCategoryTreeAsync(int? rootId)
    {
        if (rootId.HasValue)
        {
            return await DAL.DbContext.Queryable<FamilyCategory>()
                .Where(fc => fc.ParentId == rootId)
                .Includes(c => c.Parent)
                .ToTreeAsync(c => c.Children, t => t.ParentId, rootId);
        }
        return await DAL.DbContext.Queryable<FamilyCategory>()
            .Includes(c => c.Parent)
            .ToTreeAsync(c => c.Children, t => t.ParentId, null);
    }
}