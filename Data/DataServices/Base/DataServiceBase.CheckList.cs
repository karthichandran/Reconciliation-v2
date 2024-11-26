using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
   partial class DataServiceBase
    {
        public async Task<int> AddCheckListAsync(CheckList model)
        {
            if (model == null)
                return 0;

            var entity = new CheckList()
            {
                CheckListId = model.CheckListId,
                CheckListGuid = model.CheckListGuid,
                CheckListName = model.CheckListName,
                CheckListIsActive = model.CheckListIsActive,

        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<CheckList> GetCheckListAsync(long id)
        {
            return await _dataSource.CheckLists
                .Where(x => x.CheckListId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<CheckList>> GetCheckListsAsync(DataRequest<CheckList> request)
        {
            IQueryable<CheckList> items = GetCheckLists(request);
            return await items.ToListAsync();
        }

        private IQueryable<CheckList> GetCheckLists(DataRequest<CheckList> request)
        {
            IQueryable<CheckList> items = _dataSource.CheckLists;

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

        public async Task<IList<CheckList>> GetCheckListsAsync(int skip, int take, DataRequest<CheckList> request)
        {
            IQueryable<CheckList> items = GetCheckLists(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new CheckList
                {
                    CheckListId = source.CheckListId,
                    CheckListGuid = source.CheckListGuid,
                    CheckListName = source.CheckListName,
                    CheckListIsActive = source.CheckListIsActive,
        })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetCheckListsCountAsync(DataRequest<CheckList> request)
        {
            IQueryable<CheckList> items = _dataSource.CheckLists;

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

        public async Task<int> UpdateCheckListAsync(CheckList model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteCheckListAsync(CheckList model)
        {
            _dataSource.CheckLists.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
