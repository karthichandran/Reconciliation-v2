using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
   partial class DataServiceBase
    {
        public async Task<int> AddPropertyCheckListAsync(PropertyCheckList model)
        {
            if (model == null)
                return 0;
            try
            {
               // ICollection<PropertyCheckListDocuments> docs = model.PropertyCheckListDocuments;
                ICollection<PropertyCheckListVendor> vendors = model.PropertyCheckListVendors;
                ICollection<CheckListOfProperty> checklist = model.CheckListOfProperties;

                var entity = new PropertyCheckList()
                {
                    PropertyGuid = model.PropertyGuid,
                    PropertyName = model.PropertyName,
                    TalukId = model.TalukId,
                    HobliId = model.HobliId,
                    VillageId = model.VillageId,
                    DocumentTypeId = model.DocumentTypeId,
                    PropertyTypeId = model.PropertyTypeId,
                    SurveyNo = model.SurveyNo,
                    PropertyGMapLink = model.PropertyGMapLink,
                    LandAreaInputAcres = model.LandAreaInputAcres,
                    LandAreaInputGuntas = model.LandAreaInputGuntas,
                    LandAreaInputAanas = model.LandAreaInputAanas,
                    LandAreaInAcres = model.LandAreaInAcres,
                    LandAreaInGuntas = model.LandAreaInGuntas,
                    LandAreaInSqMts = model.LandAreaInSqMts,
                    LandAreaInSqft = model.LandAreaInSqft,
                    AKarabAreaInputAcres = model.AKarabAreaInputAcres,
                    AKarabAreaInputGuntas = model.AKarabAreaInputGuntas,
                    AKarabAreaInputAanas = model.AKarabAreaInputAanas,
                    AKarabAreaInAcres = model.AKarabAreaInAcres,
                    AKarabAreaInGuntas = model.AKarabAreaInGuntas,
                    AKarabAreaInSqMts = model.AKarabAreaInSqMts,
                    AKarabAreaInSqft = model.AKarabAreaInSqft,
                    BKarabAreaInputAcres = model.BKarabAreaInputAcres,
                    BKarabAreaInputGuntas = model.BKarabAreaInputGuntas,
                    BKarabAreaInputAanas = model.BKarabAreaInputAanas,
                    BKarabAreaInAcres = model.BKarabAreaInAcres,
                    BKarabAreaInGuntas = model.BKarabAreaInGuntas,
                    BKarabAreaInSqMts = model.BKarabAreaInSqMts,
                    BKarabAreaInSqft = model.BKarabAreaInSqft,
                    CompanyID = model.CompanyID,
                    CheckListMaster=model.CheckListMaster,
                    PropertyDescription=model.PropertyDescription                   
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                await _dataSource.SaveChangesAsync();
                int res = entity.PropertyCheckListId;

               
                if (vendors != null) {
                    foreach (var vendor in vendors)
                    {
                        if (vendor.CheckListVendorId == 0)
                        {
                            vendor.PropertyCheckListId = res;
                            _dataSource.PropertyCheckListVendor.Add(vendor);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();

                if (checklist != null)
                {
                    foreach (var check in checklist)
                    {
                        if (check.CheckListPropertyId == 0)
                        {
                            check.PropertyCheckListId = res;
                            _dataSource.CheckListOfProperty.Add(check);
                        }
                        await _dataSource.SaveChangesAsync();

                        var docs = check.Documents;
                        if (docs != null)
                        {
                            foreach (var doc in docs)
                            {
                                if (doc.PropertyCheckListBlobId == 0)
                                {
                                    doc.CheckListPropertyId = check.CheckListPropertyId;
                                    doc.PropertyGuid = model.PropertyGuid;
                                    _dataSource.PropertyCheckListDocuments.Add(doc);
                                }
                            }
                        }
                       
                    }
                }
                await _dataSource.SaveChangesAsync();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PropertyCheckList> GetPropertyCheckListAsync(long id)
        {
            var property = await _dataSource.PropertyCheckList.Where(r => r.PropertyCheckListId == id).FirstOrDefaultAsync();

            if (property.PropertyGuid != null)
            {
                var docs =await GetPropertyCheckListDocumentsAsync(property.PropertyGuid);
                if (docs.Any())
                {
                    property.PropertyCheckListDocuments = docs;
                }
                var vendors = GetPropertyCheckListVendors(property.PropertyCheckListId);
                if (vendors.Any()) {
                    property.PropertyCheckListVendors = vendors;
                }
                var checklist=await GetCheckListOfProperty(property.PropertyCheckListId);
                if (checklist.Any()) {
                    property.CheckListOfProperties = checklist;
                }
            }
            return property;
        }
                

        public async  Task<IList<PropertyCheckListDocuments>> GetPropertyCheckListDocumentsAsync(Guid id)
        {
            try
            {
                return await _dataSource.PropertyCheckListDocuments
                    .Where(r => r.PropertyGuid == id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UploadPropertyCheckListDocumentsAsync(List<PropertyCheckListDocuments> documents)
        {
            try
            {
                foreach (var doc in documents)
                {
                    _dataSource.Entry(doc).State = EntityState.Added;
                }
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PropertyCheckListVendor> GetPropertyCheckListVendors(int id)
        {
            try
            {

                //var list=   (from pv in _dataSource.PropertyCheckListVendor join
                //    v in _dataSource.Vendors on pv.VendorId equals v.VendorId
                //    where(pv.VendorId== v.VendorId && pv.PropertyCheckListId==id)
                //   select(  new PropertyCheckListVendor { PropertyCheckListId=pv.PropertyCheckListId,
                //   CheckListVendorId=pv.CheckListVendorId,
                //   VendorId=pv.VendorId,
                //   IsPrimaryVendor=pv.IsPrimaryVendor,
                //   VendorName=v.VendorName})).ToList();

                var list = (from pv in _dataSource.PropertyCheckListVendor
                            from v in _dataSource.Vendors.Where(x=>x.VendorId==pv.VendorId).DefaultIfEmpty()
                            from vg in _dataSource.Groups.Where(x=>x.GroupId==pv.VendorId).DefaultIfEmpty()
                            where ( pv.PropertyCheckListId == id)
                            select (new PropertyCheckListVendor
                            {
                                PropertyCheckListId = pv.PropertyCheckListId,
                                CheckListVendorId = pv.CheckListVendorId,
                                VendorId =(pv.IsGroup)?vg.GroupId: pv.VendorId,
                                IsPrimaryVendor = pv.IsPrimaryVendor,
                                VendorName = (pv.IsGroup) ?vg.GroupName+" (G)":  v.VendorName
                            })).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
               
        public async Task<List<CheckListOfProperty>> GetCheckListOfProperty(int id)
        {
            try
            {
                var items =await  (from cl in _dataSource.CheckLists join
                                   clp in _dataSource.CheckListOfProperty on cl.CheckListId equals clp.CheckListId 
                                   where clp.PropertyCheckListId==id
                             select new CheckListOfProperty
                             {
                                 PropertyCheckListId = clp.PropertyCheckListId,
                                 CheckListId = clp.CheckListId,
                                 Mandatory = clp.Mandatory,
                                 Name = cl.CheckListName,
                                 CheckListPropertyId = clp.CheckListPropertyId,
                                 IsReceived=clp.IsReceived
                             }
                              ).ToListAsync();

                foreach (var obj in items) {
                    obj.Documents =await _dataSource.PropertyCheckListDocuments.Where(x => x.CheckListPropertyId == obj.CheckListPropertyId).ToListAsync();
                }

                return items;
                //return await _dataSource.CheckListOfProperty
                //    .Where(r => r.PropertyCheckListId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<PropertyCheckList>> GetPropertyCheckListAsync(DataRequest<PropertyCheckList> request)
        {
            IQueryable<PropertyCheckList> items = GetPropertyCheckList(request);
            return await items.ToListAsync();
        }

        private IQueryable<PropertyCheckList> GetPropertyCheckList(DataRequest<PropertyCheckList> request)
        {
            IQueryable<PropertyCheckList> items = _dataSource.PropertyCheckList;

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

        public async Task<IList<PropertyCheckList>> GetPropertyCheckListAsync(int skip, int take, DataRequest<PropertyCheckList> request)
        {
            try
            {
                IQueryable<PropertyCheckList> items = GetPropertyCheckList(request);
                var records = await items.Skip(skip).Take(take).ToListAsync();

                var finalResult = (from r in records
                                   from c in _dataSource.Companies.Where(x => x.CompanyID == r.CompanyID).DefaultIfEmpty()
                                   from v in _dataSource.Villages.Where(x => x.VillageId == r.VillageId).DefaultIfEmpty()
                                   select new PropertyCheckList
                                   {
                                       PropertyCheckListId = r.PropertyCheckListId,
                                       PropertyGuid = r.PropertyGuid,
                                       PropertyName = r.PropertyName,
                                       TalukId = r.TalukId,
                                       HobliId = r.HobliId,
                                       VillageId = r.VillageId,
                                       DocumentTypeId = r.DocumentTypeId,
                                       PropertyTypeId = r.PropertyTypeId,
                                       SurveyNo = r.SurveyNo,
                                       PropertyGMapLink = r.PropertyGMapLink,
                                       LandAreaInputAcres = r.LandAreaInputAcres,
                                       LandAreaInputGuntas = r.LandAreaInputGuntas,
                                       LandAreaInputAanas = r.LandAreaInputAanas,
                                       LandAreaInAcres = r.LandAreaInAcres,
                                       LandAreaInGuntas = r.LandAreaInGuntas,
                                       LandAreaInSqMts = r.LandAreaInSqMts,
                                       LandAreaInSqft = r.LandAreaInSqft,
                                       AKarabAreaInputAcres = r.AKarabAreaInputAcres,
                                       AKarabAreaInputGuntas = r.AKarabAreaInputGuntas,
                                       AKarabAreaInputAanas = r.AKarabAreaInputAanas,
                                       AKarabAreaInAcres = r.AKarabAreaInAcres,
                                       AKarabAreaInGuntas = r.AKarabAreaInGuntas,
                                       AKarabAreaInSqMts = r.AKarabAreaInSqMts,
                                       AKarabAreaInSqft = r.AKarabAreaInSqft,
                                       BKarabAreaInputAcres = r.BKarabAreaInputAcres,
                                       BKarabAreaInputGuntas = r.BKarabAreaInputGuntas,
                                       BKarabAreaInputAanas = r.BKarabAreaInputAanas,
                                       BKarabAreaInAcres = r.BKarabAreaInAcres,
                                       BKarabAreaInGuntas = r.BKarabAreaInGuntas,
                                       BKarabAreaInSqMts = r.BKarabAreaInSqMts,
                                       BKarabAreaInSqft = r.BKarabAreaInSqft,
                                       CheckListMaster = r.CheckListMaster,
                                       PropertyDescription = r.PropertyDescription,
                                       CompanyName = c.Name,
                                       VillageName = v.VillageName,
                                       Status = r.Status,
                                       Remarks = r.Remarks,
                                       TotalArea = (r.LandAreaInputAcres + r.AKarabAreaInputAcres + r.BKarabAreaInputAcres) + " - " +
                                (r.LandAreaInputGuntas + r.AKarabAreaInputGuntas + r.BKarabAreaInputGuntas) + " - " +
                                (r.LandAreaInputAanas + r.AKarabAreaInputAanas + r.BKarabAreaInputAanas)                                       
                                   }).ToList();

                return finalResult;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> GetPropertyCheckListCountAsync(DataRequest<PropertyCheckList> request)
        {
            IQueryable<PropertyCheckList> items = _dataSource.PropertyCheckList;

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

        public async Task<int> UpdatePropertyCheckListAsync(PropertyCheckList model)
        {
            try
            {
                //ICollection<PropertyCheckListDocuments> docs = model.PropertyCheckListDocuments;
                ICollection<PropertyCheckListVendor> vendors = model.PropertyCheckListVendors;
                ICollection<CheckListOfProperty> checklist = model.CheckListOfProperties;
                _dataSource.Entry(model).State = EntityState.Modified;
                 await _dataSource.SaveChangesAsync();
                int res = model.PropertyCheckListId;
                //if (docs != null)
                //{
                //    foreach (var doc in docs)
                //    {
                //        if (doc.PropertyCheckListBlobId == 0)
                //        {
                //            doc.PropertyGuid = model.PropertyGuid;
                //            _dataSource.PropertyCheckListDocuments.Add(doc);
                //        }
                //    }
                //}
                //await _dataSource.SaveChangesAsync();
                if (vendors != null)
                {
                    foreach (var vendor in vendors)
                    {
                        if (vendor.CheckListVendorId == 0)
                        {
                            vendor.PropertyCheckListId = res;
                            _dataSource.PropertyCheckListVendor.Add(vendor);
                        }
                        else {
                            _dataSource.PropertyCheckListVendor.Update(vendor);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();

                if (checklist != null)
                {
                    foreach (var check in checklist)
                    {
                        check.PropertyCheckListId = res;
                        if (check.CheckListPropertyId == 0)
                        {

                            _dataSource.CheckListOfProperty.Add(check);
                        }
                        else if (check.Delete)
                        {
                            var item = _dataSource.CheckListOfProperty.Where(x => x.CheckListPropertyId == check.CheckListPropertyId).FirstOrDefault();
                            _dataSource.CheckListOfProperty.Remove(item);
                        }
                        else
                        {
                            var item = _dataSource.CheckListOfProperty.Where(x => x.CheckListPropertyId == check.CheckListPropertyId).FirstOrDefault();
                            item.Mandatory = check.Mandatory;
                            item.IsReceived = check.IsReceived;
                            _dataSource.Entry(item).State = EntityState.Modified;
                        }

                        var docs = check.Documents;
                        if (docs != null)
                        {
                            foreach (var doc in docs)
                            {
                                if (doc.PropertyCheckListBlobId == 0)
                                {
                                    doc.CheckListPropertyId = check.CheckListPropertyId;
                                    doc.PropertyGuid = model.PropertyGuid;
                                    _dataSource.PropertyCheckListDocuments.Add(doc);
                                }
                            }
                        }
                    }
                }
               
                await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> UpdatePropertyCheckListStatusAsync(int id, int status, string remarks) {
            var model = _dataSource.PropertyCheckList.FirstOrDefault(x => x.PropertyCheckListId == id);
            if (model != null)
            {
                model.Status = status;
                model.Remarks = remarks;
                _dataSource.PropertyCheckList.Update(model);
                await _dataSource.SaveChangesAsync();
                return 0;
            }
            return -1;
        }

        public async Task<int> DeletePropertyCheckListAsync(PropertyCheckList model)
        {
            _dataSource.PropertyCheckList.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

      
        public async Task<int> DeletePropertyCheckListDocumentAsync(PropertyCheckListDocuments documents)
        {
            _dataSource.PropertyCheckListDocuments.Remove(documents);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeletePropertyCheckListVendorAsync(PropertyCheckListVendor vendor)
        {
            _dataSource.PropertyCheckListVendor.Remove(vendor);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteCheckListOfPropertyAsync(int checkListPropertyId)
        {
            var item = _dataSource.CheckListOfProperty.Where(x => x.CheckListPropertyId == checkListPropertyId).FirstOrDefault();
            _dataSource.CheckListOfProperty.Remove(item);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
