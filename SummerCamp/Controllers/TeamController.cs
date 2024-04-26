using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ICompetitionTeamRepository _competitionTeamRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ICoachRepository _coachRepository;
        private readonly IMapper _mapper;
        public TeamController(ITeamRepository teamRepository, IPlayerRepository playerRepository, ICoachRepository coachRepository, ISponsorRepository sponsorRepository, ICompetitionTeamRepository competitionTeamRepository, IMapper mapper)
        {
            _coachRepository = coachRepository;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _sponsorRepository = sponsorRepository;
            _competitionTeamRepository = competitionTeamRepository;
            _mapper = mapper;
        }

        public IActionResult Index(TeamViewModel teamViewModel)
        {
            var teams = _teamRepository.GetAll();
            var teamModels = _mapper.Map<IList<TeamViewModel>>(teams);
            return View(teamModels);
        }

        public IActionResult Add()
        {
            var players = _playerRepository.Get(p => p.TeamId == null);

            ViewData["TeamCoach"] = new SelectList(_coachRepository.GetAll(), "Id", "FullName");
            ViewData["TeamSponsors"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");

            var teamViewModel = new TeamViewModel { Players = _mapper.Map<IList<PlayerViewModel>>(players) };

            return View(teamViewModel);
        }

        [HttpPost]
        public IActionResult Add(TeamViewModel teamViewModel)
        {
            var players = _playerRepository.GetAll();

            if (ModelState.IsValid)
            {
                var playersList = new List<Player>();

                foreach (var p in players)
                {
                    if (teamViewModel.SelectedPlayerIds.Contains(p.Id))
                    {
                        playersList.Add(p);
                    }
                }
                var result = _mapper.Map<Team>(teamViewModel);
                result.Players = playersList;
                _teamRepository.Add(result);
                _teamRepository.Save();
                return RedirectToAction("Index");
            }
            teamViewModel.Players = _mapper.Map<List<PlayerViewModel>>(players);
            ViewData["TeamCoach"] = new SelectList(_coachRepository.GetAll(), "Id", "FullName");
            ViewData["TeamSponsors"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");

            return View(teamViewModel);
        }

        public IActionResult Edit(int teamId)
        {
            var team = _teamRepository.GetById(teamId);

            var players = _playerRepository.Get(p => p.TeamId == null || p.TeamId == teamId);
            ViewData["TeamCoach"] = new SelectList(_coachRepository.GetAll(), "Id", "FullName");
            ViewData["TeamSponsors"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");

            var data = _mapper.Map<TeamViewModel>(team);
            data.Players = _mapper.Map<List<PlayerViewModel>>(players);
            data.SelectedPlayerIds = players.Where(p => p.TeamId == teamId).Select(p => p.Id).ToList();


            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(TeamViewModel teamViewModel)
        {
            var players = _playerRepository.Get(p => p.TeamId == null || p.TeamId == teamViewModel.Id);


            if (ModelState.IsValid)
            {
                foreach (var player in players)
                {
                    if (teamViewModel.SelectedPlayerIds == null || !teamViewModel.SelectedPlayerIds.Contains(player.Id))
                    {
                        player.TeamId = null;
                    }
                    else
                    {
                        player.TeamId = teamViewModel.Id;
                    }


                    _playerRepository.Update(player);
                    _playerRepository.Save();
                }

                _teamRepository.Update(_mapper.Map<Team>(teamViewModel));
                _teamRepository.Save();
                return RedirectToAction("Index");
            }

            ViewData["Players"] = new SelectList(players, "Id", "Name");
            ViewData["TeamCoach"] = new SelectList(_coachRepository.GetAll(), "Id", "FullName");
            ViewData["TeamSponsors"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");


            teamViewModel.SelectedPlayerIds = players.Where(p => p.TeamId == teamViewModel.Id).Select(p => p.Id).ToList();

            return View(teamViewModel);
        }

        public IActionResult Delete(int teamId)
        {
            var team = _teamRepository.Get(x => x.Id == teamId).FirstOrDefault();
            if (team != null)
            {
                foreach (var competitionTeam in team.CompetitionTeams)
                {
                    competitionTeam.TeamId = null;
                }
                _teamRepository.Delete(team);
                _teamRepository.Save();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Show(int teamId)
        {
            var players = _playerRepository.Get(p => p.TeamId == teamId);
            var teamViewModel = new TeamViewModel { Players = _mapper.Map<IList<PlayerViewModel>>(players) };

            return View(teamViewModel);
        }

    }
}
