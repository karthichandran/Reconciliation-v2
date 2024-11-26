using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPropertyMergeAsync(PropertyMerge model)
        {
            if (model == null)
                return 0;
            try
            {
                var entity = new PropertyMerge()
                {
                    PropertyMergeGuid = model.PropertyMergeGuid,
                    PropertyMergeDealName = model.PropertyMergeDealName,
                    MergedTotalArea = model.MergedTotalArea,
                    MergedSaleValue1 = model.MergedSaleValue1,
                    MergedSaleValue2 = model.MergedSaleValue2,
                    MergedAmountPaid1 = model.MergedAmountPaid1,
                    MergedAmountPaid2 = model.MergedAmountPaid2,
                    MergedBalancePayable1 = model.MergedBalancePayable1,
                    MergedBalancePayable2 = model.MergedBalancePayable2,
                    ForProposal = model.ForProposal,
                   FormattedTotalArea=model.FormattedTotalArea
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();

                foreach (var item in model.propertyMergeLists)
                {
                    _dataSource.Entry(item).State = EntityState.Added;
                }
                await _dataSource.SaveChangesAsync();

                return entity.PropertyMergeId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PropertyMergeList> GetPropertyListItemForProeprty(int propertyId,int DocumentTypeId)
        {

            //var partyname = await (from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == id)
            //                       from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId)
            //                      select party.PartyFirstName).ToListAsync();

            bool isOld = (from p in _dataSource.Properties join pm in _dataSource.PropertyMergeList on p.PropertyGuid 
                          equals pm.PropertyGuid join pd in _dataSource.PropertyDocumentType on pm.PropertyDocumentTypeId equals pd.PropertyDocumentTypeId 
                          join dt in _dataSource.DocumentTypes on pd.DocumentTypeId equals dt.DocumentTypeId
                          where dt.DocumentTypeId == DocumentTypeId select p.PropertyId).Any();

            var partyName = "";
            var propertyparty =await _dataSource.PropertyParty.Where(x => x.PropertyId == propertyId).ToListAsync();
            if (propertyparty != null&& propertyparty.Count !=0) {
                if (propertyparty.Count == 1)
                    partyName = _dataSource.Parties.Where(x => x.PartyId == propertyparty[0].PartyId).Select(s => s.PartyFirstName).First();
                else
                    partyName = _dataSource.Parties.Where(x => x.PartyId == propertyparty.Where(p=>p.IsPrimaryParty==true).Select(s=>s.PartyId).First()).Select(s => s.PartyFirstName).First();
            }

            var item = await (from pt in _dataSource.Properties.Where(x => x.PropertyId == propertyId) join
                               v in _dataSource.Villages on pt.VillageId equals v.VillageId join
                              pdt in _dataSource.PropertyDocumentType on pt.PropertyId equals pdt.PropertyId join
                              pd in _dataSource.DocumentTypes on pdt.DocumentTypeId equals pd.DocumentTypeId
                              where pdt.DocumentTypeId==DocumentTypeId
                              select new PropertyMergeList
                              {
                                  PropertyGuid = pt.PropertyGuid,
                                  PropertyDocumentTypeId=pdt.PropertyDocumentTypeId,
                                  PropertyName = pt.PropertyName,
                                  DocumentTypeName=pd.DocumentTypeName,
                                  Village = v.VillageName,
                                  SurveyNo = pt.SurveyNo,
                                  LandArea = CalculateArea(pdt),
                                  SaleValue1 = pdt.SaleValue1.ToString(),
                                  SaleValue2 = pdt.SaleValue2.ToString(),
                                  Amount1 = _dataSource.Payments.Where(x => x.PropertyId == pt.PropertyId && x.PayeeTypeId==2 && x.PaymentTypeId==1 && x.DocumentTypeId==DocumentTypeId).Sum(x => x.Amount).ToString(),
                                  Amount2 = _dataSource.Payments.Where(x => x.PropertyId == pt.PropertyId && x.PayeeTypeId == 2 && x.PaymentTypeId == 2 && x.DocumentTypeId == DocumentTypeId && x.PDC == false).Sum(x => x.Amount).ToString(),
                                  Expense = _dataSource.Payments.Where(x => x.PropertyId == pt.PropertyId && x.PayeeTypeId == 1 ).Sum(x => x.Amount).ToString(),
                                  Party = string.Join(",", partyName),
                                  IsOld=isOld
                              }).FirstOrDefaultAsync();

            item.Balance1 =(Convert.ToDecimal( item.SaleValue1) - Convert.ToDecimal(item.Amount1)).ToString();
            item.Balance2 = (Convert.ToDecimal(item.SaleValue2) - Convert.ToDecimal(item.Amount2)).ToString();
            return item;

        }

        private string CalculateArea(PropertyDocumentType pt) {
            var area = pt.LandAreaInputAcres + pt.AKarabAreaInputAcres + pt.BKarabAreaInputAcres;
            var guntas = pt.LandAreaInputGuntas + pt.AKarabAreaInputGuntas + pt.BKarabAreaInputGuntas;
            var anas = pt.LandAreaInputAanas + pt.AKarabAreaInputAanas + pt.BKarabAreaInputAanas;

            return area + " - " + guntas + " - " + anas;
        }
        public async Task<PropertyMerge> GetPropertyMergeAsync(long id)
        {
            var merge = await _dataSource.PropertyMerge.Where(r => r.PropertyMergeId == id).FirstOrDefaultAsync();

            var dealPrepared = await _dataSource.Deal.Where(x => x.PropertyMergeId == merge.PropertyMergeId).FirstOrDefaultAsync();
           
            if (dealPrepared != null)
                merge.IsSold = true;

            if (_dataSource.PropertyMergeList.Where(x => x.PropertyMergeGuid == merge.PropertyMergeGuid).Count() > 0)
            {

                var list = (from pm in _dataSource.PropertyMergeList.Where(x => x.PropertyMergeGuid == merge.PropertyMergeGuid)
                            from pt in _dataSource.Properties.Where(x => x.PropertyGuid == pm.PropertyGuid).DefaultIfEmpty()
                            from c in _dataSource.Companies.Where(x => x.CompanyID == pt.CompanyID).DefaultIfEmpty()
                            from v in _dataSource.Villages.Where(x => x.VillageId == pt.VillageId).DefaultIfEmpty()
                            from pdt in _dataSource.PropertyDocumentType.Where(x=>x.PropertyId==pt.PropertyId && x.PropertyDocumentTypeId==pm.PropertyDocumentTypeId).DefaultIfEmpty()
                            from pd in _dataSource.DocumentTypes.Where(x=>x.DocumentTypeId==pdt.DocumentTypeId).DefaultIfEmpty()
                            select new PropertyMergeList
                            {
                                PropertyMergeListId = pm.PropertyMergeListId,
                                PropertyMergeGuid = merge.PropertyMergeGuid,
                                PropertyGuid = pm.PropertyGuid,
                                PropertyName = pt.PropertyName,
                                DocumentTypeName=pd.DocumentTypeName,
                                PropertyDocumentTypeId=pd.DocumentTypeId,
                                Village = v.VillageName,
                                SurveyNo = pt.SurveyNo,
                                LandArea = CalculateArea(pdt),
                                SaleValue1 = pdt.SaleValue1.ToString(),
                                SaleValue2 = pdt.SaleValue2.ToString(),
                                Amount1 = _dataSource.Payments.Where(x => x.PropertyId == pt.PropertyId && x.PayeeTypeId == 2 && x.PaymentTypeId == 1 && x.DocumentTypeId == pdt.DocumentTypeId).Sum(x => x.Amount).ToString(),
                                Amount2 = _dataSource.Payments.Where(x => x.PropertyId == pt.PropertyId && x.PayeeTypeId == 2 && x.PaymentTypeId == 2 && x.DocumentTypeId == pdt.DocumentTypeId && x.PDC==false).Sum(x => x.Amount).ToString(),
                                Expense = _dataSource.Payments.Where(x => x.PropertyId == pt.PropertyId && x.PayeeTypeId == 1).Sum(x => x.Amount).ToString(),
                                companyId=pt.CompanyID.ToString(),
                                propertyId=pt.PropertyId.ToString()
                            }).ToList();

                foreach (var item in list) {
                    item.Balance1 = (Convert.ToDecimal(item.SaleValue1) - Convert.ToDecimal(item.Amount1)).ToString();
                    item.Balance2 = (Convert.ToDecimal(item.SaleValue2) - Convert.ToDecimal(item.Amount2)).ToString();
                    var propertyparty = await _dataSource.PropertyParty.Where(x => x.PropertyGuid == item.PropertyGuid).ToListAsync();
                    if (propertyparty != null && propertyparty.Count>0)
                    {
                        if (propertyparty.Count == 1)
                            item.Party  = _dataSource.Parties.Where(x => x.PartyId == propertyparty[0].PartyId).Select(s => s.PartyFirstName).First();
                        else
                            item.Party = _dataSource.Parties.Where(x => x.PartyId == propertyparty.Where(p => p.IsPrimaryParty == true).Select(s => s.PartyId).First()).Select(s => s.PartyFirstName).First();
                    }

                   
                }

                merge.propertyMergeLists = list;
            }
            return merge;
        }

        public async Task<IList<PropertyMerge>> GetPropertyMergeAsync(DataRequest<PropertyMerge> request)
        {
            IQueryable<PropertyMerge> items = GetPropertyMerge(request);
            return await items.ToListAsync();
        }

        public async Task<IList<PropertyMerge>> GetPropertyMergeAsync(int skip, int take, DataRequest<PropertyMerge> request)
        {
            IQueryable<PropertyMerge> items = GetPropertyMerge(request);
            var records = await items.Skip(skip).Take(take)
                .Select(x => x)
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<PropertyMerge> GetPropertyMerge(DataRequest<PropertyMerge> request)
        {

            // IQueryable<PropertyMerge> items = _dataSource.PropertyMerge;         
            IQueryable<PropertyMerge> items = (from p in _dataSource.PropertyMerge
                                               from d in _dataSource.Deal.Where(d => d.PropertyMergeId == p.PropertyMergeId).DefaultIfEmpty()
                                               select new PropertyMerge
                                               {
                                                   PropertyMergeId = p.PropertyMergeId,
                                                   PropertyMergeGuid = p.PropertyMergeGuid,
                                                   PropertyMergeDealName = p.PropertyMergeDealName,
                                                   MergedTotalArea = p.MergedTotalArea,
                                                   MergedSaleValue1 = p.MergedSaleValue1,
                                                   MergedSaleValue2 = p.MergedSaleValue2,
                                                   MergedAmountPaid1 = p.MergedAmountPaid1,
                                                   MergedAmountPaid2 = p.MergedAmountPaid2,
                                                   MergedBalancePayable1 = p.MergedBalancePayable1,
                                                   MergedBalancePayable2 = p.MergedBalancePayable2,
                                                   ForProposal = p.ForProposal,
                                                   FormattedTotalArea=p.FormattedTotalArea,
                                                   IsSold = d == null ? false : true
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

        public async Task<int> GetPropertyMergeCountAsync(DataRequest<PropertyMerge> request)
        {

            IQueryable<PropertyMerge> items = (from p in _dataSource.PropertyMerge
                                               from d in _dataSource.Deal.Where(d => d.PropertyMergeId == p.PropertyMergeId).DefaultIfEmpty()
                                               select new PropertyMerge
                                               {
                                                   PropertyMergeId = p.PropertyMergeId,
                                                   PropertyMergeGuid = p.PropertyMergeGuid,
                                                   PropertyMergeDealName = p.PropertyMergeDealName,
                                                   MergedTotalArea = p.MergedTotalArea,
                                                   MergedSaleValue1 = p.MergedSaleValue1,
                                                   MergedSaleValue2 = p.MergedSaleValue2,
                                                   MergedAmountPaid1 = p.MergedAmountPaid1,
                                                   MergedAmountPaid2 = p.MergedAmountPaid2,
                                                   MergedBalancePayable1 = p.MergedBalancePayable1,
                                                   MergedBalancePayable2 = p.MergedBalancePayable2,
                                                   ForProposal = p.ForProposal,
                                                   FormattedTotalArea = p.FormattedTotalArea,
                                                   IsSold = d == null ? false : true
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

            return await items.CountAsync();
        }

        public async Task<int> UpdatePropertyMergeAsync(PropertyMerge model)
        {
            try
            {
                _dataSource.Entry(model).State = EntityState.Modified;
                int res = await _dataSource.SaveChangesAsync();

                foreach (var pay in model.propertyMergeLists)
                {
                    _dataSource.Entry(pay).State = EntityState.Added;
                }
                await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
            }

        public async Task<int> DeletePropertyMergeAsync(PropertyMerge model)
        {
            try
            {
                var dealPrepared = await _dataSource.Deal.Where(x => x.PropertyMergeId == model.PropertyMergeId).FirstOrDefaultAsync();

                if (dealPrepared != null)
                    return 0;

                var entities = _dataSource.PropertyMergeList.Where(x => x.PropertyMergeGuid == model.PropertyMergeGuid);
                if (entities != null)
                {
                    _dataSource.PropertyMergeList.RemoveRange(entities);
                     await _dataSource.SaveChangesAsync();
                }

                _dataSource.PropertyMerge.Remove(model);
                return await _dataSource.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeletePropertyMergeItemAsync(int id)
        {
            try
            {
                var entity = _dataSource.PropertyMergeList.Where(x => x.PropertyMergeListId == id).FirstOrDefault();
                if (entity != null)
                {
                    _dataSource.PropertyMergeList.Remove(entity);
                    return await _dataSource.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

    }

}
