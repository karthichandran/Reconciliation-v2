using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
   partial class DataServiceBase
    {
        //public async Task<int> AddVendorAsync(Vendor vendor)
        //{
        //    try
        //    {
        //        if (vendor == null)
        //            return 0;

        //        var entity = new Vendor()
        //        {
        //            VendorName = vendor.VendorName,
        //            VendorGuid = vendor.VendorGuid,
        //            VendorAlias = vendor.VendorAlias,
        //            VendorSalutation = vendor.VendorSalutation,
        //            AadharNo = vendor.AadharNo,
        //            ContactPerson = vendor.ContactPerson,
        //            PAN = vendor.PAN,
        //            GSTIN = vendor.GSTIN,
        //            email = vendor.email,
        //            IsVendorActive = vendor.IsVendorActive,
        //            PhoneNo = vendor.PhoneNo,
        //            AddressLine1 = vendor.AddressLine1,
        //            AddressLine2 = vendor.AddressLine2,
        //            City = vendor.City,
        //            PinCode = vendor.PinCode
        //        };
        //        _dataSource.Entry(entity).State = EntityState.Added;
        //        int res = await _dataSource.SaveChangesAsync();
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        public async Task<int> UpdateVendorAsync(Vendor vendor)
        {
            try
            {

                ICollection<VendorDocuments> docs = vendor.VendorDocuments;
                vendor.VendorDocuments = null;
                if (vendor.VendorId > 0)
                {
                    _dataSource.Entry(vendor).State = EntityState.Modified;
                }
                else
                {
                    vendor.VendorGuid = Guid.NewGuid();
                    //Company.CreatedOn = DateTime.UtcNow;
                    _dataSource.Entry(vendor).State = EntityState.Added;
                }
                vendor.SearchTerms = vendor.BuildSearchTerms();
                int res = await _dataSource.SaveChangesAsync();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (doc.VendorBlobId == 0)
                        {
                            doc.VendorGuid = vendor.VendorGuid;
                            _dataSource.VendorDocuments.Add(doc);

                        }
                    }
                }
                await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
           
        }

        public async Task<Vendor> GetVendorAsync(long id)
        {
           
                var vendor= await _dataSource.Vendors.Where(x => x.VendorId == id).FirstOrDefaultAsync();
                if (vendor.VendorGuid != null)
                {
                    var docs =await GetVendorDocumentsAsync(vendor.VendorGuid);
                    if (docs.Any())
                    {
                        vendor.VendorDocuments = docs;
                    }

                }
                return vendor;
           
        }
        public async Task<List<VendorDocuments>> GetVendorDocumentsAsync(Guid id)
        {
            return await _dataSource.VendorDocuments
                .Where(r => r.VendorGuid == id).ToListAsync();
        }
        public async Task<IList<Vendor>> GetVendorsAsync(int skip, int take, DataRequest<Vendor> request)
        {
            IQueryable<Vendor> items = GetVendors(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Vendor
                {
                    VendorId = source.VendorId,
                    VendorGuid = source.VendorGuid,
                    VendorSalutation = source.VendorSalutation,
                    VendorLastName = source.VendorLastName,
                    VendorName = source.VendorName,
                    VendorAlias = source.VendorAlias,
                    RelativeName = source.RelativeName,
                    RelativeLastName = source.RelativeLastName,
                    RelativeSalutation = source.RelativeSalutation,
                    ContactPerson = source.ContactPerson,
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    City = source.City,
                    PinCode = source.PinCode,
                    PhoneNoIsdCode = source.PhoneNoIsdCode,
                    PhoneNo = source.PhoneNo,
                    email = source.email,
                    PAN = source.PAN,
                    AadharNo = source.AadharNo,
                    GSTIN = source.GSTIN,
                    IsVendorActive = source.IsVendorActive,
                    SalutationType=source.SalutationType,
                    GroupId = source.GroupId??0,
                    GroupName = source.GroupName,
                    BankName = source.BankName,
                    Branch = source.Branch,
                    IFSCCode = source.IFSCCode,
                    AccountNumber = source.AccountNumber
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<IList<Vendor>> GetVendorsAsync(DataRequest<Vendor> request)
        {
            IQueryable<Vendor> items = GetVendors(request);
            return await items.ToListAsync();
        }

        public async Task<int> DeleteVendorAsync(Vendor model)
        {
            _dataSource.Vendors.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

        private IQueryable<Vendor> GetVendors(DataRequest<Vendor> request)
        {
            //IQueryable<Vendor> items = _dataSource.Vendors;
            IQueryable<Vendor> items = from v in _dataSource.Vendors
                                       from g in _dataSource.Groups.Where(x=>x.GroupId==v.GroupId).DefaultIfEmpty()
                                       select new Vendor
                                       {
                                           VendorId = v.VendorId,
                                           VendorGuid = v.VendorGuid,
                                           VendorSalutation = v.VendorSalutation,
                                           VendorLastName = v.VendorLastName,
                                           VendorName = v.VendorName,
                                           VendorAlias = v.VendorAlias,
                                           RelativeName = v.RelativeName,
                                           RelativeLastName = v.RelativeLastName,
                                           RelativeSalutation = v.RelativeSalutation,
                                           ContactPerson = v.ContactPerson,
                                           AddressLine1 = v.AddressLine1,
                                           AddressLine2 = v.AddressLine2,
                                           City = v.City,
                                           PinCode = v.PinCode,
                                           PhoneNoIsdCode = v.PhoneNoIsdCode,
                                           PhoneNo = v.PhoneNo,
                                           email = v.email,
                                           PAN = v.PAN,
                                           AadharNo = v.AadharNo,
                                           GSTIN = v.GSTIN,
                                           IsVendorActive = v.IsVendorActive,
                                           SalutationType = v.SalutationType,
                                           GroupId = v.GroupId??0,
                                           GroupName =(v.GroupId==null || v.GroupId==0)?"": g.GroupName,
                                           BankName = v.BankName,
                                           Branch = v.Branch,
                                           IFSCCode = v.IFSCCode,
                                           AccountNumber = v.AccountNumber
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

        public async Task<int> GetVendorsCountAsync(DataRequest<Vendor> request)
        {
            IQueryable<Vendor> items = _dataSource.Vendors;
          
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
        public async Task<int> UploadVendorDocumentsAsync(List<VendorDocuments> documents)
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

        public async Task<int> DeleteVendorDocumentAsync(VendorDocuments documents)
        {
            _dataSource.VendorDocuments.Remove(documents);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
