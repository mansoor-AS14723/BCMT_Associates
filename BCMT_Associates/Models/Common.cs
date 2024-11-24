using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Data;
using System.Reflection;
//using Syncfusion.XlsIO;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
//using EFCore.BulkExtensions;
using System.IO;
//using Org.BouncyCastle.Utilities.Zlib;
//using Org.BouncyCastle.Utilities.IO;
using System.Net;
using System.Net.Http.Headers;
//using OfficeOpenXml.Style;
//using OfficeOpenXml;
//using LicenseContext = OfficeOpenXml.LicenseContext;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using System.Security.Policy;

namespace BCMT_Associates.Models
{
    public static class Common
    {

        public static void SetItemFromRow<T>(T item, DataRow row)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (row.Table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                {
                    var propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    var value = row[prop.Name];

                    if (propertyType == typeof(int) && value is decimal decimalValue)
                    {
                        value = Convert.ToInt32(decimalValue);
                    }

                    prop.SetValue(item, Convert.ChangeType(value, propertyType), null);
                }
            }
        }

        public static T CreateItemFromRow<T>(DataRow row)
        {
            T item = Activator.CreateInstance<T>();
            SetItemFromRow(item, row);
            return item;
        }

        public static List<T> CreateListFromTable<T>(DataTable tbl)
        {
            List<T> list = new List<T>();
            foreach (DataRow row in tbl.Rows)
            {
                list.Add(CreateItemFromRow<T>(row));
            }
            return list;
        }
        public static Claim[] GetClaims(User userInfo, IConfiguration _configuration)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, userInfo.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                          new Claim(ClaimTypes.NameIdentifier,  userInfo.Id.ToString()),
                         
