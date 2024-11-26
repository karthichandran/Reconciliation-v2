using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
   partial class DataServiceBase
    {
        public async Task<int> AddDocumentTypeAsync(DocumentType model)
        {
            if (model == null)
                return 0;

            var entity = new DocumentType()
            {
                DocumentTypeId = model.DocumentTypeId,
                DocumentTypeName = model.DocumentTypeName,
                IsDocumentTypeActive = model.IsDocumentTypeActive,
        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<DocumentType> GetDocumentTypeAsync(long id)
        {
            return await _dataSource.DocumentTypes
                .Where(r => r.DocumentTypeId == id).FirstOrDefaultAsync();
        }

        private IQueryable<DocumentType> GetDocumentTypes(DataRequest<DocumentType> request)
        {
            IQueryable<DocumentType> items = _dataSource.DocumentTypes;

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


        public async Task<IList<DocumentType>> GetDocumentTypesAsync(DataRequest<DocumentType> request)
        {
            IQueryable<DocumentType> items = GetDocumentTypes(request);
            return await items.ToListAsync();
        }

        public async Task<IList<DocumentType>> GetDocumentTypesAsync(int skip, int take, DataRequest<DocumentType> request)
        {
            IQueryable<DocumentType> items = GetDocumentTypes(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new DocumentType
                {
                    DocumentTypeId = source.DocumentTypeId,
                    DocumentTypeName = source.DocumentTypeName,
                    IsDocumentTypeActive = source.IsDocumentTypeActive,
        })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetDocumentTypesCountAsync(DataRequest<DocumentType> request)
        {
            IQueryable<DocumentType> items = _dataSource.DocumentTypes;

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

        public async Task<int> UpdateDocumentTypeAsync(DocumentType model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteDocumentTypeAsync(DocumentType model)
        {
            var isUseIn = _dataSource.PropertyDocumentType.Where(x => x.DocumentTypeId == model.DocumentTypeId).Select(x => x).Any();
            if (isUseIn)
                return -1;
            _dataSource.DocumentTypes.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
