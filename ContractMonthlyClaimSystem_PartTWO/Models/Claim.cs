using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonthlyClaimSystem_PartTWO.Models
{
    public class Claim
    {
    public int ClaimId { get; set; }

    public string LecturerRefID { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Faculty { get; set; }

    public string ClaimName { get; set; }

    public decimal Amount { get; set; }

    
    public DateTime StartDate { get; set; }

   
    public DateTime EndDate { get; set; }

    public int HoursWorked { get; set; }

    public decimal HourlyRate { get; set; }

    public string Description { get; set; }
    public string Email { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public int? SubmittedBy { get; set; }

    public ClaimStatus Status { get; set; }

    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedDate { get; set; }

    public string ContactNum { get; set; }
    public List<FileModel> UploadedFiles { get; set; } = new List<FileModel>();

    public List<ClaimReview> Reviews { get; set; } = new List<ClaimReview>();

   
}
}

