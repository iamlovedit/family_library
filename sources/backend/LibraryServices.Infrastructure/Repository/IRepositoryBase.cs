﻿using SqlSugar;
using System.Linq.Expressions;

namespace LibraryServices.Infrastructure.Repository
{
    public interface IRepositoryBase<T> where T : class, new()
    {
        ISqlSugarClient DbContext { get; }

        Task<T> GetByIdAsync(long id);

        Task<List<T>> GetAllAsync();

        Task<T> GetFirstByExpressionAsync(Expression<Func<T, bool>> expression);

        Task<long> AddSnowflakeAsync(T entity);

        Task<IList<long>> AddSnowflakesAsync(IList<T> entities);

        Task<PageData<T>> QueryPageAsync(Expression<Func<T, bool>>? whereExpression, int pageIndex = 1, int pageSize = 20,
            Expression<Func<T, object>>? orderExpression = null, OrderByType orderByType = OrderByType.Asc);

        Task<bool> UpdateColumnsAsync(T entity, Expression<Func<T, object>> expression);
    }
}
