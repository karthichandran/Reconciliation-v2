using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddGroupsAsync(Groups model)
        {
            if (model == null)
                return 0;

            var entity = new Groups()
            {
                GroupId = model.GroupId,
                GroupName = model.GroupName,
                IsActive = model.IsActive,
                GroupType=model.GroupType
            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<Groups> GetGroupsAsync(long id)
        {
            return await _dataSource.Groups
                .Where(r => r.GroupId == id).FirstOrDefaultAsync();
        }

        private IQueryable<Groups> GetGroups(DataRequest<Groups> request)
        {
            IQueryable<Groups> items = _dataSource.Groups;

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


        public async Task<IList<Groups>> GetGroupsAsync(DataRequest<Groups> request)
        {
            IQueryable<Groups> items = GetGroups(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Groups>> GetGroupsAsync(int skip, int take, DataRequest<Groups> request)
        {
            IQueryable<Groups> items = GetGroups(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Groups
                {
                    GroupId = source.GroupId,
                    GroupName = source.GroupName,
                    GroupType=source.GroupType,
                    IsActive = source.IsActive,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetGroupsCountAsync(DataRequest<Groups> request)
        {
            IQueryable<Groups> items = _dataSource.Groups;

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

        public async Task<int> UpdateGroupsAsync(int existingGroupdId, Groups model)
        {
            if (model.DoCopyGroup)
            {

                var entity = new Groups()
                {
                    GroupName = model.GroupName,
                    IsActive = true,
                    GroupType = model.GroupType
                };
                _dataSource.Groups.Add(entity);
                int id = await _dataSource.SaveChangesAsync();
                await DoCopyGroup(existingGroupdId, entity.GroupId);
                return entity.GroupId;
            }
            else
            {
                _dataSource.Entry(model).State = EntityState.Modified;
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
        }

        private async Task<int> DoCopyGroup(int vendorGroupId, int partyGroupId) {
            try
            {
                var vendors = _dataSource.Vendors.Where(x => x.GroupId == vendorGroupId).ToList();
                foreach (var vendor in vendors)
                {
                    var entity = new Party()
                    {
                        GroupId = partyGroupId,
                        PartyGuid = Guid.NewGuid(),
                        PartyFirstName = vendor.VendorName,
                        PartyAlias = vendor.VendorAlias,
                        PartySalutation = vendor.VendorSalutation,
                        AadharNo = vendor.AadharNo,
                        ContactPerson = vendor.ContactPerson,
                        PAN = vendor.PAN,
                        GSTIN = vendor.GSTIN,
                        email = vendor.email,
                        IsPartyActive = vendor.IsVendorActive,
                        PhoneNo = vendor.PhoneNo,
                        AddressLine1 = vendor.AddressLine1,
                        AddressLine2 = vendor.AddressLine2,
                        City = vendor.City,
                        PinCode = vendor.PinCode,
                        SalutationType = vendor.SalutationType,
                        BankName = vendor.BankName,
                        Branch = vendor.Branch,
                        AccountNumber = vendor.AccountNumber,
                        IFSCCode = vendor.IFSCCode,
                    };

                    _dataSource.Entry(entity).State = EntityState.Added;
                    await _dataSource.SaveChangesAsync();

                    var docs = _dataSource.VendorDocuments.Where(x => x.VendorGuid == vendor.VendorGuid).ToList();
                    if (docs != null && docs.Count > 0)
                    {
                        foreach (var doc in docs)
                        {
                            var partyDoc = new PartyDocuments
                            {
                                PartyGuid = entity.PartyGuid,
                                FileName = doc.FileName,
                                FileBlob = doc.FileBlob,
                                FileType = doc.FileType,
                                FileLength = doc.FileLength,
                                FileCategoryId = doc.FileCategoryId,
                                UploadTime=DateTime.Now
                            };
                            _dataSource.PartyDocuments.Add(partyDoc);
                            await _dataSource.SaveChangesAsync();
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeleteGroupsAsync(Groups model)
        {
            _dataSource.Groups.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
