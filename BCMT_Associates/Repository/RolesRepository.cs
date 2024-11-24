using BCMT_Associates.Context;
using BCMT_Associates.Interfaces;
using BCMT_Associates.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BCMT_Associates.Repository
{
    public class RolesRepository : IRolesInterface
    {
        private readonly AppDBContext _dBContext;

        public RolesRepository(AppDBContext appDBContext)
        {
            _dBContext = appDBContext;
        }

        // Get role by ID
        public async Task<List<Roles>> Get(int? Id)
        {
            return await _dBContext.Roles.FromSqlRaw("RoleGet {0}", Id == 0 ? (object)null : Id).ToListAsync();
        }

        public Task<List<Roles>> GetRolesAccess(int? Id)
        {
            throw new NotImplementedException();
        }

        // Get all roles without any filtering
        public async Task<List<Roles>> GetRolesList(int? Id)
        {
            return await _dBContext.Roles.ToListAsync();
        }

        // Get role menu by RoleId
        public async Task<List<RoleMenu>> GetRoleMenu(int? RoleId = null)
        {
            var res = await _dBContext.RoleMenu.FromSqlRaw("RoleMenuGet {0}", RoleId == 0 ? (object)null : RoleId).AsNoTracking().ToListAsync();
            return res;
        }

        // Get menu list
        public async Task<List<Menus>> GetMenu()
        {
            return await _dBContext.Menus.FromSqlRaw("MenuGet").AsNoTracking().ToListAsync();
        }

        // Add or remove role menu entries
        public async Task<string> AddRemove(List<RoleMenu> removalList, List<int> Addroles, int RoleId, Roles roles)
        {
            // Execute role update query
            var result = await _dBContext.Database.ExecuteSqlRawAsync("RoleUpdate {0}, {1}, {2}, {3}", RoleId, roles.Title, roles.Name, roles.Description);

            // Remove role menu entries
            foreach (var item in removalList)
            {
                await _dBContext.Database.ExecuteSqlRawAsync("RoleMenuDelete {0}, {1}", item.RolesId, item.MenusId);
            }

            // Save changes (EF Core does not track changes on ExecuteSqlRawAsync)
            await _dBContext.SaveChangesAsync();

            // Add new role menu entries
            foreach (var menu in Addroles)
            {
                await _dBContext.Database.ExecuteSqlRawAsync("RoleMenuAdd {0}, {1}", RoleId, menu);
            }

            return "Successfully Updated Role!";
        }

        // Add new role
        public async Task<string> AddRoles(string Addroles, Roles roles)
        {
            var result = await _dBContext.Database.ExecuteSqlRawAsync("RoleAdd {0}, {1}, {2}, {3}", roles.Title, roles.Name, roles.Description, Addroles);
            return "Added Role Successfully!";
        }

        // Delete a role
        public async Task<string> DeleteRole(int? RoleId)
        {
            var result = await _dBContext.Database.ExecuteSqlRawAsync("RoleDelete {0}", RoleId);
            return "Role Deleted Successfully!";
        }
    }
}
