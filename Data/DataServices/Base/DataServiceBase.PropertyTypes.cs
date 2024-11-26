using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPropertyTypeAsync(PropertyType model)
        {

            if (model == null)
                return 0;

            var entity = new PropertyType()
            {
                PropertyTypeId = model.PropertyTypeId,
                PropertyTypeGuid = model.PropertyTypeGuid,
                PropertyTypeText = model.PropertyTypeText,
                PropertyTypeIsActive = model.PropertyTypeIsActive,
        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<PropertyType> GetPropertyTypeAsync(long id)
        {
            
                return await _dataSource.PropertyTypes
                    .Where(x => x.PropertyTypeId == id).FirstOrDefaultAsync();
           
        }

        public async Task<IList<PropertyType>> GetPropertyTypesAsync(DataRequest<PropertyType> request)
        {
            IQueryable<PropertyType> items = GetPropertyTypes(request);
            return await items.ToListAsync();
        }

        private IQueryable<PropertyType> GetPropertyTypes(DataRequest<PropertyType> request)
        {
            IQueryable<PropertyType> items = _dataSource.PropertyTypes;

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

        public async Task<IList<PropertyType>> GetPropertyTypesAsync(int skip, int take, DataRequest<PropertyType> request)
        {
            IQueryable<PropertyType> items = GetPropertyTypes(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new PropertyType
                {
                    PropertyTypeId = source.PropertyTypeId,
                    PropertyTypeGuid = source.PropertyTypeGuid,
                    PropertyTypeText = source.PropertyTypeText,
                    PropertyTypeIsActive = source.PropertyTypeIsActive,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetPropertyTypesCountAsync(DataRequest<PropertyType> request)
        {
            IQueryable<PropertyType> items = _dataSource.PropertyTypes;

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

        public async Task<int> UpdatePropertyTypeAsync(PropertyType model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeletePropertyTypeAsync(PropertyType model)
        {
            _dataSource.PropertyTypes.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
