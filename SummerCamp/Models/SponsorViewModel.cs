using SummerCamp.DataModels.Models;
using System.ComponentModel.DataAnnotations;

namespace SummerCamp.Models
{
    public class SponsorViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati un nume!")]
        public string Name { get; set; }
        public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

        public virtual ICollection<TeamSponsor> TeamSponsors { get; set; } = new List<TeamSponsor>();
    }
}
