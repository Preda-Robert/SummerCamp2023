using SummerCamp.DataModels.Models;
using System.ComponentModel.DataAnnotations;

namespace SummerCamp.Models
{
    public class TeamViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati un nume!")]
        public string Name { get; set; }
        public string NickName { get; set; }
        public int? CoachId { get; set; }
        public virtual Coach? Coach { get; set; }

        public List<int>? SelectedPlayerIds { get; set; }

        public virtual ICollection<CompetitionMatch>? CompetitionMatchAwayTeams { get; set; } = new List<CompetitionMatch>();

        public virtual ICollection<CompetitionMatch>? CompetitionMatchHomeTeams { get; set; } = new List<CompetitionMatch>();

        public virtual ICollection<CompetitionTeam>? CompetitionTeams { get; set; } = new List<CompetitionTeam>();

        public virtual IList<PlayerViewModel>? Players { get; set; } = new List<PlayerViewModel>();

        public virtual ICollection<TeamSponsor>? TeamSponsors { get; set; } = new List<TeamSponsor>();
    }
}
