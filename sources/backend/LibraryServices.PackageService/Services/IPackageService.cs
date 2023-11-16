using LibraryServices.Domain.Models.Dynamo;
using LibraryServices.Infrastructure;
using LibraryServices.Infrastructure.Repository;
using SqlSugar;
using SqlSugar.Extensions;
using System.Linq.Expressions;

namespace LibraryServices.PackageService.Services
{
    public interface IPackageService : IServiceBase<Package>
    {
        Task<Package> GetPackageDetailByIdAsync(string id);
        Task<PageData<Package>> GetPackagePageAsync(Expression<Func<Package, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderBy = null);
    }

    public class PackageService : ServiceBase<Package>, IPackageService
    {
        public PackageService(IRepositoryBase<Package> dbContext) : base(dbContext)
        {
        }

        public async Task<Package> GetPackageDetailByIdAsync(string id)
        {
            return await DAL.DbContext.Queryable<Package>()
                 .Includes(p => p.Versions)
                 .InSingleAsync(id);
        }

        public async Task<PageData<Package>> GetPackagePageAsync(Expression<Func<Package, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20, string? orderBy = null)
        {
            var orderModels = default(List<OrderByModel>);
            if (!string.IsNullOrEmpty(orderBy))
            {
                var fieldName = DAL.DbContext.EntityMaintenance.GetDbColumnName<Package>(orderBy);
                orderModels = OrderByModel.Create(new OrderByModel() { FieldName = fieldName, OrderByType = OrderByType.Desc });
            }
            RefAsync<int> totalCount = 0;
            var packages = await DAL.DbContext.Queryable<Package>()
                   .WhereIF(whereExpression != null, whereExpression)
                   .OrderBy(orderModels)
                   .ToPageListAsync(pageIndex, pageSize, totalCount);
            var pageCount = Math.Ceiling(totalCount.ObjToDecimal() / pageSize.ObjToDecimal()).ObjToInt();
            return new PageData<Package>(pageIndex, pageCount, totalCount, pageSize, packages);
        }
    }
}
