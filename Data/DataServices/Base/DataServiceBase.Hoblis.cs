using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddHobliAsync(Hobli model)
        {

            if (model == null)
                return 0;

            var entity = new Hobli()
            {
                HobliId = model.HobliId,
                HobliGuid = model.HobliGuid,
                TalukId = model.TalukId,
                HobliName = model.HobliName,
                HobliGMapLink = model.HobliGMapLink,
                HobliIsActive = model.HobliIsActive,

            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;

        }

        public async Task<Hobli> GetHobliAsync(long id)
        {
           
                return await _dataSource.Hoblis
                                        .Where(x => x.HobliId == id)
                                        .FirstOrDefaultAsync();
           
        }

        public async Task<IList<Hobli>> GetHoblisAsync(DataRequest<Hobli> request)
        {
            IQueryable<Hobli> items = GetHoblis(request);
            return await items.ToListAsync();
        }

        private IQueryable<Hobli> GetHoblis(DataRequest<Hobli> request)
        {
            IQueryable<Hobli> items = from h in _dataSource.Hoblis
                                      join t in _dataSource.Taluks on h.TalukId equals t.TalukId
                                      select (new Hobli
                                      {
                                          HobliId = h.HobliId,
                                          HobliGuid = h.HobliGuid,
                                          HobliGMapLink = h.HobliGMapLink,
                                          TalukId = h.TalukId,
                                          HobliName = h.HobliName,
                                          TalukName = t.TalukName,
                                          HobliIsActive=h.HobliIsActive
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

        public async Task<IList<Hobli>> GetHoblisAsync(int skip, int take, DataRequest<Hobli> request)
        {
            IQueryable<Hobli> items = GetHoblis(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Hobli
                {
                    HobliId = source.HobliId,
                    HobliGuid = source.HobliGuid,
                    TalukId = source.TalukId,
                    HobliName = source.HobliName,
                    HobliGMapLink = source.HobliGMapLink,
                    HobliIsActive = source.HobliIsActive,
                    TalukName = source.TalukName
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetHoblisCountAsync(DataRequest<Hobli> request)
        {
            IQueryable<Hobli> items = from h in _dataSource.Hoblis
                                      join t in _dataSource.Taluks on h.TalukId equals t.TalukId
                                      select h;

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

        public async Task<int> UpdateHobliAsync(Hobli model)
        {
            try
            {
                _dataSource.Entry(model).State = EntityState.Modified;
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeleteHobliAsync(Hobli model)
        {
            _dataSource.Hoblis.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }


    }
}
