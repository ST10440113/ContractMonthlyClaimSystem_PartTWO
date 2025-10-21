using ContractMonthlyClaimSystem_PartTWO.Data;
using ContractMonthlyClaimSystem_PartTWO.Models;
using ContractMonthlyClaimSystem_PartTWO.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContractMonthlyClaimSystem_PartTWO.Controllers
{
    public class ClaimsController : Controller
    {
        public readonly IWebHostEnvironment _environment;
        public readonly FileEncryptionService _encryptionService;

        public ClaimsController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _encryptionService = new FileEncryptionService();
        }
        public IActionResult Index()
        {
            try
            {
                var claims = ClaimData.GetAllClaims();
                return View(claims);
            }
            catch (Exception ex)
            {
                
                ViewBag.Error = "Unable to load claims at this time. Please try again later.";
                return View(new List<Claim>());
            }

        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim claim, List<IFormFile> documents)
        {
            try
            {
                if (string.IsNullOrEmpty(claim.ClaimName))
                {
                    ViewBag.Error = "Claim Name is required";
                    return View(claim);
                }

                if (string.IsNullOrEmpty(claim.Faculty))
                {
                    ViewBag.Error = " Lecturer faculty is required";
                    return View(claim);
                }

                if (documents != null && documents.Count > 0)
                {
                    foreach (var file in documents)
                    {
                        if (file.Length > 0)
                        {
                            var allowedExtensions = new[] { ".pdf", ".docx", ".txt", ".xlsx" };
                            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                            if (!allowedExtensions.Contains(extension))
                            {
                                ViewBag.Error = $"File extension {extension} not allowed";
                                return View(claim);
                            }

                            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                            Directory.CreateDirectory(uploadsFolder);

                            var uniqueFileName = Guid.NewGuid().ToString() + ".encrypted";
                            var encryptedFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = file.OpenReadStream())
                            {
                                await _encryptionService.EncryptFileAsync(fileStream, encryptedFilePath);
                            }

                            claim.UploadedFiles.Add(new FileModel
                            {
                                FileName = file.FileName,
                                FilePath = "/uploads/" + uniqueFileName,
                                FileSize = file.Length,
                                IsEncrypted = true
                            });

                        }
                    }
                }

                ClaimData.AddClaim(claim);
                TempData["Success"] = "Claim added successfully!";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
               
                ViewBag["Error"] = "Error adding claim. " + ex.Message;
                return View(claim);
            }

        }

        public IActionResult Details(int id)
        {
            try
            {
                var claims = ClaimData.GetClaimById(id);
                if (claims == null)
                {
                    TempData["Error"] = "Claim not found.";
                    return View();
                }
                return View(claims);

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading claim";
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> DownloadDocument(int claimId, int docId)
        {
            try
            {
                var claim = ClaimData.GetClaimById(claimId);
                if (claim == null) { return NotFound("Claim not found."); }

                var document = claim.UploadedFiles.FirstOrDefault(doc => doc.Id == docId);
                if (document == null) { return NotFound("Document not found."); }

                var encryptedFilePath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
                if (!System.IO.File.Exists(encryptedFilePath)) return NotFound("File not found;");

                var decryptedStream = await _encryptionService.DecryptFileAsync(encryptedFilePath);

                var contentType = Path.GetExtension(document.FileName).ToLower()
                    switch
                {
                    ".pdf" => "application/pdf",
                    ".txt" => "application/txt",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    _ => "application/octet-stream"
                };

                return File(decryptedStream, contentType, document.FileName);

            }
            catch (Exception ex)
            {
                return BadRequest("Error downloading file: " + ex.Message);
            }
        }


    }
}

