using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPartyAsync(Party party)
        {
            try
            {
                if (party == null)
                    return 0;

                var entity = new Party()
                {
                    PartyFirstName = party.PartyFirstName,
                    PartyGuid = party.PartyGuid,
                    PartyAlias = party.PartyAlias,
                    PartySalutation = party.PartySalutation,
                    AadharNo = party.AadharNo,
                    ContactPerson = party.ContactPerson,
                    PAN = party.PAN,
                    GSTIN = party.GSTIN,
                    email = party.email,
                    IsPartyActive = party.IsPartyActive,
                    PhoneNo = party.PhoneNo,
                    AddressLine1 = party.AddressLine1,
                    AddressLine2 = party.AddressLine2,
                    City = party.City,
                    PinCode = party.PinCode,
                    GroupId=party.GroupId,
                    BankName=party.BankName,
                    Branch=party.Branch,
                    IFSCCode=party.IFSCCode,
                    AccountNumber=party.AccountNumber
                };
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdatePartyAsync(Party party)
        {
           
                ICollection<PartyDocuments> docs = party.PartyDocuments;
                party.PartyDocuments = null;
                if (party.PartyId > 0)
                {
                    _dataSource.Entry(party).State = EntityState.Modified;
                }
                else
                {
                    party.PartyGuid = Guid.NewGuid();
                    _dataSource.Entry(party).State = EntityState.Added;
                }

                party.SearchTerms = party.BuildSearchTerms();
                int res = await _dataSource.SaveChangesAsync();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (doc.PartyBlobId == 0)
                        {
                            doc.PartyGuid = party.PartyGuid;
                            _dataSource.PartyDocuments.Add(doc);
                        }
                    }
                }
                await _dataSource.SaveChangesAsync();
                return res;
               
           
        }

        public async Task<Party> GetPartyAsync(long id)
        {
            var party =await _dataSource.Parties.Where(r => r.PartyId == id).FirstOrDefaultAsync();
            if (party.PartyGuid != null)
            {
                var docs =await GetPartyDocumentsAsync(party.PartyGuid);
                if (docs.Any())
                {
                    party.PartyDocuments = docs;
                }

            }
            return party;
        }
        public async Task<List<PartyDocuments>> GetPartyDocumentsAsync(Guid id)
        {
            return await _dataSource.PartyDocuments
                .Where(r => r.PartyGuid == id).ToListAsync();
        }
        public async Task<IList<Party>> GetPartiesAsync(int skip, int take, DataRequest<Party> request)
        {
            IQueryable<Party> items = GetParties(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Party
                {
                    PartyId = source.PartyId,
                    PartyFirstName = source.PartyFirstName,
                    PartyGuid = source.PartyGuid,
                    PartyAlias = source.PartyAlias,
                    PartySalutation = source.PartySalutation,
                    AadharNo = source.AadharNo,
                    ContactPerson = source.ContactPerson,
                    PAN = source.PAN,
                    GSTIN = source.GSTIN,
                    email = source.email,
                    IsPartyActive = source.IsPartyActive,
                    PhoneNo = source.PhoneNo,
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    City = source.City,
                    PinCode = source.PinCode,
                    SalutationType=source.SalutationType,
                    GroupId=source.GroupId??0,
                    GroupName=source.GroupName,
                    BankName=source.BankName,
                    Branch=source.Branch,
                    IFSCCode=source.IFSCCode,
                    AccountNumber=source.AccountNumber
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<IList<Party>> GetPartiesAsync(DataRequest<Party> request)
        {
            IQueryable<Party> items = GetParties(request);
            return await items.ToListAsync();
        }

        public async Task<int> DeletePartyAsync(Party model)
        {
            var isReferenced = _dataSource.PropertyParty.Where(x => x.PartyId == model.PartyId).FirstOrDefault();
            if (isReferenced != null)
                return -1;
            _dataSource.Parties.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> UploadPartyDocumentsAsync(List<PartyDocuments> documents)
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
        public async Task<int> DeletePartyDocumentAsync(PartyDocuments documents)
        {
            _dataSource.PartyDocuments.Remove(documents);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> GetPartiesCountAsync(DataRequest<Party> request)
        {
            IQueryable<Party> items = _dataSource.Parties;
           

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

        private IQueryable<Party> GetParties(DataRequest<Party> request)
        {
            //IQueryable<Party> items = _dataSource.Parties;
            IQueryable<Party> items = from p in _dataSource.Parties
                                      from g in _dataSource.Groups.Where(x=>x.GroupId==p.GroupId).DefaultIfEmpty()
                                      select new Party
                                      {
                                          PartyId = p.PartyId,
                                          PartyFirstName = p.PartyFirstName,
                                          PartyGuid = p.PartyGuid,
                                          PartyAlias = p.PartyAlias,
                                          PartySalutation = p.PartySalutation,
                                          AadharNo = p.AadharNo,
                                          ContactPerson = p.ContactPerson,
                                          PAN = p.PAN,
                                          GSTIN = p.GSTIN,
                                          email = p.email,
                                          IsPartyActive = p.IsPartyActive,
                                          PhoneNo = p.PhoneNo,
                                          AddressLine1 = p.AddressLine1,
                                          AddressLine2 = p.AddressLine2,
                                          City = p.City,
                                          PinCode = p.PinCode,
                                          SalutationType = p.SalutationType,
                                          GroupId = p.GroupId??0,
                                          GroupName = (p.GroupId == null || p.GroupId == 0) ? "" : g.GroupName,
                                          BankName = p.BankName,
                                          Branch = p.Branch,
                                          IFSCCode = p.IFSCCode,
                                          AccountNumber = p.AccountNumber,
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
    }
}
