using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> AddRolePermissionAsync(RolePermission model)
        {
           
                if (model == null)
                    return 0;

                var entity = new RolePermission()
                {
                    RolePermissionId = model.RolePermissionId,
                    RoleInfoId = model.RoleInfoId,
                    ScreenId = model.ScreenId,
                    OptionId = model.OptionId,
                    CanView = model.CanView,
        };
                _dataSource.Entry(entity).State = EntityState.Added;
                int res = await _dataSource.SaveChangesAsync();
                return res;
           
        }

        public async Task<int> AddRolePermissionsAsync(List<RolePermission> models)
        {

            if (models.Count == 0)
                return 0;
            var existingPermission = await _dataSource.RolePermissions.Where(x => x.RoleInfoId == models[0].RoleInfoId).ToListAsync();
            _dataSource.RolePermissions.RemoveRange(existingPermission);
            _dataSource.SaveChanges();

            _dataSource.RolePermissions.AddRange(models);
            int res = await _dataSource.SaveChangesAsync();
            return res;

        }


        public async Task<RolePermission> GetRolePermissionAsync(long id)
        {
            return await _dataSource.RolePermissions
                .Where(x => x.RolePermissionId == id).FirstOrDefaultAsync();

        }

        public async Task<List<RolePermission>> GetRolePermissionsByRoleIDAsync(int roleId) {

            try
            {
                var existingPermission = await _dataSource.RolePermissions.Where(x => x.RoleInfoId == roleId).ToListAsync();
                if (existingPermission.Count > 0)
                {
                    var models = await (from sc in _dataSource.ScreenList
                                        from per in _dataSource.RolePermissions.Where(x => x.RoleInfoId == roleId && x.ScreenId == sc.ScreenId).DefaultIfEmpty()

                                        select (new RolePermission
                                        {
                                            ScreenId = sc.ScreenId,
                                            ScreenName = sc.ScreenName,
                                            RoleInfoId = roleId,
                                            RolePermissionId = per == null ? 0 : per.RolePermissionId,
                                            OptionId = per.OptionId==true?true:false,
                                            CanView = per.CanView == true ? true : false
                                        })).ToListAsync();

                    //RoleInfoId = 0,
                    //                        RolePermissionId = per == null ? 0 : per.RolePermissionId,
                    //                        OptionId = per == null ? false : per.OptionId,
                    //                        CanView = per == null ? false : per.CanView

                    //var models =await (from sc in _dataSource.ScreenList
                    //              join per in _dataSource.RolePermissions.Where(x => x.RoleInfoId == roleId) on sc.ScreenId equals per.ScreenId into roleList
                    //              from pr in roleList.DefaultIfEmpty()
                    //              select (new RolePermission
                    //              {
                    //                  ScreenId = sc.ScreenId,
                    //                  RolePermissionId = pr.RolePermissionId,
                    //                  RoleInfoId = pr.RoleInfoId,
                    //                  ScreenName = sc.ScreenName,
                    //                  OptionId = pr.OptionId,
                    //                  CanView = pr.CanView
                    //              })).ToListAsync();
                    return models;
                }
                else
                {
                    var models = await _dataSource.ScreenList.Select(x => new RolePermission { ScreenId = x.ScreenId, ScreenName = x.ScreenName }).ToListAsync();
                    return models;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        
        }

        public async Task<IList<RolePermission>> GetRolePermissionsAsync(DataRequest<RolePermission> request)
        {
            IQueryable<RolePermission> items = GetRolePermissions(request);
            return await items.ToListAsync();
        }

        private IQueryable<RolePermission> GetRolePermissions(DataRequest<RolePermission> request)
        {
            IQueryable<RolePermission> items = _dataSource.RolePermissions;
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


        public async Task<IList<RolePermission>> GetRolePermissionsAsync(int skip, int take, DataRequest<RolePermission> request)
        {
            IQueryable<RolePermission> items = GetRolePermissions(request);
            var records = await items.Skip(skip).Take(take)
                .Select(source => new RolePermission
                {
                    RolePermissionId = source.RolePermissionId,
                    RoleInfoId = source.RoleInfoId,
                    ScreenId = source.ScreenId,
                    OptionId = source.OptionId,
                    CanView = source.CanView,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<int> GetRolePermissionsCountAsync(DataRequest<RolePermission> request)
        {
            IQueryable<RolePermission> items = _dataSource.RolePermissions;

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

        public async Task<int> UpdateRolePermissionAsync(RolePermission model)
        {
            _dataSource.Entry(model).State = EntityState.Modified;
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteRolePermissionAsync(RolePermission model)
        {
            _dataSource.RolePermissions.Remove(model);
            return await _dataSource.SaveChangesAsync();
        }
    }
}
