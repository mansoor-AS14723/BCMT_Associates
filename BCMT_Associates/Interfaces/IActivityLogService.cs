using BCMT_Associates.Models;

namespace BCMT_Associates.Interfaces
{
    public interface IActivityLogService
    {
        Task LogAsync(ActivityLog logEntry);
    }
}
