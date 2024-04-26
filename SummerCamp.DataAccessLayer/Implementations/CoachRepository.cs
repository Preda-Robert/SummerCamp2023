using Microsoft.EntityFrameworkCore;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataAccessLayer.Repositories;
using SummerCamp.DataModels.Models;
using System.Linq.Expressions;

namespace SummerCamp.DataAccessLayer.Implementations
{
    public class CoachRepository : GenericRepository<Coach>, ICoachRepository
    {
        public CoachRepository(SummerCampDbContext dbContext) : base(dbContext)
        {
        }
        public override IList<Coach> Get(Expression<Func<Coach, bool>> expression)
        {
            return dbContext.Set<Coach>().Include(x => x.Teams).Where(expression).ToList();
        }
    }
}
