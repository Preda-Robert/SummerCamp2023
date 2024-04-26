using Microsoft.EntityFrameworkCore;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Models;
using System.Linq.Expressions;

namespace SummerCamp.DataAccessLayer.Implementations
{
    public class CompetitionRepository : GenericRepository<Competition>, ICompetitionRepository
    {
        public CompetitionRepository(SummerCampDbContext dbContext) : base(dbContext)
        {
        }

        public override IList<Competition> Get(Expression<Func<Competition, bool>> expression)
        {
            return dbContext.Competitions
                .Include(c => c.Sponsor)
                .Include(c => c.CompetitionTeams)
                    .ThenInclude(ct => ct.Team)
                    .Where(expression)
                .ToList();
        }

    }
}
