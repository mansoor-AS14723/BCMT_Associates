using BCMT_Associates.Context;
using BCMT_Associates.Helpers;
using BCMT_Associates.Interfaces;
using BCMT_Associates.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;

namespace BCMT_Associates.Repository
{
    public class UserRepository : IUserInterface
    {
        private readonly AppDBContext _dBContext;
        private readonly IEmailService _emailService;

        public UserRepository(AppDBContext _dBContext, IEmailService emailService)
        {
            this._dBContext = _dBContext;
            _emailService = emailService;
        }

        public async Task<List<User>> Get(int? userId = null, string RoleName = null)
        {
            return await _dBContext.Users.FromSqlRaw("UserGet {0},{1}", userId == 0 ? null : userId, RoleName).ToListAsync();
           
        }

        public async Task<User> LoginUser(User user)
        {
            var res = await _dBContext.Users.FromSqlInterpolated($"UserLogin {user.Email}, {user.Password}").ToListAsync();
            return res.FirstOrDefault();

        }

        public async Task<int> Add(User user)
        {

            return await _dBContext.Database.ExecuteSqlAsync($"userAdd {user.FirstName}, {user.MiddleName}, {user.LastName}, {user.Gender},  {user.DOB},{user.Email},{user.ContactNo},{user.Username}, {user.Password}, {user.RoleId},{user.ProfilePicture},{user.CreatedBy}");

        }

        public async Task<int> Update(User user)
        {
            return await _dBContext.Database.ExecuteSqlAsync($"userUpdate  {user.Id}, {user.FirstName}, {user.MiddleName}, {user.LastName}, {user.Gender},  {user.DOB},{user.Email},{user.ContactNo},{user.Username}, {user.Password}, {user.RoleId},{user.ProfilePicture},{user.UpdatedBy}, {user.IsActive}");
        }
        public string ForgotPassword(User user, string origin, IConfiguration configuration, string logopath)
        {
            AccountHelper accountServices = new AccountHelper(_emailService);
            var userAccount = (_dBContext.Users.FromSqlInterpolated($"FindEmail {user.Email}").ToList()).SingleOrDefault();

            // always return ok response to prevent email enumeration
            if (userAccount == null) return "Email Not Found";

            var Token = generateResetToken();
            ResetPassword resetPassword = new ResetPassword();
            resetPassword.UserId = userAccount.Id;
            resetPassword.Token = Token;
            resetPassword.CreatedOn = DateTime.Now;
            resetPassword.ExpiresOn = DateTime.Now.AddHours(1);
            var savetoken = SaveToken(resetPassword);

            // send email
            accountServices.sendPasswordResetEmail(userAccount, origin, Token, logopath);
            return "Please check your email for password reset instructions";

        }
        public async Task<string> ResetPassword(User user)
        {
            var verifyUser = await _dBContext.ResetPassword.FirstOrDefaultAsync(x => x.Token == user.Token && x.ExpiresOn >= DateTime.Now);
            if (verifyUser != null)
            {
                var result = await _dBContext.Users
               .FirstOrDefaultAsync(e => e.Id == verifyUser.UserId);

                result.Password = user.Password;
                result.UpdatedOn = DateTime.Now;
                await _dBContext.SaveChangesAsync();

                return "Password Reset Successfully";

            }

            else
            {
                return "You took more than 1 hour. Please reset password again";
            }
        }


        private string SaveToken(ResetPassword model)
        {
            if (model != null)
            {

                _dBContext.ResetPassword.Add(model);
                _dBContext.SaveChanges();
                return "Success";
            }
            return null;

        }


        private string generateResetToken()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            // ensure token is unique by checking against db
            var tokenIsUnique = !_dBContext.ResetPassword.Any(x => x.Token == token);
            if (!tokenIsUnique)
                return generateResetToken();

            return token;
        }

        public async Task<int> Delete(int Id, int LoginUser)
        {
            return await _dBContext.Database.ExecuteSqlAsync($"UserDelete {Id}, {LoginUser}");
        }

        



    }
}
