using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ICompetitionMatchRepository _competitionMatchRepository;
        private readonly ICompetitionTeamRepository _competitionTeamRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;
        public CompetitionController(ICompetitionRepository competitionRepository,
            ICompetitionTeamRepository competitionTeamRepository,
            ICompetitionMatchRepository competitionMatchRepository,
            ISponsorRepository sponsorRepository,
            IMapper mapper,
            ITeamRepository teamRepository)
        {
            _sponsorRepository = sponsorRepository;
            _competitionRepository = competitionRepository;
            _competitionMatchRepository = competitionMatchRepository;
            _competitionTeamRepository = competitionTeamRepository;
            _mapper = mapper;
            _teamRepository = teamRepository;
        }

        public IActionResult Index(CompetitionViewModel competitionViewModel)
        {
            var competition = _competitionRepository.GetAll();
            var competitionModels = _mapper.Map<IList<CompetitionViewModel>>(competition);
            return View(competitionModels);
        }

        public IActionResult Add()
        {
            var teams = _teamRepository.GetAll();
            ViewData["Sponsor"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");

            var competitionViewModel = new CompetitionViewModel { Teams = teams };

            return View(competitionViewModel);
        }

        [HttpPost]
        public IActionResult Add(CompetitionViewModel competitionViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Sponsor"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");

                competitionViewModel.Teams = _teamRepository.GetAll();
                return View(competitionViewModel);
            }

            var teams = _teamRepository.GetAll();
            ViewData["Sponsor"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");

            var competition = _mapper.Map<Competition>(competitionViewModel);

            competition.CompetitionTeams.Clear();

            foreach (var t in teams)
            {
                if (competitionViewModel.SelectedTeamIds.Contains(t.Id))
                {
                    var competionTeamToAdd = new CompetitionTeam
                    {
                        TeamId = t.Id,
                    };
                    competition.CompetitionTeams.Add(competionTeamToAdd);
                }
            }

            _competitionRepository.Update(competition);
            _competitionRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int competitionId)
        {
            var teams = _teamRepository.GetAll();
            var compTeams = _competitionTeamRepository.Get(c => c.CompetitionId == competitionId);
            var competition = _competitionRepository.Get(c => c.Id == competitionId).FirstOrDefault();
            ViewData["TeamSponsors"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");
            var competitionViewModel = _mapper.Map<CompetitionViewModel>(competition);

            var selectedCompetitionTeams = competition.CompetitionTeams.Select(c => c.Team).ToList();

            competitionViewModel.SelectedTeamIds = selectedCompetitionTeams.Select(c => c.Id).ToList();
            competitionViewModel.Teams = teams;



            //_competitionRepository.Update(_mapper.Map<Competition>(competitionViewModel));
            //_competitionRepository.Save();
            return View(competitionViewModel);
        }
        [HttpPost]
        public IActionResult Edit(CompetitionViewModel competitionViewModel)
        {
            ViewData["Sponsor"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");
            var teams = _teamRepository.GetAll();

            var notSelectedTeams = _competitionTeamRepository
                .Get(ct => !competitionViewModel.SelectedTeamIds.Contains(ct.TeamId ?? 0)).ToList();

            _competitionTeamRepository.RemoveRange(notSelectedTeams);
            _competitionRepository.Save();
            var alreadySelectedTeams = _competitionTeamRepository
                .Get(ct => ct.CompetitionId == competitionViewModel.Id)
                .Select(ct => ct.Team)
                .ToList();

            if (ModelState.IsValid)
            {
                foreach (var t in competitionViewModel.SelectedTeamIds)
                {
                    if (!alreadySelectedTeams.Select(ct => ct.Id).Contains(t))
                    {
                        var competionTeamToAdd = new CompetitionTeam
                        {
                            CompetitionId = competitionViewModel.Id,
                            TeamId = t,
                        };
                        _competitionTeamRepository.Add(competionTeamToAdd);
                    }
                }

                //_competitionRepository.Update(competition);
                _competitionTeamRepository.Save();
                return RedirectToAction("Index");
            }
            return View(competitionViewModel);
        }

        public IActionResult Delete(int competitionId)
        {
            var competition = _competitionRepository.GetById(competitionId);
            _competitionTeamRepository
                .RemoveRange(_competitionTeamRepository.Get(x => x.CompetitionId == competitionId).ToList());
            _competitionTeamRepository.Save();
            _competitionRepository.Delete(competition);
            _competitionRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int competitionId)
        {
            var matches = _competitionMatchRepository.Get(p => p.CompetitionId == competitionId);
            var competitionViewModel = new CompetitionViewModel { Id = competitionId, CompetitionMatches = matches };

            return View(competitionViewModel);
        }
    }
}
