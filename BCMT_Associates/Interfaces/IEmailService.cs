namespace BCMT_Associates.Interfaces
{
	public interface IEmailService
	{
		void Send(string to, string subject, string html, string? from = null, string? logoPath = null);
	}
}
