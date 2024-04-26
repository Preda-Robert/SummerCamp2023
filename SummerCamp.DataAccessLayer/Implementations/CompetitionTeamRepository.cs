using Microsoft.EntityFrameworkCore;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Models;
using System.Linq.Expressions;

namespace SummerCamp.DataAccessLayer.Implementations
{
    public class CompetitionTeamRepository : GenericRepository<CompetitionTeam>, ICompetitionTeamRepository
    {
        public CompetitionTeamRepository(SummerCampDbContext dbContext) : base(dbContext)
        {
        }

        public override IList<CompetitionTeam> GetAll()
        {
            return dbContext.CompetitionTeams.Include(c => c.Team).Include(c => c.Competition).ToList();
        }

        public override IList<CompetitionTeam> Get(Expression<Func<CompetitionTeam, bool>> expression)
        {
            return dbContext.CompetitionTeams.Include(c => c.Team).Include(c => c.Competition)
                .Where(expression).ToList();
        }


        public void RemoveRange(List<CompetitionTeam> competitionTeams)
        {
            dbContext.CompetitionTeams.RemoveRange(competitionTeams);
            dbContext.SaveChanges();
        }
    }
}
