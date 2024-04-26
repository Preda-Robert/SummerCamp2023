using SummerCamp.DataModels.Models;
using System.ComponentModel.DataAnnotations;

namespace SummerCamp.Models
{
    public class CompetitionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati un nume!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati un numar de echipe!")]
        public int NumbersOfTeams { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati o adresa!")]
        public string Address { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? SponsorId { get; set; }

        public virtual ICollection<CompetitionMatch>? CompetitionMatches { get; set; } = new List<CompetitionMatch>();
        public virtual IList<CompetitionTeam>? CompetitionTeams { get; set; } = new List<CompetitionTeam>();
        public List<int>? SelectedTeamIds { get; set; }

        public virtual Sponsor? Sponsor { get; set; }
        public virtual IList<Team>? Teams { get; set; }
    }
}
