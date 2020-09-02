namespace Hajrat2020.Models
{
    public class FamilyUser
    {
        public FamilyInNeed FamilyInNeed { get; set; }
        public int FamilyInNeedId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}