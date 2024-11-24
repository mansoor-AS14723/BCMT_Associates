using BCMT_Associates.Models;

namespace BCMT_Associates.Interfaces
{
    public interface IRolesInterface
    {
         Task<List<Roles>> Get(int? Id);
		Task<List<Roles>> GetRolesList(int? Id);
        Task<List<Roles>> GetRolesAccess(int? Id);
        Task<List<RoleMenu>> GetRoleMenu(int? RoleId = null);
        Task<List<Menus>> GetMenu();
        Task<string> AddRemove(List<RoleMenu> removalList, List<int> Addroles, int RoleId, Roles roles);
        Task<string> AddRoles(string Addroles, Roles roless);

        Task<string> DeleteRole(int? RoleId);
    }


}
