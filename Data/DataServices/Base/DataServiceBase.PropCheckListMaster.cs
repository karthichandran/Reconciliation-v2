using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddPropCheckListMasterAsync(PropCheckListMaster model)
        {
            if (model == null)
                return 0;

            var entity = new PropCheckListMaster()
            {
                PropCheckListMasterId = model.PropCheckListMasterId,
                PropCheckListMasterGuid = model.PropCheckListMasterGuid,
                PropCheckListMasterDescription = model.PropCheckListMasterDescription,

        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<PropCheckListMaster> GetPropCheckListMasterAsync(long id)
        {
            return await _dataSource.PropCheckListMasters
                .Where(x => x.PropCheckListMasterId == id).FirstOrDefaultAsync();

        }

        private IQueryable<PropCheckListMaster> GetPropCheckListMasters(DataRequest<PropCheckListMaster> request)
        {
            IQueryable<PropCheckListMaster> items = _dataSource.PropCheckListMasters;

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


        public async Task<IList<PropCheckListMaster>> GetPropCheckListMastersAsync(DataRequest<PropCheckListMaster> request)
        {
            IQueryable<PropCheckListMaster> items = GetPropCheckListMasters(request);
            return await items.ToListAsync();
        }

        public async Task<IList<PropCheckListMaster>> GetPropCheckListMastersAsync(int skip, int take, DataRequest<PropCheckListMaster> request)
        {
            IQueryable<PropCheckListMaster> items = GetPropCheckListMasters(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new PropCheckListMaster
                {
                    PropCheckListMasterId = source.PropCheckListMasterId,
                    PropCheckListMasterGuid = source.PropCheckListMasterGuid,
                    PropCheckListMasterDescription = source.PropCheckListMasterDescription,

        })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetPropCheckListMastersCountAsync(DataRequest<PropCheckListMaster> request)
        {
            IQueryable<PropCheckListMaster> items = _dataSource.PropCheckListMasters;

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

        public async Task<int> UpdatePropCheckListMasterAsync(PropCheckListMaster model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeletePropCheckListMasterAsync(PropCheckListMaster model)
        {
            _dataSource.PropCheckListMasters.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
