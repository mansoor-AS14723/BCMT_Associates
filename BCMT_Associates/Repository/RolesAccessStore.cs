using DynamicAuthorization.Mvc.Core;
using BCMT_Associates.Context;
using BCMT_Associates.Interfaces;
using BCMT_Associates.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS.Repository
{
    public class RoleAccessStore : IRoleAccessStore
    {
        AppDBContext _dBContext;
        public RoleAccessStore(AppDBContext appDBContext)
        {

            this._dBContext = appDBContext;
        }

        public Task<bool> AddRoleAccessAsync(RoleAccess roleAccess)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditRoleAccessAsync(RoleAccess roleAccess)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Roles>> Get(int? Id, int ? OrganizationId = null)
        {
            return await _dBContext.Roles.FromSqlRaw("RoleGet {0}", Id == 0 ? null : Id, OrganizationId == 0 ? null : OrganizationId).ToListAsync();
        }

        public Task<RoleAccess> GetRoleAccessAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Roles>> GetRolesList(int? Id, int? OrganizationId = null)
		{
			return await _dBContext.Roles.ToListAsync() ;
		}

        public Task<bool> HasAccessToActionAsync(string actionId, params string[] roles)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveRoleAccessAsync(string roleId)
        {
            throw new NotImplementedException();
        }
    }
}
