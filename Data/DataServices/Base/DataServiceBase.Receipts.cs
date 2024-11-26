//using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddReceiptAsync(Receipt model)
        {
            if (model == null)
                return 0;

            var entity = new Receipt()
            {
                ReceiptGuid = model.ReceiptGuid,
                PayeeId = model.PayeeId,
                DealId = model.DealId,
                PartyId = model.PartyId,
                PaymentTypeId = model.PaymentTypeId,
                DepositBankId = model.DepositBankId,
                DepositCashId = model.DepositCashId,
                DateOfPayment = model.DateOfPayment,
                Amount = model.Amount,
                Narration = model.Narration,
        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return entity.ReceiptId;
        }

        public async Task<Receipt> GetReceiptAsync(long id)
        {
            return await _dataSource.Receipts.Where(r => r.ReceiptId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Receipt>> GetReceiptsAsync(DataRequest<Receipt> request)
        {
            IQueryable<Receipt> items = GetReceipts(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Receipt>> GetReceiptsAsync(int skip, int take, DataRequest<Receipt> request)
        {
            IQueryable<Receipt> items = GetReceipts(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Receipt
                {
                    ReceiptId = source.ReceiptId,
                    ReceiptGuid = source.ReceiptGuid,
                    PayeeId = source.PayeeId,
                    DealId = source.DealId,
                    PartyId = source.PartyId,
                    PaymentTypeId = source.PaymentTypeId,
                    DepositBankId = source.DepositBankId,
                    DepositCashId = source.DepositCashId,
                    DateOfPayment = source.DateOfPayment,
                    Amount = source.Amount,
                    BankName=source.BankName,
                    Narration = source.Narration,
                    CashName=source.CashName
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Receipt> GetReceipts(DataRequest<Receipt> request)
        {

            IQueryable<Receipt> items = from r in _dataSource.Receipts  
                                   from  b in _dataSource.BankAccounts.Where(x=>x.BankAccountId==r.DepositBankId).DefaultIfEmpty()
                                   from  c in _dataSource.CashAccounts.Where(x=>x.CashAccountId==r.DepositCashId).DefaultIfEmpty()
                                        select (new Receipt
                                     {
                                         ReceiptId=r.ReceiptId,
                                         ReceiptGuid=r.ReceiptGuid,
                                         DealId = r.DealId,
                                        PayeeId=r.PayeeId,
                                        PaymentTypeId=r.PaymentTypeId,
                                        DateOfPayment=r.DateOfPayment,
                                        Amount=r.Amount,
                                        Narration=r.Narration,
                                         DepositBankId = r.DepositBankId,                                        
                                         DepositCashId = r.DepositCashId,                                        
                                         BankName = r.DepositBankId>0? b.BankName+" - "+b.AccountNumber: c.CashAccountName
                                        });
           // IQueryable<Receipt> items = _dataSource.Receipts;

            // Query
            if (!string.IsNullOrEmpty(request.Query))
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

        public async Task<int> GetReceiptsCountAsync(DataRequest<Receipt> request)
        {
            IQueryable<Receipt> items = from r in _dataSource.Receipts
                                        from b in _dataSource.BankAccounts.Where(x => x.BankAccountId == r.DepositBankId).DefaultIfEmpty()
                                        from c in _dataSource.CashAccounts.Where(x => x.CashAccountId == r.DepositCashId).DefaultIfEmpty()
                                        select (new Receipt
                                        {
                                            ReceiptId = r.ReceiptId,
                                            ReceiptGuid = r.ReceiptGuid,
                                            DealId = r.DealId,
                                            PayeeId = r.PayeeId,
                                            PaymentTypeId = r.PaymentTypeId,
                                            DateOfPayment = r.DateOfPayment,
                                            Amount = r.Amount,
                                            Narration = r.Narration,
                                            DepositBankId = r.DepositBankId,
                                            DepositCashId = r.DepositCashId,
                                            BankName = b.BankName + " - " + b.AccountNumber,
                                            CashName=c.CashAccountName
                                        });

            // Query
            if (!string.IsNullOrEmpty(request.Query))
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

        public async Task<int> UpdateReceiptAsync(Receipt model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteReceiptAsync(Receipt model)
        {
            _dataSource.Receipts.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

    }
}
