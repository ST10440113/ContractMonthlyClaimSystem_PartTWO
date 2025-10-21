using ContractMonthlyClaimSystem_PartTWO.Data;
using ContractMonthlyClaimSystem_PartTWO.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContractMonthlyClaimSystem_PartTWO.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index(string filter = "all")
        {
            try
            {
                var claims = ClaimData.GetAllClaims();
                ViewBag.Filter = filter;
                claims = filter.ToLower() switch
                {
                    "pending" => ClaimData.GetClaimsByStatus(ClaimStatus.Pending),
                    "approved" => ClaimData.GetClaimsByStatus(ClaimStatus.Approved),
                    "declined" => ClaimData.GetClaimsByStatus(ClaimStatus.Declined),
                    "verified" => ClaimData.GetClaimsByStatus(ClaimStatus.Verified),
                    _ => claims
                };
                ViewBag.PendingCount = ClaimData.GetClaimsByStatus(ClaimStatus.Pending).Count;
                ViewBag.ApprovedCount = ClaimData.GetClaimsByStatus(ClaimStatus.Approved).Count;
                ViewBag.DeclinedCount = ClaimData.GetClaimsByStatus(ClaimStatus.Declined).Count;
                ViewBag.VerifiedCount = ClaimData.GetClaimsByStatus(ClaimStatus.Verified).Count;

                return View(claims);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to load claims";
                return View(new List<Claim>());
            }


        }


        public IActionResult Review(int id)
        {
            try
            {
                var claim = ClaimData.GetClaimById(id);
                if (claim == null)
                {
                    TempData["Error"] = "Claim not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(claim);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading claim.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

