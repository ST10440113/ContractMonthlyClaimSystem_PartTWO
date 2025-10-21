using Microsoft.AspNetCore.Mvc;

namespace ContractMonthlyClaimSystem_PartTWO.Controllers
{
    public class ClaimsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
