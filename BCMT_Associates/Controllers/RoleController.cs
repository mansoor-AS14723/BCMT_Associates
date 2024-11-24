
using BCMT_Associates.Helpers;
using BCMT_Associates.Interfaces;
using BCMT_Associates.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace BCMT_Associates.Controllers
{
   
    public class RoleController : Controller
    {
        private readonly IMvcControllerDiscovery _mvcControllerDiscovery;
       
        private readonly IRolesInterface _roles;


        public RoleController(IMvcControllerDiscovery mvcControllerDiscovery,  IRolesInterface roles)
        {
            _mvcControllerDiscovery = mvcControllerDiscovery;
            
            _roles = roles;
        }
        [AuthorizedAction]
        [DisplayName("Role List")]
        public async Task<ActionResult> Index(int? Id = null)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            
            var roles = await _roles.Get(Id);

            return View(roles);
        }

        // GET: Role/Create
        [AuthorizedAction]
        public async Task<IActionResult> Create()
        {
            //ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            DataSet ds = new DataSet();
            var menus = await _roles.GetMenu();
            var Selectedmenus = await _roles.GetRoleMenu(null);
            List<string> menus_id = Selectedmenus.Select(x => x.MenusId.ToString()).ToList();
            ds = Helper.ToDataSet(menus);
            DataTable table = ds.Tables[0];
            DataRow[] parentMenus = table.Select("ParentId = 0");

            var sb = new StringBuilder();
            string unorderedList = Helper.GenerateUL(parentMenus, table, sb, null);
            ViewBag.menu = unorderedList;

            return View();
        }

        [HttpPost]
        [AuthorizedAction]
        public async Task<IActionResult> Create(Roles collection)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                
                var rolemenu = Request.Form["roleMenus"];
                var msg = await _roles.AddRoles(rolemenu, collection);
             

                return Json("");

            }
            catch (Exception ex)
            {
                Common.SaveExceptionLog(ex);
                return Json(new { status = false, message = "Error" });
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(RoleViewModel viewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
        //        return View(viewModel);
        //    }

        //    var role = new ApplicationRole { Name = viewModel.Name };
        //    if (viewModel.SelectedControllers != null && viewModel.SelectedControllers.Any())
        //    {
        //        foreach (var controller in viewModel.SelectedControllers)
        //            foreach (var action in controller.Actions)
        //                action.ControllerId = controller.Id;

        //        var accessJson = JsonConvert.SerializeObject(viewModel.SelectedControllers);
        //        role.Access = accessJson;
        //    }

        //    var result = await _roleManager.CreateAsync(role);
        //    if (result.Succeeded)
        //        return RedirectToAction(nameof(Index));

        //    foreach (var error in result.Errors)
        //        ModelState.AddModelError("", error.Description);

        //    ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();

        //    return View(viewModel);
        //}

        // GET: Role/Edit/5
        [AuthorizedAction]
        public async Task<IActionResult> Edit(int? id = null)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _roles.Get(id);
            if (roles == null)
            {
                return NotFound();
            }

            DataSet ds = new DataSet();

            var menus = await _roles.GetMenu();
            var Selectedmenus = await _roles.GetRoleMenu(id);
            List<string> menus_id = Selectedmenus.Select(x => x.MenusId.ToString()).ToList();
            ds = Helper.ToDataSet(menus);
            DataTable table = ds.Tables[0];
            DataRow[] parentMenus = table.Select("ParentId = 0");

            //var menus = await _roles.GetRoleMenu(id, Convert.ToInt32(Helper.getClaims(claimsIdentity, "OrganizationId")));
            //List<string> menus_id = menus.Select(x=>x.MenusId.ToString()).ToList();
            //ds = Helper.ToDataSet(menus);
            //DataTable table = ds.Tables[0];
            //DataRow[] parentMenus = table.Select("ParentId = 0");

            var sb = new StringBuilder();
            string unorderedList = Helper.GenerateUL(parentMenus, table, sb, menus_id);
            ViewBag.menu = unorderedList;

            return View(roles.FirstOrDefault());
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] Roles roles)
        {
            if (id != roles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                  //  _context.Update(roles);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roles);
        }



        public async Task<IActionResult> Update(int id, List<int> roles,Roles collection)
        {
            try {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;

                var organizationid = Convert.ToInt32(Helper.getClaims(claimsIdentity, "OrganizationId"));

                var tempRoles = await _roles.GetRoleMenu(id);
                var msg = await _roles.AddRemove(tempRoles, roles, id,collection);

                return Json(new { status = true, message = msg });

            } catch (Exception ex) {
                Common.SaveExceptionLog(ex);
                return Json(new { status = false, message = "Error" });
            }
            
        }

        //private bool RolesExists(int id)
        //{
        //    return _context.Roles.Any(e => e.Id == id);
        //}

        // POST: Role/Delete/5
        [HttpPost]
        [AuthorizedAction]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;

                var organizationid = Convert.ToInt32(Helper.getClaims(claimsIdentity, "OrganizationId"));

                var tempRoles = await _roles.GetRoleMenu(id);
                var msg = await _roles.DeleteRole(id);

                return Json(new { status = true, message = msg });

            }
            catch (Exception ex)
            {
                Common.SaveExceptionLog(ex);
                return Json(new { status = false, message = "Error" });
            }
        }
    }


}
