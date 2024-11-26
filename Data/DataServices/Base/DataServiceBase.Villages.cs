using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddVillageAsync(Village model)
        {
            if (model == null)
                return 0;

            var entity = new Village()
            {
                VillageId = model.VillageId,
                VillageGuid = model.VillageGuid,
                TalukId = model.TalukId,
                HobliId = model.HobliId,
                VillageName = model.VillageName,
                VillageGMapLink = model.VillageGMapLink,
                VillageIsActive = model.VillageIsActive,

            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<Village> GetVillageAsync(long id)
        {

            return await _dataSource.Villages
                .Where(x => x.VillageId == id)
                .FirstOrDefaultAsync();

        }

        private IQueryable<Village> GetVillages(DataRequest<Village> request)
        {
            IQueryable<Village> items = from v in _dataSource.Villages join
                                        h in _dataSource.Hoblis on v.HobliId equals h.HobliId join
                                        t in _dataSource.Taluks on v.TalukId equals t.TalukId
                                        select (new Village
                                        {
                                            VillageId = v.VillageId,
                                            VillageGuid = v.VillageGuid,
                                            TalukId = v.TalukId,
                                            HobliId = v.HobliId,
                                            VillageName = v.VillageName,
                                            VillageGMapLink = v.VillageGMapLink,
                                            VillageIsActive = v.VillageIsActive,
                                            HobliName = h.HobliName,
                                            TalukName = t.TalukName
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

        public async Task<IList<Village>> GetVillagesAsync(DataRequest<Village> request)
        {
            IQueryable<Village> items = GetVillages(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Village>> GetVillagesAsync(int skip, int take, DataRequest<Village> request)
        {
            IQueryable<Village> items = GetVillages(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Village
                {
                    VillageId = source.VillageId,
                    VillageGuid = source.VillageGuid,
                    TalukId = source.TalukId,
                    HobliId = source.HobliId,
                    VillageName = source.VillageName,
                    VillageGMapLink = source.VillageGMapLink,
                    VillageIsActive = source.VillageIsActive,
                    HobliName = source.HobliName,
                    TalukName = source.TalukName
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetVillagesCountAsync(DataRequest<Village> request)
        {
            IQueryable<Village> items = from v in _dataSource.Villages
                                        join h in _dataSource.Hoblis on v.HobliId equals h.HobliId
                                        join t in _dataSource.Taluks on h.TalukId equals t.TalukId
                                        select v;

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

        public async Task<int> UpdateVillageAsync(Village model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteVillageAsync(Village model)
        {
            _dataSource.Villages.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }

    }
}
