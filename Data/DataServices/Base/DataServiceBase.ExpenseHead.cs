using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {

        public async Task<int> AddExpenseHeadAsync(ExpenseHead expense)
        {
                if (expense == null)
                    return 0;

                var entity = new ExpenseHead()
                {
                    ExpenseHeadGuid=expense.ExpenseHeadGuid,
                    ExpenseHeadName=expense.ExpenseHeadName,
                    IsExpenseHeadActive=expense.IsExpenseHeadActive
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();
                return res;
           
         
        }

        public async Task<ExpenseHead> GetExpenseHeadAsync(long id)
        {
            return await _dataSource.ExpenseHeads.Where(r => r.ExpenseHeadId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<ExpenseHead>> GetExpenseHeadsAsync(int skip, int take, DataRequest<ExpenseHead> request)
        {
            IQueryable<ExpenseHead> items = GetExpenseHead(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new ExpenseHead
                {
                    ExpenseHeadId=source.ExpenseHeadId,
                    ExpenseHeadGuid=source.ExpenseHeadGuid,
                    ExpenseHeadName=source.ExpenseHeadName,
                    IsExpenseHeadActive=source.IsExpenseHeadActive
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<ExpenseHead> GetExpenseHead(DataRequest<ExpenseHead> request)
        {
            IQueryable<ExpenseHead> items = _dataSource.ExpenseHeads;

            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.BuildSearchTerms().Contains(request.Query.ToLower()));
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            // Order By
            if (request.OrderBy != null)
            {
                items = items.OrderBy(request.OrderBy);
            }
            if (request.OrderByDesc != null)
            {
                items = items.OrderByDescending(request.OrderByDesc);
            }

            return items;
        }

       
        public async Task<int> GetExpenseHeadsCountAsync(DataRequest<ExpenseHead> request)
        {
            IQueryable<ExpenseHead> items = _dataSource.ExpenseHeads;

            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.BuildSearchTerms().Contains(request.Query.ToLower()));
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            return await items.CountAsync();
        }

        public async Task<int> UpdateExpenseHeadAsync(ExpenseHead expense)
        {
            if (expense.ExpenseHeadId > 0)
            {
                _dataSource.Entry(expense).State = EntityState.Modified;
            }
            else
            {
                expense.ExpenseHeadGuid = Guid.NewGuid();
                //Company.CreatedOn = DateTime.UtcNow;
                _dataSource.Entry(expense).State = EntityState.Added;
            }
            // Company.LastModifiedOn = DateTime.UtcNow;
           // expense.SearchTerms = expense.BuildSearchTerms();
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteExpenseHeadAsync(ExpenseHead model)
        {
            _dataSource.ExpenseHeads.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteExpenseHeadAsync(params ExpenseHead[] heads)
        {
            _dataSource.ExpenseHeads.RemoveRange(heads);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
