using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddUserRoleAsync(UserRole model)
        {
            if (model == null)
                return 0;

            var entity = new UserRole()
            {
                UserInfoId = model.UserInfoId,
                RoleId = model.RoleId,
                Created = model.Created,
                CreatedBy = model.CreatedBy,
                Updated = model.Updated,
                UpdatedBy = model.UpdatedBy,
        };
            _dataSource.Entry(entity).State = EntityState.Added;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> AddUserRoleForUserAsync(List<UserRole> models,int userId)
        {
            if (models == null)
                return 0;
            try
            {

                var oldRoles = _dataSource.UserRoles.Where(x => x.UserInfoId == userId).ToList();
                if (oldRoles != null)
                {
                    _dataSource.UserRoles.RemoveRange(oldRoles);
                    await _dataSource.SaveChangesAsync();
                }
                foreach (var model in models)
                {
                    if (!model.IsSelected)
                        continue;

                    var entity = new UserRole()
                    {
                        UserInfoId = userId,
                        RoleId = model.RoleId,
                        Created = DateTime.Now,
                        CreatedBy = model.CreatedBy,
                        Updated = DateTime.Now,
                        UpdatedBy = model.UpdatedBy,
                    };
                    _dataSource.Entry(entity).State = EntityState.Added;
                }
                int res = await _dataSource.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) {
                throw ex;
            }
        }


        public async Task<UserRole> GetUserRoleAsync(long id)
        {
            return await _dataSource.UserRoles.Where(r => r.UserRoleId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<UserRole>> GetUserRolesAsync(DataRequest<UserRole> request)
        {
            IQueryable<UserRole> items = GetUserRoles(request);
            return await items.ToListAsync();
        }

        public async Task<IList<UserRole>> GetUserRolesAsync(int skip, int take, DataRequest<UserRole> request)
        {
            IQueryable<UserRole> items = GetUserRoles(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new UserRole
                {
                    UserInfoId = source.UserInfoId,
                    UserRoleId = source.UserRoleId,
                    RoleId = source.RoleId,
                    Created = source.Created,
                    CreatedBy = source.CreatedBy,
                    Updated = source.Updated,
                    UpdatedBy = source.UpdatedBy,
        })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<UserRole> GetUserRoles(DataRequest<UserRole> request)
        {
            IQueryable<UserRole> items = _dataSource.UserRoles;

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

        public async Task<int> GetUserRolesCountAsync(DataRequest<UserRole> request)
        {
            IQueryable<UserRole> items = _dataSource.UserRoles;

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

        public async Task<int> UpdateUserRoleAsync(UserRole userRole)
        {
            _dataSource.Entry(userRole).State = EntityState.Modified;
            userRole.SearchTerms = userRole.BuildSearchTerms();
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteUserRoleAsync(UserRole userRole)
        {
            _dataSource.UserRoles.Remove(userRole);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<IList<UserRole>> GetUserRolesForUserAsync(int userId)
        {
            var list= await(from role  in _dataSource.Roles
           from userRole in _dataSource.UserRoles.Where(x=>x.RoleId==role.RoleId && x.UserInfoId==userId).DefaultIfEmpty()
                select(new UserRole
                {
                    UserInfoId =(userRole==null|| userRole.UserInfoId==0)?0 : userRole.UserInfoId,
                    UserRoleId = (userRole == null || userRole.UserRoleId == 0) ? 0 : userRole.UserRoleId,
                    RoleId = role.RoleId,
                    Name=role.Name,
                    IsSelected= (userRole == null || userRole.UserRoleId == 0) ? false : true
                })).ToListAsync();

            return list;
        }
    }
}
