using BCMT_Associates.Helpers;

namespace BCMT_Associates.Interfaces
{
    public interface IMvcControllerDiscovery
    {
        IEnumerable<MvcControllerInfo> GetControllers();
    }
}
