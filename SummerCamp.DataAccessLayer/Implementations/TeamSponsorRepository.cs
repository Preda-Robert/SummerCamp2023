using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Implementations
{
    public class TeamSponsorRepository : GenericRepository<TeamSponsor>, ITeamSponsorRepository
    {
        public TeamSponsorRepository(SummerCampDbContext dbContext) : base(dbContext)
        {
        }
    }
}
