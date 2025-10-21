using Microsoft.AspNetCore.Mvc;

namespace ContractMonthlyClaimSystem_PartTWO.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
