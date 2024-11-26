using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddRoleAsync(Role model)
        {
            if (model == null)
                return 0;

            var entity = new Role()
            {
                Name = model.Name,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Updated = model.Updated,
                UpdatedBy = model.UpdatedBy,
            };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<Role> GetRoleAsync(long id)
        {
            return await _dataSource.Roles.Where(r => r.RoleId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Role>> GetRolesAsync(DataRequest<Role> request)
        {
            IQueryable<Role> items = GetRoles(request);
            return await items.ToListAsync();
        }

        public async Task<IList<Role>> GetRolesAsync(int skip, int take, DataRequest<Role> request)
        {
            IQueryable<Role> items = GetRoles(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new Role
                {
                    RoleId = source.RoleId,
                    Name = source.Name,
                    Created = source.Created,
                    CreatedBy = source.CreatedBy,
                    Updated = source.Updated,
                    UpdatedBy = source.UpdatedBy,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Role> GetRoles(DataRequest<Role> request)
        {
            IQueryable<Role> items = _dataSource.Roles;

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

        public async Task<int> GetRolesCountAsync(DataRequest<Role> request)
        {
            IQueryable<Role> items = _dataSource.Roles;

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

        public async Task<int> UpdateRoleAsync(Role role)
        {

            _dataSource.Entry(role).State = EntityState.Modified;


            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteRoleAsync(Role role)
        {
            _dataSource.Roles.Remove(role);
            return await _dataSource.SaveChangesAsync();
        }

        public IList<RolePermission> GetRolePermisions(int roleId)
        {
         
            return _dataSource.RolePermissions.Where(x => x.RoleInfoId == roleId).ToList();
        }

        public async Task<IList<Role>> GetRolesAsync()
        {
            var records = await _dataSource.Roles
                .Select(source => new Role
                {
                    RoleId = source.RoleId,
                    Name = source.Name,
                    Created = source.Created,
                    CreatedBy = source.CreatedBy,
                    Updated = source.Updated,
                    UpdatedBy = source.UpdatedBy,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }
    }
}
