using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPropertyAsync(Property model)
        {
            if (model == null)
                return 0;
            try
            {
                
                var entity = new Property()
                {
                    PropertyGuid = model.PropertyGuid,
                    PropertyName = model.PropertyName,
                    GroupGuid = model.GroupGuid,
                    PartyId = model.PartyId,
                    TalukId = model.TalukId,
                    HobliId = model.HobliId,
                    VillageId = model.VillageId,
                   // DocumentTypeId = model.DocumentTypeId,
                    DateOfExecution = model.DateOfExecution,
                    DocumentNo = model.DocumentNo,
                    PropertyTypeId = model.PropertyTypeId,
                    SurveyNo = model.SurveyNo,
                    PropertyGMapLink = model.PropertyGMapLink,
                    //LandAreaInputAcres = model.LandAreaInputAcres,
                    //LandAreaInputGuntas = model.LandAreaInputGuntas,
                    //LandAreaInputAanas = model.LandAreaInputAanas,
                    //LandAreaInAcres = model.LandAreaInAcres,
                    //LandAreaInGuntas = model.LandAreaInGuntas,
                    //LandAreaInSqMts = model.LandAreaInSqMts,
                    //LandAreaInSqft = model.LandAreaInSqft,
                    //AKarabAreaInputAcres = model.AKarabAreaInputAcres,
                    //AKarabAreaInputGuntas = model.AKarabAreaInputGuntas,
                    //AKarabAreaInputAanas = model.AKarabAreaInputAanas,
                    //AKarabAreaInAcres = model.AKarabAreaInAcres,
                    //AKarabAreaInGuntas = model.AKarabAreaInGuntas,
                    //AKarabAreaInSqMts = model.AKarabAreaInSqMts,
                    //AKarabAreaInSqft = model.AKarabAreaInSqft,
                    //BKarabAreaInputAcres = model.BKarabAreaInputAcres,
                    //BKarabAreaInputGuntas = model.BKarabAreaInputGuntas,
                    //BKarabAreaInputAanas = model.BKarabAreaInputAanas,
                    //BKarabAreaInAcres = model.BKarabAreaInAcres,
                    //BKarabAreaInGuntas = model.BKarabAreaInGuntas,
                    //BKarabAreaInSqMts = model.BKarabAreaInSqMts,
                    //BKarabAreaInSqft = model.BKarabAreaInSqft,
                    //SaleValue1 = model.SaleValue1,
                    //SaleValue2 = model.SaleValue2,
                    CompanyID = model.CompanyID,
                    IsSold = false
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                await _dataSource.SaveChangesAsync();
                int res = entity.PropertyId;

                await AddPropertyDocumentTypeAndDocument(model.PropertyDocumentType, res, model.PropertyGuid);

                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private async Task AddPropertyDocumentTypeAndDocument(ICollection<PropertyDocumentType> documentTypes, int propId, Guid guid)
        {

            foreach (var docType in documentTypes)
            {
                var docTypeEntity = new PropertyDocumentType
                {
                    PropertyId = propId,
                    DocumentTypeId = docType.DocumentTypeId,
                    LandAreaInputAcres = docType.LandAreaInputAcres,
                    LandAreaInputGuntas = docType.LandAreaInputGuntas,
                    LandAreaInputAanas = docType.LandAreaInputAanas,
                    LandAreaInAcres = docType.LandAreaInAcres,
                    LandAreaInGuntas = docType.LandAreaInGuntas,
                    LandAreaInSqMts = docType.LandAreaInSqMts,
                    LandAreaInSqft = docType.LandAreaInSqft,
                    AKarabAreaInputAcres = docType.AKarabAreaInputAcres,
                    AKarabAreaInputGuntas = docType.AKarabAreaInputGuntas,
                    AKarabAreaInputAanas = docType.AKarabAreaInputAanas,
                    AKarabAreaInAcres = docType.AKarabAreaInAcres,
                    AKarabAreaInGuntas = docType.AKarabAreaInGuntas,
                    AKarabAreaInSqMts = docType.AKarabAreaInSqMts,
                    AKarabAreaInSqft = docType.AKarabAreaInSqft,
                    BKarabAreaInputAcres = docType.BKarabAreaInputAcres,
                    BKarabAreaInputGuntas = docType.BKarabAreaInputGuntas,
                    BKarabAreaInputAanas = docType.BKarabAreaInputAanas,
                    BKarabAreaInAcres = docType.BKarabAreaInAcres,
                    BKarabAreaInGuntas = docType.BKarabAreaInGuntas,
                    BKarabAreaInSqMts = docType.BKarabAreaInSqMts,
                    BKarabAreaInSqft = docType.BKarabAreaInSqft,
                    SaleValue1 = docType.SaleValue1,
                    SaleValue2 = docType.SaleValue2
                };
                _dataSource.PropertyDocumentType.Add(docTypeEntity);
                await _dataSource.SaveChangesAsync();
                int docTypeId = docTypeEntity.PropertyDocumentTypeId;

                ICollection<PropertyDocuments> docs = docType.PropertyDocuments;

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (doc.PropertyBlobId == 0)
                        {
                            doc.PropertyGuid = guid;
                            doc.PropertyDocumentTypeId = docTypeId;
                            _dataSource.PropertyDocuments.Add(doc);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();
            }
        }

        public async Task<Property> GetPropertyAsync(long id)
        {
            var property = await _dataSource.Properties.Where(r => r.PropertyId == id).FirstOrDefaultAsync();

            if (property != null)
            {
                var docTypes = await GetPropertyDocumentTypeAsync(property.PropertyId);
                if (docTypes.Any())
                {
                    foreach (var doc in docTypes) {
                        doc.PropertyDocuments = await GetPropertyDocumentsAsync(doc.PropertyDocumentTypeId);
                    }                   
                }
                property.PropertyDocumentType = docTypes;
            }
            return property;
        }

        public async Task<List<Property>> GetPropertyByGroupGuidAsync(Guid guid)
        {
            var properties = await _dataSource.Properties.Where(r => r.GroupGuid == guid).ToListAsync();
            if (properties != null) {

                foreach (var model in properties) {
                    var docTypes = await GetPropertyDocumentTypeAsync(model.PropertyId);
                    if (docTypes.Any())
                    {
                        foreach (var doc in docTypes)
                        {
                            doc.PropertyDocuments = await GetPropertyDocumentsAsync(doc.PropertyDocumentTypeId);
                        }
                    }
                    model.PropertyDocumentType = docTypes;
                }
            }
           
            return properties;
        }

        public async Task<List<PropertyDocumentType>> GetPropertyDocumentTypeAsync(int id)
        {
            try
            {
                return await _dataSource.PropertyDocumentType
                    .Where(r => r.PropertyId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<PropertyDocuments>> GetPropertyDocumentsAsync(int id)
        {
            try
            {
                return await _dataSource.PropertyDocuments
                    .Where(r => r.PropertyDocumentTypeId == id).ToListAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }


        public async Task<IList<Property>> GetPropertiesAsync(DataRequest<Property> request)
        {
            IQueryable<Property> items = GetProperties(request);
            return await items.ToListAsync();
        }

        private IQueryable<Property> GetProperties(DataRequest<Property> request)
        {
             IQueryable<Property> items = _dataSource.Properties;
            //IQueryable<Property> items = from p in _dataSource.Properties
            //                             select new Property
            //                             {
            //                                 PropertyId = p.PropertyId,
            //                                 PropertyGuid = p.PropertyGuid,
            //                                 PropertyName = p.PropertyName,
            //                                 GroupGuid = p.GroupGuid,
            //                                 PartyId = p.PartyId,
            //                                 TalukId = p.TalukId,
            //                                 HobliId = p.HobliId,
            //                                 VillageId = p.VillageId,
            //                                 DocumentTypeId = p.DocumentTypeId,
            //                                 DateOfExecution = p.DateOfExecution,
            //                                 DocumentNo = p.DocumentNo,
            //                                 PropertyTypeId = p.PropertyTypeId,
            //                                 SurveyNo = p.SurveyNo,
            //                                 PropertyGMapLink = p.PropertyGMapLink,
            //                                 PropertyDocumentType = _dataSource.PropertyDocumentType.Where(x => x.PropertyId == p.PropertyId).ToList()
            //                             };

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

        public async Task<IList<Property>> GetPropertiesAsync(int skip, int take, DataRequest<Property> request)
        {
            IQueryable<Property> items = GetProperties(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Property
                {
                    PropertyId = source.PropertyId,
                    PropertyGuid = source.PropertyGuid,
                    PropertyName = source.PropertyName,
                    GroupGuid = source.GroupGuid,
                    PartyId = source.PartyId,
                    TalukId = source.TalukId,
                    HobliId = source.HobliId,
                    VillageId = source.VillageId,
                    DocumentTypeId = source.DocumentTypeId,
                    DateOfExecution = source.DateOfExecution,
                    DocumentNo = source.DocumentNo,
                    PropertyTypeId = source.PropertyTypeId,
                    SurveyNo = source.SurveyNo,
                    PropertyGMapLink = source.PropertyGMapLink,
                    //LandAreaInputAcres = source.LandAreaInputAcres,
                    //LandAreaInputGuntas = source.LandAreaInputGuntas,
                    //LandAreaInputAanas=source.LandAreaInputAanas,
                    //LandAreaInAcres = source.LandAreaInAcres,
                    //LandAreaInGuntas = source.LandAreaInGuntas,
                    //LandAreaInSqMts = source.LandAreaInSqMts,
                    //LandAreaInSqft = source.LandAreaInSqft,
                    //AKarabAreaInputAcres = source.AKarabAreaInputAcres,
                    //AKarabAreaInputGuntas = source.AKarabAreaInputGuntas,
                    //AKarabAreaInputAanas=source.AKarabAreaInputAanas,
                    //AKarabAreaInAcres = source.AKarabAreaInAcres,
                    //AKarabAreaInGuntas = source.AKarabAreaInGuntas,
                    //AKarabAreaInSqMts = source.AKarabAreaInSqMts,
                    //AKarabAreaInSqft = source.AKarabAreaInSqft,
                    //BKarabAreaInputAcres = source.BKarabAreaInputAcres,
                    //BKarabAreaInputGuntas = source.BKarabAreaInputGuntas,
                    //BKarabAreaInputAanas=source.BKarabAreaInputAanas,
                    //BKarabAreaInAcres = source.BKarabAreaInAcres,
                    //BKarabAreaInGuntas = source.BKarabAreaInGuntas,
                    //BKarabAreaInSqMts = source.BKarabAreaInSqMts,
                    //BKarabAreaInSqft = source.BKarabAreaInSqft,
                    //SaleValue1 = source.SaleValue1,
                    //SaleValue2 = source.SaleValue2,
                    IsSold = source.IsSold,
                    
                }).AsNoTracking()              
                .ToListAsync();
            foreach (var prop in records) {
                var propDocumentTypes = (from pd in _dataSource.PropertyDocumentType
                                        join
dt in _dataSource.DocumentTypes on pd.DocumentTypeId equals dt.DocumentTypeId
                                        where (pd.PropertyId == prop.PropertyId)
                                        select new PropertyDocumentType
                                        {
                                            PropertyDocumentTypeId = pd.PropertyDocumentTypeId,
                                            PropertyId = pd.PropertyId,
                                            DocumentTypeName = dt.DocumentTypeName,
                                            DocumentTypeId = pd.DocumentTypeId,
                                            LandAreaInputAcres = pd.LandAreaInputAcres,
                                            LandAreaInputGuntas = pd.LandAreaInputGuntas,
                                            LandAreaInputAanas = pd.LandAreaInputAanas,
                                            LandAreaInAcres = pd.LandAreaInAcres,
                                            LandAreaInGuntas = pd.LandAreaInGuntas,
                                            LandAreaInSqMts = pd.LandAreaInSqMts,
                                            LandAreaInSqft = pd.LandAreaInSqft,
                                            AKarabAreaInputAcres = pd.AKarabAreaInputAcres,
                                            AKarabAreaInputGuntas = pd.AKarabAreaInputGuntas,
                                            AKarabAreaInputAanas = pd.AKarabAreaInputAanas,
                                            AKarabAreaInAcres = pd.AKarabAreaInAcres,
                                            AKarabAreaInGuntas = pd.AKarabAreaInGuntas,
                                            AKarabAreaInSqMts = pd.AKarabAreaInSqMts,
                                            AKarabAreaInSqft = pd.AKarabAreaInSqft,
                                            BKarabAreaInputAcres = pd.BKarabAreaInputAcres,
                                            BKarabAreaInputGuntas = pd.BKarabAreaInputGuntas,
                                            BKarabAreaInputAanas = pd.BKarabAreaInputAanas,
                                            BKarabAreaInAcres = pd.BKarabAreaInAcres,
                                            BKarabAreaInGuntas = pd.BKarabAreaInGuntas,
                                            BKarabAreaInSqMts = pd.BKarabAreaInSqMts,
                                            BKarabAreaInSqft = pd.BKarabAreaInSqft,
                                            SaleValue1 = pd.SaleValue1,
                                            SaleValue2 = pd.SaleValue2,
                                            LandArea=CalculateArea(pd)
                                        }).ToList();

                prop.PropertyDocumentType = propDocumentTypes;
            }

            return records;
        }
      
        public async Task<int> GetPropertiesCountAsync(DataRequest<Property> request)
        {
            IQueryable<Property> items = _dataSource.Properties;

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

        public async Task<int> UpdatePropertyAsync(Property model)
        {
            try
            {
                var modelCopy = model;
                _dataSource.Entry(model).State = EntityState.Modified;
                int res = await _dataSource.SaveChangesAsync();
               await UpdatePropertyDocumentTypeAndDocument(modelCopy.PropertyDocumentType, modelCopy.PropertyId, modelCopy.PropertyGuid);
                
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private async Task UpdatePropertyDocumentTypeAndDocument(ICollection<PropertyDocumentType> documentTypes, int propId,Guid guid)
        {

            foreach (var docType in documentTypes)
            {
                int docTypeId = 0;
                if (docType.PropertyDocumentTypeId == 0)
                {
                    
                    var docTypeEntity = new PropertyDocumentType
                    {
                        PropertyId = propId,
                        DocumentTypeId = docType.DocumentTypeId,
                        LandAreaInputAcres = docType.LandAreaInputAcres,
                        LandAreaInputGuntas = docType.LandAreaInputGuntas,
                        LandAreaInputAanas = docType.LandAreaInputAanas,
                        LandAreaInAcres = docType.LandAreaInAcres,
                        LandAreaInGuntas = docType.LandAreaInGuntas,
                        LandAreaInSqMts = docType.LandAreaInSqMts,
                        LandAreaInSqft = docType.LandAreaInSqft,
                        AKarabAreaInputAcres = docType.AKarabAreaInputAcres,
                        AKarabAreaInputGuntas = docType.AKarabAreaInputGuntas,
                        AKarabAreaInputAanas = docType.AKarabAreaInputAanas,
                        AKarabAreaInAcres = docType.AKarabAreaInAcres,
                        AKarabAreaInGuntas = docType.AKarabAreaInGuntas,
                        AKarabAreaInSqMts = docType.AKarabAreaInSqMts,
                        AKarabAreaInSqft = docType.AKarabAreaInSqft,
                        BKarabAreaInputAcres = docType.BKarabAreaInputAcres,
                        BKarabAreaInputGuntas = docType.BKarabAreaInputGuntas,
                        BKarabAreaInputAanas = docType.BKarabAreaInputAanas,
                        BKarabAreaInAcres = docType.BKarabAreaInAcres,
                        BKarabAreaInGuntas = docType.BKarabAreaInGuntas,
                        BKarabAreaInSqMts = docType.BKarabAreaInSqMts,
                        BKarabAreaInSqft = docType.BKarabAreaInSqft,
                        SaleValue1 = docType.SaleValue1,
                        SaleValue2 = docType.SaleValue2
                    };
                    _dataSource.Entry(docTypeEntity).State = EntityState.Added;
                    await _dataSource.SaveChangesAsync();
                     docTypeId = docTypeEntity.PropertyDocumentTypeId;
                }
                else {
                    var docTypeEntity = _dataSource.PropertyDocumentType.Where(x => x.PropertyDocumentTypeId == docType.PropertyDocumentTypeId).FirstOrDefault();
                    docTypeEntity.PropertyId = docType.PropertyId;
                    docTypeEntity.DocumentTypeId = docType.DocumentTypeId;
                    docTypeEntity.LandAreaInputAcres = docType.LandAreaInputAcres;
                    docTypeEntity.LandAreaInputGuntas = docType.LandAreaInputGuntas;
                    docTypeEntity.LandAreaInputAanas = docType.LandAreaInputAanas;
                    docTypeEntity.LandAreaInAcres = docType.LandAreaInAcres;
                    docTypeEntity.LandAreaInGuntas = docType.LandAreaInGuntas;
                    docTypeEntity.LandAreaInSqMts = docType.LandAreaInSqMts;
                    docTypeEntity.LandAreaInSqft = docType.LandAreaInSqft;
                    docTypeEntity.AKarabAreaInputAcres = docType.AKarabAreaInputAcres;
                    docTypeEntity.AKarabAreaInputGuntas = docType.AKarabAreaInputGuntas;
                    docTypeEntity.AKarabAreaInputAanas = docType.AKarabAreaInputAanas;
                    docTypeEntity.AKarabAreaInAcres = docType.AKarabAreaInAcres;
                    docTypeEntity.AKarabAreaInGuntas = docType.AKarabAreaInGuntas;
                    docTypeEntity.AKarabAreaInSqMts = docType.AKarabAreaInSqMts;
                    docTypeEntity.AKarabAreaInSqft = docType.AKarabAreaInSqft;
                    docTypeEntity.BKarabAreaInputAcres = docType.BKarabAreaInputAcres;
                    docTypeEntity.BKarabAreaInputGuntas = docType.BKarabAreaInputGuntas;
                    docTypeEntity.BKarabAreaInputAanas = docType.BKarabAreaInputAanas;
                    docTypeEntity.BKarabAreaInAcres = docType.BKarabAreaInAcres;
                    docTypeEntity.BKarabAreaInGuntas = docType.BKarabAreaInGuntas;
                    docTypeEntity.BKarabAreaInSqMts = docType.BKarabAreaInSqMts;
                    docTypeEntity.BKarabAreaInSqft = docType.BKarabAreaInSqft;
                    docTypeEntity.SaleValue1 = docType.SaleValue1;
                    docTypeEntity.SaleValue2 = docType.SaleValue2;

                    _dataSource.Entry(docTypeEntity).State = EntityState.Modified;
                    await _dataSource.SaveChangesAsync();
                    docTypeId = docType.PropertyDocumentTypeId;
                }

                    ICollection<PropertyDocuments> docs = docType.PropertyDocuments;

                    if (docs != null)
                    {
                        foreach (var doc in docs)
                        {
                            if (doc.PropertyBlobId == 0)
                            {
                                doc.PropertyGuid = guid;
                                doc.PropertyDocumentTypeId = docTypeId;
                                _dataSource.PropertyDocuments.Add(doc);
                            }
                        }
                    }
                    await _dataSource.SaveChangesAsync();
                
            }
        }

        public async Task<int> DeletePropertyAsync(Property model)
        {
            try
            {
                var payments = await _dataSource.Payments.Where(x => x.PropertyId == model.PropertyId).ToListAsync();
                if (payments.Count > 0)
                    return -1;

                var propertyEnity = await _dataSource.Properties.Where(x => x.PropertyId == model.PropertyId).FirstOrDefaultAsync();
                var usedProperty = await _dataSource.PropertyMergeList.Where(x => x.PropertyGuid == propertyEnity.PropertyGuid).FirstOrDefaultAsync();
                if (usedProperty != null)
                    return 0;

                _dataSource.Properties.Remove(propertyEnity);
                return await _dataSource.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> AddPropertyParty(List<PropertyParty> propertyParties) {
            if (propertyParties == null)
                return 0;

            var party = propertyParties[0];
            var properties =await _dataSource.Properties.Where(x => x.GroupGuid == party.PropertyGuid).ToListAsync();
            foreach (var prop in properties) {
                var existParty = _dataSource.PropertyParty.Where(x => x.PropertyId == prop.PropertyId).ToList();
                if (existParty != null) {
                    _dataSource.PropertyParty.RemoveRange(existParty);
                    await _dataSource.SaveChangesAsync();
                }
                var entity = new PropertyParty
                {
                    PropertyId = prop.PropertyId,
                    IsGroup = party.IsGroup,
                    IsPrimaryParty = party.IsPrimaryParty,
                    PartyId = party.PartyId,
                    PropertyGuid = party.PropertyGuid
                };
                _dataSource.Entry(entity).State = EntityState.Added;
            }

            //foreach (var model in propertyParties) {
            //    if(model.PropertyPartyId==0)
            //    _dataSource.Entry(model).State = EntityState.Added;
            //    else
            //        _dataSource.Entry(model).State = EntityState.Modified;
            //}
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<List<PropertyParty>> GetPartiesOfProperty(int propertyId) {
            try
            {
                var list =await (from pp in _dataSource.PropertyParty
                            from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId).DefaultIfEmpty()
                            from pg in _dataSource.Groups.Where(x => x.GroupId == pp.PartyId).DefaultIfEmpty()
                            where ( pp.PropertyId == propertyId)
                             select new PropertyParty
                            {
                                PropertyPartyId = pp.PropertyPartyId,
                                PartyId =  (pp.IsGroup)?pg.GroupId: pp.PartyId,
                                PropertyGuid = pp.PropertyGuid,
                                PropertyId = pp.PropertyId,
                                IsGroup=pp.IsGroup,
                                PartyName = (pp.IsGroup) ? pg.GroupName+" (G)" : party.PartyFirstName,
                                IsPrimaryParty = pp.IsPrimaryParty
                            }).ToListAsync();
               
                //var model = await (from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == propertyId)
                //                   from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId)
                //                   select new PropertyParty
                //                   {
                //                       PropertyPartyId = pp.PropertyPartyId,
                //                       PartyId = pp.PartyId,
                //                       PropertyGuid = pp.PropertyGuid,
                //                       PropertyId = pp.PropertyId,
                //                       PartyName = party.PartyFirstName,
                //                       IsPrimaryParty=pp.IsPrimaryParty
                //                   }).ToListAsync();
                return list;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeletePropertyPartyAsync(PropertyParty model)
        {
            _dataSource.PropertyParty.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> AddPropPaySchedule(int propDocTypeId, List<PropPaySchedule> schedules,decimal Sale1,decimal Sale2)
        {
            if (schedules == null)
                return 0;
            try
            {
                //var property = _dataSource.Properties.Where(x => x.PropertyId == propertyId).FirstOrDefault();
                //if (property != null) {
                //    property.SaleValue1 = Sale1;
                //    property.SaleValue2 = Sale2;
                //    _dataSource.Entry(property).State = EntityState.Modified;
                //    await _dataSource.SaveChangesAsync();
                //}

                var propDocType = _dataSource.PropertyDocumentType.Where(x => x.PropertyDocumentTypeId == propDocTypeId).FirstOrDefault();
                if (propDocType != null)
                {
                    propDocType.SaleValue1 = Sale1;
                    propDocType.SaleValue2 = Sale2;
                    _dataSource.Entry(propDocType).State = EntityState.Modified;
                    await _dataSource.SaveChangesAsync();
                }
                foreach (var model in schedules)
                {
                    if(model.PropPayScheduleId==0)
                    _dataSource.Entry(model).State = EntityState.Added;
                    else
                        _dataSource.Entry(model).State = EntityState.Modified;
                }
               await _dataSource.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        public async Task<int> DeletePropPaySchedule(int propPayScheduleId) {
            try
            {
                var paySchedule = _dataSource.PropPaySchedules.Where(x => x.PropPayScheduleId == propPayScheduleId).FirstOrDefault();
                _dataSource.PropPaySchedules.Remove(paySchedule);
                await _dataSource.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PropertyCostDetails> GetPropertyCostDetails(int propDocTypeId)
        {
            try
            {
                var model = (from pdt in _dataSource.PropertyDocumentType.Where(x=>x.PropertyDocumentTypeId== propDocTypeId)
                             from prop in _dataSource.Properties.Where(x => x.PropertyId == pdt.PropertyId)
                             from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == pdt.PropertyId).DefaultIfEmpty()
                             from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId).DefaultIfEmpty()
                             from com in _dataSource.Companies.Where(x => x.CompanyID == prop.CompanyID).DefaultIfEmpty()
                             from pt in _dataSource.PropertyTypes.Where(x => x.PropertyTypeId == prop.PropertyTypeId).DefaultIfEmpty()
                             from dt in _dataSource.DocumentTypes.Where(x => x.DocumentTypeId == pdt.DocumentTypeId).DefaultIfEmpty()
                             from t in _dataSource.Taluks.Where(x => x.TalukId == prop.TalukId).DefaultIfEmpty()
                             from h in _dataSource.Hoblis.Where(x => x.HobliId == prop.HobliId).DefaultIfEmpty()
                             from v in _dataSource.Villages.Where(x => x.VillageId == prop.VillageId).DefaultIfEmpty()
                             select new PropertyCostDetails
                             {
                                 PropertyDocumentTypeId=pdt.PropertyDocumentTypeId,
                                 PropertyId = prop.PropertyId,
                                 PropertyName = prop.PropertyName,
                                 ComapnyName = com.Name,
                                 PropertyType = pt.PropertyTypeText,
                                 DocumentType = dt.DocumentTypeName,
                                 Taluk = t.TalukName,
                                 Hobli = h.HobliName,
                                 Village = v.VillageName,
                                 SurveyNo = prop.SurveyNo,
                                 SaleValue1 = pdt.SaleValue1,
                                 SaleValue2 = pdt.SaleValue2,

                             }).FirstOrDefault();

                model.PropertyParties = await (from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == model.PropertyId)
                                               from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId)
                                               select new PropertyParty
                                               {
                                                   PropertyPartyId = pp.PropertyPartyId,
                                                   PartyId = pp.PartyId,
                                                   PropertyGuid = pp.PropertyGuid,
                                                   PropertyId = pp.PropertyId,
                                                   PartyName = party.PartyFirstName,
                                                   IsPrimaryParty=pp.IsPrimaryParty
                                               }).ToListAsync();
                model.PropPaySchedules = await _dataSource.PropPaySchedules.Where(p => p.PropertyDocumentTypeId == model.PropertyDocumentTypeId).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UploadPropertyDocumentsAsync(List<PropertyDocuments> documents)
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

        public async Task<int> DeletePropertyDocumentAsync(PropertyDocuments documents)
        {
            _dataSource.PropertyDocuments.Remove(documents);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
