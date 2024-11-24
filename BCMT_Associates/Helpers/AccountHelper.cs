using BCMT_Associates.Interfaces;
using BCMT_Associates.Models;

namespace BCMT_Associates.Helpers
{
	public class AccountHelper
	{
		private readonly IEmailService _emailService;
		private string _BaseUrl;
		public AccountHelper(IEmailService emailservice)
		{
			this._emailService = emailservice;

		}
		public string generateResetToken(User user, IConfiguration configuration)
		{

			var claims = Common.GetClaims(user, configuration);
			var token = Common.GenerateToken(configuration, claims);


			return token;
		}

		public void sendPasswordResetEmail(User account, string origin, string Token, string logopath)
		{
			string message;



			var resetUrl = $"{origin}Account/ResetPassword?token={Token}";

			message = $@"<div style='background: #ebeef1;padding: 20px; border-radius: 3px;border: 1px 
                            solid #184C4C;border-top:2px solid #184C4C;'> 
                           <a href='{_BaseUrl}' target='_blank'> <img width = '200' alt='{_BaseUrl}' src = 'cid:{logopath}' style='width:104; height:30; vertical-align:middle'></a>
                         <b style='color : #1470F9'>  </b><br /> 
                         <h2>We received a request to change your password</h2> 
     
                         <hr><br /> 
                          Hi {account.FirstName},<br/>
                   Use the link below to set up a new password for your account.
                   This link will expire in 1 hours.<br/><br/><br/>
                    <a type=""button""  class=""btn btn-primary""  href={resetUrl} style=""text-decoration: none !important;background-color: #184C4C;  color: white; padding: 14px 25px; text-align: center; display: inline-block;""> Change Password  </a> <br/><br/><br/>
                   If you did not make this request, you do not need to do anything. <br/>
                   Thanks for your time,<br/>
                   The MTS Team 
                                 System Generated Email  do not reply<hr> www.mtspk.org
                                 <br/> Meta Testing Service Pakistan <br/>
                                 </div> ";

			_emailService.Send(
				to: account.Email,
				subject: "Reset Password",
				html: $@"{message}",
				logoPath: logopath

			);
		}

	}
}