                          //new Claim(ClaimTypes.Uri, string.IsNullOrEmpty(userInfo.ProfilePicture) ? "" : userInfo.ProfilePicture ),
                            new Claim(ClaimTypes.Role, string.IsNullOrEmpty(userInfo.RoleName) ? "": userInfo.RoleName),
                           new Claim(ClaimTypes.Name,string.IsNullOrEmpty(userInfo.Username) ? "" : userInfo.Username),
                        new Claim("FirstName", string.IsNullOrEmpty(userInfo.FirstName) ? "" : userInfo.FirstName),
                        new Claim("MiddleName",string.IsNullOrEmpty(userInfo.MiddleName) ? "" : userInfo.MiddleName),
                        new Claim("LastName", string.IsNullOrEmpty(userInfo.LastName) ? "" : userInfo.LastName),
                        new Claim("ContactNo", string.IsNullOrEmpty(userInfo.ContactNo) ? "" : userInfo.ContactNo)

                    };


            return claims;
        }
        public static string GenerateToken(IConfiguration _configuration, Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is AIRISP Secret key which is custom key"));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                "Jwt:Issuer",
                "Jwt:Audience",
                claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static bool FileExtention(string FileName)
        {
            var _extensions = ".jpg,.png,.jpeg,.pdf,.doc";
            var extension = Path.GetExtension(FileName);
            if (!_extensions.Contains(extension.ToLower()))
            {
                return false;
            }
            return true;

        }

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
        {
            //{".txt", "text/plain"},
            //{".pdf", "application/pdf"},
            //{".doc", "application/vnd.ms-word"},
            //{".docx", "application/vnd.ms-word"},
            //{".xls", "application/vnd.ms-excel"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            //{".gif", "image/gif"},
            //{".csv", "text/csv"}
        };
        }

        //public static DataTable ExcelToDataTable(string filePath, int createdBy)
        //{
        //    using (Stream inputStream = File.OpenRead(filePath))
        //    {
        //        using (ExcelEngine excelEngine = new ExcelEngine())
        //        {
        //            IApplication application = excelEngine.Excel;
        //            IWorkbook workbook = application.Workbooks.Open(inputStream);
        //            IWorksheet worksheet = workbook.Worksheets[0];

        //            DataTable dataTable = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
        //            dataTable = dataTable.Rows
        //          .Cast<DataRow>()
        //          .Where(row => !row.ItemArray.All(f => f is DBNull))
        //          .CopyToDataTable();
        //            return dataTable;
        //        }
        //    }

        //}

        public static void SaveActivityLog(string exc)
        {
            try
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Activity Log") == false)
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Activity Log");

                FileStream objFS = null;
                string strFilePath = String.Format(@"{0}\Activity Log\{1:yyyy-MM-dd }Activity.txt", AppDomain.CurrentDomain.BaseDirectory, System.DateTime.Now);
                if (!File.Exists(strFilePath))
                {
                    objFS = new FileStream(strFilePath, FileMode.Create);
                }
                else
                    objFS = new FileStream(strFilePath, FileMode.Append);

                using (StreamWriter Sr = new StreamWriter(objFS))
                {
                    Sr.WriteLine(String.Format("{0:HH:mm:ss}---{1}", System.DateTime.Now, exc));
                }
            }
            catch
            {
            }
        }


        public static void SaveExceptionLog(Exception exc)
        {
            try
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Exception Log") == false)
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Exception Log");

                FileStream objFS = null;
                string strFilePath = String.Format(@"{0}\Exception Log\{1:yyyy-MM-dd }Exception.txt", AppDomain.CurrentDomain.BaseDirectory, System.DateTime.Now);
                if (!File.Exists(strFilePath))
                {
                    objFS = new FileStream(strFilePath, FileMode.Create);
                }
                else
                    objFS = new FileStream(strFilePath, FileMode.Append);

                using (StreamWriter Sr = new StreamWriter(objFS))
                {
                    Sr.WriteLine(String.Format("Exception Time: {0}" + Environment.NewLine + "TargetSite: {1}" + Environment.NewLine + "Exception Message: {2}" + Environment.NewLine + "Inner Exception: {3} " + Environment.NewLine + "StackTrace: {4}", System.DateTime.Now.ToLongTimeString(), exc.TargetSite, exc.Message, exc.InnerException, exc.ToString()));
                    if (!String.IsNullOrEmpty(exc.StackTrace))
                    {

                    }
                    Sr.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
            catch (Exception exp)
            {
                // System.Diagnostics.EventLog.WriteEntry("PMS NZ", "Error saving logs : " + exp.Message);
            }
        }

        //public static MemoryStream ExportToExcel(DataTable dataTable, string FileName)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (var package = new ExcelPackage(new FileInfo(FileName)))
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        //        using (var headerCells = worksheet.Cells[1, 1, 1, dataTable.Columns.Count])
        //        {
        //            headerCells.Style.Font.Bold = true;
        //            headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        //        }
        //        // Add headers
        //        for (int i = 0; i < dataTable.Columns.Count; i++)
        //        {
        //            worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
        //        }

        //        // Populate data
        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < dataTable.Columns.Count; j++)
        //            {
        //                worksheet.Cells[i + 2, j + 1].Value = dataTable.Rows[i][j];
        //            }
        //        }

        //        // Save to a MemoryStream
        //        using (var stream = new MemoryStream())
        //        {
        //            package.SaveAs(stream);
        //            stream.Position = 0;

        //            // Return the Excel file as a response
        //            return stream;//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerData.xlsx");
        //        }
        //    }



        //}

        public static DataTable ConvertToDataTable<T>(List<T> dataList)
        {
            DataTable dataTable = new DataTable();

            // Get the properties of the type T
            var properties = typeof(T).GetProperties();

            // Create columns in DataTable based on the properties of T
            foreach (var property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            // Populate rows in DataTable with data from the list
            foreach (var item in dataList)
            {
                DataRow row = dataTable.NewRow();

                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        public static DataTable RemoveNullOrEmptyOrIntegerColumns(DataTable dataTable)
        {
            for (int columnIndex = dataTable.Columns.Count - 1; columnIndex >= 0; columnIndex--)
            {
                DataColumn column = dataTable.Columns[columnIndex];

                bool isColumnNullOrEmptyOrInteger = true;

                // Check if all values in the column are either null, empty, or integers
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row[column] != null)
                    {
                        if (column.ColumnName.ToLower().Contains("id") || column.ColumnName.ToLower().Contains("createdby") || column.ColumnName.ToLower().Contains("updatedby"))
                        {
                            isColumnNullOrEmptyOrInteger = true; break;
                        }
                        //if (row[column] is int)
                        //{
                        //    // Integer value
                        //    isColumnNullOrEmptyOrInteger = false;
                        //    break;
                        //}
                        if (row[column] is string && !string.IsNullOrEmpty((string)row[column]))
                        {
                            // Non-empty string
                            isColumnNullOrEmptyOrInteger = false;
                            break;
                        }
                        
                        // Add more type checks as needed
                    }
                }

                // If all values in the column are null, empty, or integers, remove the column
                if (isColumnNullOrEmptyOrInteger)
                {
                    dataTable.Columns.Remove(column);
                }
            }

            return dataTable;
        }

        //public static List<Customer> GetCustomersForDropDown(List<Customer> customers)
        //{
        //    return customers.Select(x => new Customer
        //    {
        //        Id = x.Id,
        //        Username = x.FullName + (string.IsNullOrEmpty(x.Username) ? "" : " (" + x.Username + ")") +
        //       (string.IsNullOrEmpty(x.CNIC) ? "" : " (" + x.CNIC + ")") +
        //       (string.IsNullOrEmpty(x.ContactNo) ? "" : " (" + x.ContactNo + ")")
        //    }).ToList();

        //}

        


	}



}
