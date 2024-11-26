using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPaymentAsync(Payment model)
        {
            if (model == null)
                return 0;
            try
            {
                var entity = new Payment()
                {
                    PaymentGuid = model.PaymentGuid,
                    PayeeId = model.PayeeId,
                    PayeeTypeId = model.PayeeTypeId,
                    ExpenseHeadId = model.ExpenseHeadId,
                    PropertyId = model.PropertyId,
                    PartyId = model.PartyId,
                    PaymentTypeId = model.PaymentTypeId,
                    DocumentTypeId = model.DocumentTypeId,
                    DateOfPayment = model.DateOfPayment,
                    Amount = model.Amount,
                    ChequeNo = model.ChequeNo,
                    Narration = model.Narration,
                    PDC = model.PDC,
                    BankAccountId = model.BankAccountId,
                    CashAccountId = model.CashAccountId,
                    GroupId=model.GroupId
                };
                //entity.Amount = model.PaymentLists.Sum(x => x.Amount);
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();

                //foreach (var pay in model.PaymentLists) {
                //    pay.PaymentId = entity.PaymentId;
                //    _dataSource.Entry(pay).State = EntityState.Added;
                //}
                //await  _dataSource.SaveChangesAsync();

                return entity.PaymentId;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<Payment> GetPaymentAsync(long id)
        {
            var payment= await _dataSource.Payments.Where(r => r.PaymentId == id).FirstOrDefaultAsync();

            if (_dataSource.paymentLists.Where(x => x.PaymentId == id).Count() > 0)
            {

                var list = (from p in _dataSource.Payments.Where(x => x.PaymentId == id)
                            from l in _dataSource.paymentLists.Where(x => x.PaymentId == p.PaymentId).DefaultIfEmpty()
                            from b in _dataSource.BankAccounts.Where(x => x.BankAccountId == l.BankAccountId).DefaultIfEmpty()
                            from c in _dataSource.CashAccounts.Where(x => x.CashAccountId == l.CashAccountId).DefaultIfEmpty()
                           
                            select new PaymentList
                            {
                                PaymentId = p.PaymentId,
                                PaymentListId = l.PaymentListId,
                                PaymentTypeId=l.PaymentTypeId,
                                AccountName = (l.PaymentTypeId) ? c.CashAccountName : b.BankName,
                                BankAccountId = p.BankAccountId.Value,
                                CashAccountId = p.CashAccountId.Value,
                                Amount = l.Amount,
                                Narration = l.Narration,
                                ChequeNo = l.ChequeNo,
                                PDC = l.PDC,
                                DateOfPayment = l.DateOfPayment
                            }).ToList();
           

            payment.PaymentLists = list;
            }
            return payment;
        }

        public async Task<IList<Payment>> GetPaymentsAsync(DataRequest<Payment> request)
        {
            IQueryable<Payment> items = GetPayments(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Payment>> GetPaymentsAsync(int skip, int take, DataRequest<Payment> request)
        {
            IQueryable<Payment> items = GetPayments(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Payment
                {
                    PaymentId = source.PaymentId,
                    PaymentGuid = source.PaymentGuid,
                    PayeeId = source.PayeeId,
                    PayeeTypeId = source.PayeeTypeId,
                    ExpenseHeadId = source.ExpenseHeadId,
                    PropertyId = source.PropertyId,
                    PartyId = source.PartyId,
                    PaymentTypeId = source.PaymentTypeId,
                    DocumentTypeId = source.DocumentTypeId,
                    DateOfPayment = source.DateOfPayment,
                    Amount = source.Amount,
                    ChequeNo = source.ChequeNo,
                    Narration = source.Narration,
                    PDC = source.PDC,
                    BankAccountId = source.BankAccountId,
                    CashAccountId = source.CashAccountId,
                    AccountName=source.AccountName,
                    CompanyName=source.CompanyName,
                   PropertyName=source.PropertyName,
                   DocumentTypeName=source.DocumentTypeName,
                    GroupId=source.GroupId
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Payment> GetPayments(DataRequest<Payment> request)
        {
            IQueryable<Payment> items = from pay in _dataSource.Payments
                                        from c in _dataSource.Companies.Where(x=>x.CompanyID==pay.PayeeId).DefaultIfEmpty()
                                        from p in _dataSource.Parties.Where(x => x.PartyId == pay.PartyId).DefaultIfEmpty()
                                        from e in _dataSource.ExpenseHeads.Where(x => x.ExpenseHeadId == pay.ExpenseHeadId).DefaultIfEmpty()
                                        from py in _dataSource.Properties.Where(x => x.PropertyId == pay.PropertyId).DefaultIfEmpty()
                                        from d in _dataSource.DocumentTypes.Where(x => x.DocumentTypeId == pay.DocumentTypeId).DefaultIfEmpty()
                                        from ch in _dataSource.CashAccounts.Where(x=>x.CashAccountId==pay.CashAccountId).DefaultIfEmpty()
                                        from bk in _dataSource.BankAccounts.Where(x=>x.BankAccountId==pay.BankAccountId).DefaultIfEmpty()
                                        select new Payment
                                        {
                                            PaymentId = pay.PaymentId,
                                            PaymentGuid = pay.PaymentGuid,
                                            PayeeId = pay.PayeeId,
                                            PayeeTypeId = pay.PayeeTypeId,
                                            ExpenseHeadId = pay.ExpenseHeadId,
                                            PropertyId = pay.PropertyId,
                                            PartyId = pay.PartyId,
                                            PaymentTypeId = pay.PaymentTypeId,
                                            DocumentTypeId = pay.DocumentTypeId,
                                            DateOfPayment = pay.DateOfPayment,
                                            Amount = pay.Amount,
                                            ChequeNo = pay.ChequeNo,
                                            Narration = pay.Narration,
                                            PDC = pay.PDC,
                                            BankAccountId = pay.BankAccountId,
                                            CashAccountId = pay.CashAccountId,
                                           // AccountName = (pay.PartyId > 0) ? p.PartyFirstName : e.ExpenseHeadName,
                                            AccountName = (pay.PaymentTypeId ==1) ? ch.CashAccountName : bk.BankName,
                                            CompanyName=c.Name,
                                            DocumentTypeName=d.DocumentTypeName,
                                            PropertyName=py.PropertyName,
                                            GroupId=pay.GroupId??0
                                        };
            //  IQueryable<Payment> items =  _dataSource.Payments;
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

        public async Task<int> GetPaymentsCountAsync(DataRequest<Payment> request)
        {           
           
            IQueryable<Payment> items = _dataSource.Payments;
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

        public async Task<int> UpdatePaymentAsync(Payment model)
        {
           // model.Amount = model.PaymentLists.Sum(x => x.Amount);
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();

            //if (model.PaymentLists == null || model.PaymentLists.Count == 0)
            //    return res;
            //foreach (var pay in model.PaymentLists)
            //{
            //    pay.PaymentId = model.PaymentId;
            //    _dataSource.Entry(pay).State = EntityState.Added;
            //}
            //await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeletePaymentAsync(Payment model)
        {
            _dataSource.Payments.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeletePaymentListAsync(PaymentList model)
        {
            var entity = _dataSource.paymentLists.Where(x => x.PaymentListId == model.PaymentListId).FirstOrDefault();
            if (entity != null)
            {
                _dataSource.paymentLists.Remove(entity);
                return await _dataSource.SaveChangesAsync();
            }
            return 0;
        }

    }
}
