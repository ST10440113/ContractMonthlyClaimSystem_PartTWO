
using ContractMonthlyClaimSystem_PartTWO.Models;

namespace ContractMonthlyClaimSystem_PartTWO.Data
{
    public class ClaimData
    {
        public static List<Claim> _claims = new List<Claim>
        {
            new Claim
            {
                ClaimId = 1,
                LecturerRefID = "LID03",
                FirstName = "Alex",
                LastName = "Davis",
                Faculty = "Engineering",
                ClaimName = "Research Grant",
                Amount = 1500.00m,
                StartDate = new DateTime(2024, 1, 15),
                EndDate = new DateTime(2024, 3, 15),
                HoursWorked = 120,
                HourlyRate = 12.50m,
                Description = "Research on renewable energy sources.",
                Email = "alexdavis@gmail.com",
                SubmittedDate = DateTime.Now.AddDays(-10),
                Status = ClaimStatus.Pending, 
                ContactNum = "123-456-7890",
                UploadedFiles = new List<FileModel>(),
               
            },
            new Claim
            {
               ClaimId = 2,
                LecturerRefID = "LID07",
                FirstName = "Maria",
                LastName = "Garcia",
                Faculty = "Arts",
                ClaimName = "Conference Attendance",
                Amount = 800.00m,
                StartDate = new DateTime(2024, 2, 5),
                EndDate = new DateTime(2024, 2, 10),
                HoursWorked = 40,
                HourlyRate = 20.00m,
                Description = "Attending international art conference.",
                Email = "mariagarcia@gmail.com",
                SubmittedDate = DateTime.Now.AddDays(-7),
                Status = ClaimStatus.Approved,
                ContactNum = "987-654-3210",
                UploadedFiles = new List<FileModel>(),


            },
            new Claim
            {
               ClaimId = 3,
                LecturerRefID = "LID12",
                FirstName = "John",
                LastName = "Smith",
                Faculty = "Science",
                ClaimName = "Lab Equipment Purchase",
                Amount = 2000.00m,
                StartDate = new DateTime(2024, 3, 1),
                EndDate = new DateTime(2024, 3, 31),
                HoursWorked = 160,
                HourlyRate = 15.00m,
                Description = "Purchasing new lab equipment for experiments.",
                Email = "johnsmith@gmail.com",
                SubmittedDate = DateTime.Now.AddDays(-5),
                Status = ClaimStatus.Declined,
                ContactNum = "555-123-4567",
                UploadedFiles = new List<FileModel>(),


            }
        };
        private static int _nextId = 4;
        private static int _nextReviewId = 1;
        public static List<Claim> GetAllClaims() => _claims.ToList();

        public static Claim? GetClaimById(int id) => _claims.FirstOrDefault(b => b.ClaimId == id);

        public static List<Claim> GetClaimsByStatus(ClaimStatus status) => _claims.Where(b => b.Status == status).ToList();

        public static void AddClaim(Claim claim)
        {
            claim.ClaimId = _nextId;  
            _nextId++;
            claim.SubmittedDate = DateTime.Now;
            claim.Status = ClaimStatus.Pending;
            _claims.Add(claim);
        }

        public static bool UpdateClaimStatusFromManager(int id, ClaimStatus newStatus, string reviewedBy, string comments)
        {
            var claim = GetClaimById(id);
            if (claim == null) return false;

            
            var review = new ClaimReview
            {
                Id = _nextReviewId,
                ClaimId = id,
                ReviewerName = reviewedBy,
                ReviewerRole = "Academic Manager",
                ReviewDate = DateTime.Now,
                Decision = newStatus,
                Comments = comments
            };
            _nextReviewId++;

            claim.Reviews.Add(review);

           
            claim.Status = newStatus;
            claim.ReviewedBy = reviewedBy;
            claim.ReviewedDate = DateTime.Now;

            return true;
        }

        public static bool UpdateClaimStatusFromCoordinator(int id, ClaimStatus newStatus, string reviewedBy, string comments)
        {
            var claim = GetClaimById(id);
            if (claim == null) return false;


            var review = new ClaimReview
            {
                Id = _nextReviewId,
                ClaimId = id,
                ReviewerName = reviewedBy,
                ReviewerRole = "Programme Coordinator",
                ReviewDate = DateTime.Now,
                Decision = newStatus,
                Comments = comments
            };
            _nextReviewId++;

            claim.Reviews.Add(review);


            claim.Status = newStatus;
            claim.ReviewedBy = reviewedBy;
            claim.ReviewedDate = DateTime.Now;

            return true;
        }

        public static int GetPendingCount() => _claims.Count(b => b.Status == ClaimStatus.Pending);

        public static int GetApprovedCount() => _claims.Count(b => b.Status == ClaimStatus.Approved);

        public static int GetDeclinedCount() => _claims.Count(b => b.Status == ClaimStatus.Declined);

        public static int GetVerifedCount() => _claims.Count(b => b.Status == ClaimStatus.Verified);
    }
}
