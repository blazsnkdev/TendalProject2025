using TendalProject.Business.DTOs.Responses.Dashboard;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IDashboardService
    {
        Task<Result<DatosDashboardResponse>> GetDatosCabeceraDashboardAsync();
    }
}
