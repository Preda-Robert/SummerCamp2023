using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Interfaces
{
    public interface ICompetitionTeamRepository : IGenericRepository<CompetitionTeam>
    {

        void RemoveRange(List<CompetitionTeam> competitionTeams);

    }
}
