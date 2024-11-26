using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<Company> GetCompanyAsync(long id)
        {
            var company = await _dataSource.Companies.Where(r => r.CompanyID == id).FirstOrDefaultAsync();
            if (company.CompanyGuid != null)
            {
                var companyDocs =await GetCompanyDocumentsAsync(company.CompanyGuid);
                if (companyDocs.Any())
                {
                    company.CompanyDocuments = companyDocs;
                }

            }
          
            return company;
        }

        public async Task<List<CompanyDocuments>> GetCompanyDocumentsAsync(Guid id)
        {
            return await  _dataSource.CompanyDocuments
                .Where(r => r.CompanyGuid == id).ToListAsync();
        }

        public async Task<IList<Company>> GetCompaniesAsync(int skip, int take, DataRequest<Company> request)
        {
            IQueryable<Company> items = GetCompanies(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Company
                {
                    CompanyID = source.CompanyID,
                    CompanyGuid = source.CompanyGuid,
                    City = source.City,
                    GSTIN = source.GSTIN,
                    IsActive = source.IsActive,
                    Name = source.Name,
                    PAN = source.PAN,
                    PhoneNo = source.PhoneNo,
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    PhoneNoIsdCode = source.PhoneNoIsdCode,
                    Pincode = source.Pincode,
                    Email = source.Email,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Company> GetCompanies(DataRequest<Company> request)
        {
            IQueryable<Company> items = _dataSource.Companies;

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

        //todo understand the need for this method
        public async Task<IList<Company>> GetCompanyKeysAsync(int skip, int take, DataRequest<Company> request)
        {
            IQueryable<Company> items = GetCompanies(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Select(r => new Company
                {
                    CompanyID = r.CompanyID,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetCompaniesCountAsync(DataRequest<Company> request)
        {
            IQueryable<Company> items = _dataSource.Companies;

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

        public async Task<int> UpdateCompanyAsync(Company company)
        {
            try
            {
                ICollection<CompanyDocuments> docs = company.CompanyDocuments;
                company.CompanyDocuments = null;
                if (company.CompanyID > 0)
                {
                    _dataSource.Entry(company).State = EntityState.Modified;
                }
                else
                {
                    company.CompanyGuid = Guid.NewGuid();
                    _dataSource.Entry(company).State = EntityState.Added;
                }

                company.SearchTerms = company.BuildSearchTerms();
                int res = await _dataSource.SaveChangesAsync();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (doc.CompanyBlobId == 0)
                        {
                            doc.CompanyGuid = company.CompanyGuid;
                            _dataSource.CompanyDocuments.Add(doc);
                           
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

        public async Task<int> DeleteCompanyAsync(params Company[] company)
        {
            _dataSource.Companies.RemoveRange(company);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> UploadCompanyDocumentsAsync(List<CompanyDocuments> documents) {
            try
            {
                foreach (var doc in documents)
                {
                    _dataSource.Entry(doc).State = EntityState.Added;
                }
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeleteCompanyDocumentAsync(CompanyDocuments documents)
        {
            _dataSource.CompanyDocuments.Remove(documents);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<IList<Company>> GetCompaniesAsync()
        {
            var items = _dataSource.Companies;

            var result = await items.Select(source => new Company
            {
                CompanyID = source.CompanyID,
                CompanyGuid = source.CompanyGuid,
                City = source.City,
                GSTIN = source.GSTIN,
                IsActive = source.IsActive,
                Name = source.Name,
                PAN = source.PAN,
                PhoneNo = source.PhoneNo,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                PhoneNoIsdCode = source.PhoneNoIsdCode,
                Pincode = source.Pincode,
                Email = source.Email,
            }).AsNoTracking()
              .ToListAsync();

            return result;
        }
    }
}
