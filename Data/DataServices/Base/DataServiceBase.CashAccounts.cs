using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddCashAccountAsync(CashAccount model)
        {
           
                if (model == null)
                    return 0;

                var entity = new CashAccount()
                {
                    CashAccountGuid = model.CashAccountGuid,
                    AccountTypeId = 1,
                    CashAccountName = model.CashAccountName,
                    IsCashAccountActive = model.IsCashAccountActive,
                    CompanyID=model.CompanyID,
                    OpeningBalance=model.OpeningBalance
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();
                return res;
           
        }

        public async Task<CashAccount> GetCashAccountAsync(long id)
        {
            return await _dataSource.CashAccounts
                .Where(x => x.CashAccountId == id).FirstOrDefaultAsync();

        }

        public async Task<IList<CashAccount>> GetCashAccountsAsync(DataRequest<CashAccount> request)
        {
            IQueryable<CashAccount> items = GetCashAccounts(request);
            return await items.ToListAsync();
        }

        private IQueryable<CashAccount> GetCashAccounts(DataRequest<CashAccount> request)
        {
            IQueryable<CashAccount> items = from cash in _dataSource.CashAccounts join 
                                            c in _dataSource.Companies on cash.CompanyID equals c.CompanyID
                                            select (new CashAccount
                                            {
                                                CashAccountId = cash.CashAccountId,
                                                CashAccountGuid = cash.CashAccountGuid,
                                                AccountTypeId = cash.AccountTypeId,
                                                CashAccountName = cash.CashAccountName,
                                                IsCashAccountActive = cash.IsCashAccountActive,
                                                CompanyID = cash.CompanyID,
                                                CompanyName=c.Name,
                                                OpeningBalance=cash.OpeningBalance
                                            });

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


        public async Task<IList<CashAccount>> GetCashAccountsAsync(int skip, int take, DataRequest<CashAccount> request)
        {
            IQueryable<CashAccount> items = GetCashAccounts(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new CashAccount
                {
                    CashAccountId = source.CashAccountId,
                    CashAccountGuid = source.CashAccountGuid,
                    AccountTypeId = source.AccountTypeId,
                    CashAccountName = source.CashAccountName,
                    IsCashAccountActive = source.IsCashAccountActive,
                    CompanyID = source.CompanyID,
                    CompanyName=source.CompanyName,
                    OpeningBalance=source.OpeningBalance
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetCashAccountsCountAsync(DataRequest<CashAccount> request)
        {
            IQueryable<CashAccount> items = from cash in _dataSource.CashAccounts
                                            join c in _dataSource.Companies on cash.CompanyID equals c.CompanyID
                                            select cash;

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

        public async Task<int> UpdateCashAccountAsync(CashAccount model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteCashAccountAsync(CashAccount model)
        {
            _dataSource.CashAccounts.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
