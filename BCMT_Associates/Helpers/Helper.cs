using BCMT_Associates.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace BCMT_Associates.Helpers
{
    public static class Helper
    {
        public static string IsSelected(this IHtmlHelper htmlHelper, string controller, string action)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            return (controller == routeController && action == routeAction) ? "active" : "";
        }

        public static string getClaims(ClaimsIdentity claimsIdentity, string key)
        {
            var claims = claimsIdentity.Claims.ToList();
            var val = claims?.FirstOrDefault(x => x.Type.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value;
            if (val == "0") return null;

            else return claims?.FirstOrDefault(x => x.Type.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value;

        }

        public static DataSet ToDataSet<T>(List<T> items)
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

        public static string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb, List<string> menus_id)
        {
            if (menu.Length > 0)
            {
              
                foreach (DataRow dr in menu)
                {
                    string id = dr.Table.Columns.Contains("MenusId") ? dr["MenusId"].ToString() :dr["Id"].ToString();
                    string handler = dr["Url"].ToString();
                    string menuText =dr.Table.Columns.Contains("MenuName") ? dr["MenuName"].ToString(): dr["Name"].ToString();
                    string icon = dr["Icon"].ToString();

                    string pid = dr.Table.Columns.Contains("MenusId") ? dr["MenusId"].ToString() : dr["Id"].ToString();
                    string parentId = dr["ParentId"].ToString();
                    string status = "";
                    if (menus_id != null)
                    {
                        status = (menus_id.Contains(id)) ? "Checked" : "";
                    }
                    

                    DataRow[] subMenu = table.Select(String.Format("ParentId = '{0}'", pid));
                    if (subMenu.Length > 0 && !pid.Equals(parentId))
                    {
                        string line = String.Format(@"<li class=""has""><input type=""checkbox"" name=""subdomain[]"" id=""{5}"" value=""{1}"" {4}><label> {1}</label>", handler, menuText, icon, "target", status, id);
                        sb.Append(line);

                        var subMenuBuilder = new StringBuilder();
                        sb.AppendLine(String.Format(@"<ul>"));
                        sb.Append(GenerateUL(subMenu, table, subMenuBuilder, menus_id));
                        sb.Append("</ul>");
                    }
                    else
                    {
                        string line = String.Format(@"<li class=""""><input type=""checkbox"" name=""subdomain[]"" id=""{5}"" value=""{1}"" {4}><label>{1}</label>", handler, menuText, icon, "target", status, id);
                        sb.Append(line);
                    }
                    sb.Append("</li>");
                }
            }
            return sb.ToString();
        }


		public class AppSettings
		{
			public string? Secret { get; set; }

			// refresh token time to live (in days), inactive tokens are
			// automatically deleted from the database after this time
			public int RefreshTokenTTL { get; set; }

			public string? EmailFrom { get; set; } = "mtspk2022@gmail.com";
			public string? SmtpHost { get; set; } = "smtp.gmail.com";
			public int SmtpPort { get; set; } = 587;
			public string? SmtpUser { get; set; } = "mtspk2022@gmail.com";
			public string? SmtpPass { get; set; } = "ucywdwfmxgrlofuj"; //MTS.org //"baoarypkyceydrkw";
		}
	}


}
