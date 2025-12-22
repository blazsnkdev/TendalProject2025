using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using TendalProject.Business.Interfaces;
using TendalProject.Web.ViewModels;
using TendalProject.Web.ViewModels.Dashboard;

namespace TendalProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardService _dashboardService;

        public HomeController(
            ILogger<HomeController> logger,
            IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _dashboardService.GetDatosCabeceraDashboardAsync();
            var value = result.Value!;
            var viewModel = new DashboardViewModel()
            {
                VentasHoy = value.VentasHoy,
                VentasMes = value.VentasMes,
                PedidosHoy = value.PedidosHoy,
                PedidosPendientes =value.PedidosPendientes
            };
            return View(viewModel);
        }
        public IActionResult Politica() => View();
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
