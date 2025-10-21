using Microsoft.AspNetCore.Mvc;

namespace ContractMonthlyClaimSystem_PartTWO.Controllers
{
    public class CoordinatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
