using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddTalukAsync(Taluk model)
        {
            if (model == null)
                return 0;

            var entity = new Taluk()
            {
                TalukId = model.TalukId,
                TalukGuid = model.TalukGuid,
                TalukName = model.TalukName,
                TalukGMapLink = model.TalukGMapLink,
                TalukIsActive = model.TalukIsActive
            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<Taluk> GetTalukAsync(long id)
        {
            return await _dataSource.Taluks.Where(r => r.TalukId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Taluk>> GetTaluksAsync(DataRequest<Taluk> request)
        {
            IQueryable<Taluk> items = GetTaluks(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Taluk>> GetTaluksAsync(int skip, int take, DataRequest<Taluk> request)
        {
            IQueryable<Taluk> items = GetTaluks(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Taluk
                {
                    TalukId = source.TalukId,
                    TalukGuid = source.TalukGuid,
                    TalukName = source.TalukName,
                    TalukGMapLink = source.TalukGMapLink,
                    TalukIsActive = source.TalukIsActive
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Taluk> GetTaluks(DataRequest<Taluk> request)
        {
            IQueryable<Taluk> items = _dataSource.Taluks;

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

        public async Task<int> GetTaluksCountAsync(DataRequest<Taluk> request)
        {
            IQueryable<Taluk> items = _dataSource.Taluks;

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

        public async Task<int> UpdateTalukAsync(Taluk taluk)
        {
            if (taluk.TalukId > 0)
            {
                _dataSource.Entry(taluk).State = EntityState.Modified;
            }
            else
            {
                taluk.TalukGuid = Guid.NewGuid();
                //Company.CreatedOn = DateTime.UtcNow;
                _dataSource.Entry(taluk).State = EntityState.Added;
            }
            // Company.LastModifiedOn = DateTime.UtcNow;
            taluk.SearchTerms = taluk.BuildSearchTerms();
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteTalukAsync(Taluk taluk)
        {
            _dataSource.Taluks.Remove(taluk);
            return await _dataSource.SaveChangesAsync();
        }

      
    }
}
