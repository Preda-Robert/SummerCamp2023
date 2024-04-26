using AutoMapper;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Coach, CoachViewModel>().ReverseMap();
            CreateMap<Sponsor, SponsorViewModel>().ReverseMap();
            CreateMap<Player, PlayerViewModel>().ReverseMap();
            CreateMap<Team, TeamViewModel>().ReverseMap();
            CreateMap<CompetitionTeam, CompetitionTeamViewModel>().ReverseMap();
            CreateMap<CompetitionMatch, CompetitionMatchViewModel>().ReverseMap();
            CreateMap<TeamSponsor, TeamSponsorViewModel>().ReverseMap();
            CreateMap<Competition, CompetitionViewModel>().ReverseMap();
        }
    }
}
