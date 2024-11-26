using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
   partial class DataServiceBase
    {
        public async Task<int> AddFundTransferAsync(FundTransfer model)
        {
            if (model == null)
                return 0;
            try
            {
                var entity = new FundTransfer()
                {
                    FundTransferGuid = model.FundTransferGuid,
                    PayeeId = model.PayeeId,
                    PayeePaymentType = model.PayeePaymentType,
                    PayeeBankId = model.PayeeBankId,
                    PayeeCashId=model.PayeeCashId,
                    DateOfPayment = model.DateOfPayment,
                    Amount = model.Amount,
                    Narration = model.Narration,
                    ReceiverId = model.ReceiverId,
                    ReceiverPaymentType = model.ReceiverPaymentType,
                    ReceiverBankId = model.ReceiverBankId,
                    ReceiverCashId=model.ReceiverCashId
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                 await _dataSource.SaveChangesAsync();
                return entity.FundTransferId;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        private IQueryable<FundTransfer> GetFundTransfers(DataRequest<FundTransfer> request)
        {
            IQueryable<FundTransfer> items = from fund in _dataSource.FundTransfers
                                             from fromComp in _dataSource.Companies.Where(x => x.CompanyID == fund.PayeeId).DefaultIfEmpty()
                                             from fromBank in _dataSource.BankAccounts.Where(x => x.BankAccountId == fund.PayeeBankId).DefaultIfEmpty()
                                             from fromCash in _dataSource.CashAccounts.Where(x => x.CashAccountId == fund.PayeeCashId).DefaultIfEmpty()
                                             from toComp in _dataSource.Companies.Where(x => x.CompanyID == fund.ReceiverId).DefaultIfEmpty()
                                             from toBank in _dataSource.BankAccounts.Where(x => x.BankAccountId == fund.ReceiverBankId).DefaultIfEmpty()
                                             from toCash in _dataSource.CashAccounts.Where(x => x.CashAccountId == fund.ReceiverCashId).DefaultIfEmpty()
                                             select new FundTransfer
                                             {
                                                 FundTransferId = fund.FundTransferId,
                                                 FundTransferGuid = fund.FundTransferGuid,
                                                 PayeeId = fund.PayeeId,
                                                 PayeePaymentType = fund.PayeePaymentType,
                                                 PayeeBankId = fund.PayeeBankId,
                                                 DateOfPayment = fund.DateOfPayment,
                                                 Amount = fund.Amount,
                                                 Narration = fund.Narration,
                                                 ReceiverId = fund.ReceiverId,
                                                 ReceiverPaymentType = fund.ReceiverPaymentType,
                                                 ReceiverBankId = fund.ReceiverBankId,
                                                 FromAccountName = fund.PayeeBankId>0? fromBank.BankName+" - "+fromBank.AccountNumber: fromCash.CashAccountName,
                                                 FromCompanyName = fromComp.Name,
                                                 ToAccountName = fund.ReceiverBankId>0? toBank.BankName+" - "+toBank.AccountNumber:toCash.CashAccountName,
                                                 ToCompanyName = toComp.Name
                                             };
          

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


        public async Task<FundTransfer> GetFundTransferAsync(long id)
        {
            return await _dataSource.FundTransfers
                .Where(x => x.FundTransferId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<FundTransfer>> GetFundTransfersAsync(DataRequest<FundTransfer> request)
        {
            IQueryable<FundTransfer> items = GetFundTransfers(request);
            return await items.ToListAsync();
        }

        public async Task<IList<FundTransfer>> GetFundTransfersAsync(int skip, int take, DataRequest<FundTransfer> request)
        {
            IQueryable<FundTransfer> items = GetFundTransfers(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new FundTransfer
                {
                    FundTransferId = source.FundTransferId,
                    FundTransferGuid = source.FundTransferGuid,
                    PayeeId = source.PayeeId,
                    PayeePaymentType = source.PayeePaymentType,
                    PayeeBankId = source.PayeeBankId,
                    PayeeCashId=source.PayeeCashId,
                    DateOfPayment = source.DateOfPayment,
                    Amount = source.Amount,
                    Narration = source.Narration,
                    ReceiverId = source.ReceiverId,
                    ReceiverPaymentType = source.ReceiverPaymentType,
                    ReceiverBankId = source.ReceiverBankId,
                    ReceiverCashId=source.ReceiverCashId,
                    FromAccountName=source.FromAccountName,
                    FromCompanyName=source.FromCompanyName,
                    ToAccountName=source.ToAccountName,
                    ToCompanyName=source.ToCompanyName
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetFundTransfersCountAsync(DataRequest<FundTransfer> request)
        {
            IQueryable<FundTransfer> items = _dataSource.FundTransfers;
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

        public async Task<int> UpdateFundTransferAsync(FundTransfer model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteFundTransferAsync(FundTransfer model)
        {
            _dataSource.FundTransfers.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
