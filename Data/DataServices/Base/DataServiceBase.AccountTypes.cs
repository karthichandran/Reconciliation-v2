using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddAccountTypeAsync(AccountType model)
        {
            if (model == null)
                return 0;

            var entity = new AccountType()
            {
                AccountTypeId = model.AccountTypeId,
                AccountTypeName = model.AccountTypeName
            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<AccountType> GetAccountTypeAsync(long id)
        {
            return await _dataSource.AccountTypes
                 .Where(x => x.AccountTypeId == id).FirstOrDefaultAsync();

        }

        private IQueryable<AccountType> GetAccountTypes(DataRequest<AccountType> request)
        {
            IQueryable<AccountType> items = _dataSource.AccountTypes;

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

        public async Task<IList<AccountType>> GetAccountTypesAsync(DataRequest<AccountType> request)
        {
            IQueryable<AccountType> items = GetAccountTypes(request);
            return await items.ToListAsync();
        }

        public async Task<IList<AccountType>> GetAccountTypesAsync(int skip, int take, DataRequest<AccountType> request)
        {
            IQueryable<AccountType> items = GetAccountTypes(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new AccountType
                {
                    AccountTypeId = source.AccountTypeId,
                    AccountTypeName = source.AccountTypeName,
                 })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetAccountTypesCountAsync(DataRequest<AccountType> request)
        {
            IQueryable<AccountType> items = _dataSource.AccountTypes;

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

        public async Task<int> UpdateAccountTypeAsync(AccountType model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteAccountTypeAsync(AccountType model)
        {
            _dataSource.AccountTypes.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
