using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class CompetitionMatchController : Controller
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICompetitionTeamRepository _competitionTeamRepository;
        private readonly ICompetitionMatchRepository _competitionMatchRepository;
        private readonly IMapper _mapper;
        public CompetitionMatchController(ICompetitionMatchRepository competitionMatchRepository, ICompetitionTeamRepository competitionTeamRepository, ICompetitionRepository competitionRepository, ITeamRepository teamRepository, IMapper mapper)
        {
            _competitionTeamRepository = competitionTeamRepository;
            _competitionRepository = competitionRepository;
            _competitionMatchRepository = competitionMatchRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public IActionResult Index(CompetitionMatchViewModel competitionMatchViewModel)
        {
            var competitionMatch = _competitionMatchRepository.Get(c => c.CompetitionId == competitionMatchViewModel.CompetitionId);

            var competitionMatchModels = _mapper.Map<IList<CompetitionMatchViewModel>>(competitionMatch);
            return View(competitionMatchModels);
        }

        public IActionResult Add(int competitionId)
        {
            var teams = _competitionTeamRepository
                .Get(p => p.CompetitionId == competitionId)
                .Select(c => c.Team)
                .ToList();
            // var competitionMatchViewModel = new CompetitionMatchViewModel { AwayTeam = _ };

            var teamList = new SelectList(teams, "Id", "Name");
            ViewData["Teams"] = teamList;
            return View(new CompetitionMatchViewModel()
            {
                CompetitionId = competitionId,


            });
        }

        [HttpPost]
        public IActionResult Add(CompetitionMatchViewModel competitionMatchViewModel)
        {
            if (ModelState.IsValid)
            {
                _competitionMatchRepository.Add(_mapper.Map<CompetitionMatch>(competitionMatchViewModel));
                _competitionMatchRepository.Save();

                return RedirectToAction("Index", competitionMatchViewModel);
            }
            var teams = _competitionTeamRepository
               .Get(p => p.CompetitionId == competitionMatchViewModel.CompetitionId)
               .Select(c => c.Team)
               .ToList();

            var teamList = new SelectList(teams, "Id", "Name");
            ViewData["Teams"] = teamList;
            return View(competitionMatchViewModel);
        }

        public IActionResult Edit(int competitionMatchId)
        {
            var competitionMatch = _competitionMatchRepository.GetById(competitionMatchId);
            var competitionTeams = _competitionTeamRepository
                .Get(x => x.CompetitionId == competitionMatch.CompetitionId)
                .Select(x => x.Team);
            ViewData["Teams"] = new SelectList(competitionTeams, "Id", "Name");
            ViewData["Competition"] = new SelectList(_competitionRepository.GetAll(), "Id", "Name");
            var result = _mapper.Map<CompetitionMatchViewModel>(competitionMatch);
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(CompetitionMatchViewModel competitionMatchViewModel)
        {
            if (ModelState.IsValid)
            {
                _competitionMatchRepository.Update(_mapper.Map<CompetitionMatch>(competitionMatchViewModel));
                _competitionMatchRepository.Save();
                return RedirectToAction("Index", competitionMatchViewModel);
            }

            return View(competitionMatchViewModel);
        }

        public IActionResult Delete(int competitionMatchId)
        {
            var competitionMatch = _competitionMatchRepository.GetById(competitionMatchId);
            _competitionMatchRepository.Delete(competitionMatch);
            _competitionMatchRepository.Save();
            return RedirectToAction("Details", "Competition", new { competitionId = competitionMatch.CompetitionId });
        }
    }
}
