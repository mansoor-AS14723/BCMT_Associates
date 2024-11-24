using BCMT_Associates.Models;
using BCMT_Associates.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using BCMT_Associates.Interfaces;
using BCMT_Associates.Helpers;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Text;
using BCMT_Associates.Context;
using BCMT_Associates.Services;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace BCMT_Associates.Controllers
{
    
    public class UserController : Controller
    {
        IUserInterface _user;
        IConfiguration _configuration;
        
        IRolesInterface _roles;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IOriginSettings _originSettings;

        public UserController(IUserInterface user, IConfiguration config,  IRolesInterface roles, IWebHostEnvironment webHostEnvironment, IOptions<IOriginSettings> originSettings)
        {
            _user = user;
            _configuration = config;
           
            _roles = roles;
            this.webHostEnvironment = webHostEnvironment;
            _originSettings = originSettings.Value;
        }
       
        [AuthorizedAction]
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            ViewBag.RoleId = new SelectList(await _roles.Get(null), "Id", "Name");
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Get(int? Id = null)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
               
                return Json(await _user.Get(Id));
            }
            catch (Exception ex)
            {
                Common.SaveExceptionLog(ex);
                return Json("Error");
            }
        }

        [AuthorizedAction]
        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                user.CreatedBy = Convert.ToInt32(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                //user.OrganizationId = Convert.ToInt32(Helper.getClaims(claimsIdentity, "OrganizationId"));
                //user.OrganizationDetailId = user.OrganizationDetailId == null ? Convert.ToInt32(Helper.getClaims(claimsIdentity, "OrganizationDetailId")) : user.OrganizationDetailId;
                if (Request.Form.Files.Count() > 0)
                {
                    UploadFileService uploadFileService = new UploadFileService(webHostEnvironment);
                    var result = await uploadFileService.UploadedFile(Request.Form.Files, "UserProfile");
                    user.ProfilePicture = (byte[]?)result[0];

                }
                return Json(await _user.Add(user));
            }
            catch (Exception ex)
            {
                Common.SaveExceptionLog(ex);
                return Json("Error");
            }

        }

        [AuthorizedAction]
        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                user.UpdatedBy = Convert.ToInt32(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                if (Request.Form.Files.Count() > 0)
                {
                    UploadFileService uploadFileService = new UploadFileService(webHostEnvironment);
                    var result = await uploadFileService.UploadedFile(Request.Form.Files, "UserProfile");
                    user.ProfilePicture = (byte[]?)result[0];

                }

                else
                {
                    var result = await _user.Get(user.Id);
                    user.ProfilePicture = result.FirstOrDefault().ProfilePicture;
                }
                return Json(await _user.Update(user));
            }
            catch (Exception ex)
            {
                Common.SaveExceptionLog(ex);
                return Json("Error");
            }

        }

        [AuthorizedAction]
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                return Json(await _user.Delete(Id,0));

            }
            catch (Exception ex)
            {
                Common.SaveExceptionLog(ex);
                return Json("Error");

            }

        }

       
        public ActionResult Login()
        {
            return View();
        }

       
       
        public async Task<IActionResult> LoginUser(User user)
        {
            try
            {
                var origin = HttpContext.Request.Headers["Origin"].ToString();
                Common.SaveActivityLog(origin);
                if (_originSettings.Origins.TryGetValue(origin, out var value))
                {
                    // value will be "1" if the origin matches
                    var OrganizationIddd = Convert.ToInt32(value);
                }
                var userInfo = await (_user.LoginUser(user));
                if (userInfo != null)
                {
                    if (userInfo.ProfilePicture != null)
                    {
						HttpContext.Session.SetString("ProfilePicture", Convert.ToBase64String(userInfo.ProfilePicture));
					}
                    
                    
                    var claims = Common.GetClaims(userInfo, _configuration);
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties();

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
                    var accessToken = Common.GenerateToken(_configuration, claims);



                    #region Dynamic Roles

                    HttpContext.Session.SetString("email", userInfo.Email);
                    HttpContext.Session.SetInt32("id", userInfo.Id);
                    HttpContext.Session.SetInt32("RoleId", (int)userInfo.RoleId);
                    HttpContext.Session.SetString("name", userInfo.FirstName);

                    int roleId = (int)HttpContext.Session.GetInt32("RoleId");
                    var menus = await _roles.GetRoleMenu(roleId);

                    DataSet ds = new DataSet();
                    ds = ToDataSet(menus);
                    DataTable table = ds.Tables[0];
                    DataRow[] parentMenus = table.Select("ParentId = 0");

                    var sb = new StringBuilder();
                    string menuString = GenerateUL(parentMenus, table, sb);
                    HttpContext.Session.SetString("menuString", menuString);
                    HttpContext.Session.SetString("menus", JsonConvert.SerializeObject(menus));
                    #endregion
                    
						return RedirectToAction("Index", "Home");
					
                    

                }
                else
                {
                    ViewBag.Msg = "Incorrect Username/Password";
                    return Json("Incorrect Username/Password");
                }
            }

            catch (Exception ex)
            {
                Common.SaveExceptionLog(ex);
                return BadRequest("Error");
            }

        }

        
        public async Task<IActionResult> Logout()
        {
			HttpContext.Session.Remove("OrganizationLogo");
            HttpContext.Session.Remove("menus");
            HttpContext.Session.Remove("menuString");
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("RoleId");
            HttpContext.Session.Remove("name");
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
			Response.Headers["Pragma"] = "no-cache";
			Response.Headers["Expires"] = "0";

			//await HttpContext.SignOutAsync();

			return RedirectToAction("Login", "User");
        }


        public ActionResult ResetPassword(string token)
        {
            return View();
        }


      

        private string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb)
        {
            if (menu.Length > 0)
            {
                foreach (DataRow dr in menu)
                {
                    string url = dr["Url"].ToString();
                    string menuText = dr["MenuName"].ToString();
                    string icon = dr["Icon"].ToString();

                    if (url != "#")
                    {
                        string line = String.Format(@"<li><a href=""{0}""><i class=""{2}""></i> <span>{1}</span></a></li>", url, menuText, icon);
                        sb.Append(line);
                    }

                    string pid = dr["Id"].ToString();
                    string parentId = dr["ParentId"].ToString();

                    DataRow[] subMenu = table.Select(String.Format("ParentId = '{0}'", pid));
                    if (subMenu.Length > 0 && !pid.Equals(parentId))
                    {
                        string line = String.Format(@"<li class=""treeview""><a href=""#""><i class=""{0}""></i> <span>{1}</span><span class=""pull-right-container""><i class=""fa fa-angle-left pull-right""></i></span></a><ul class=""treeview-menu"">", icon, menuText);
                        var subMenuBuilder = new StringBuilder();
                        sb.AppendLine(line);
                        sb.Append(GenerateUL(subMenu, table, subMenuBuilder));
                        sb.Append("</ul></li>");
                    }
                }
            }
            return sb.ToString();
        }

        public DataSet ToDataSet<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dataTable);
            return ds;
        }

    }
}
 
