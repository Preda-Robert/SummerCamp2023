using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class CompetitionTeamController : Controller
    {
        private readonly ICompetitionTeamRepository _competitionTeamRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;
        public CompetitionTeamController(ICompetitionTeamRepository competitionTeamRepository, ITeamRepository teamRepository, IMapper mapper)
        {
            _competitionTeamRepository = competitionTeamRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public IActionResult Index(CompetitionTeamViewModel competitionTeamViewModel)
        {
            var compTeam = _competitionTeamRepository.GetAll();
            var compTeamModels = _mapper.Map<IList<CompetitionTeamViewModel>>(compTeam);
            return View(compTeamModels);
        }

        public IActionResult Add()
        {
            ViewData["Teams"] = new SelectList(_teamRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(CompetitionTeamViewModel competitionTeamViewModel)
        {
            if (ModelState.IsValid)
            {
                _competitionTeamRepository.Add(_mapper.Map<CompetitionTeam>(competitionTeamViewModel));
                _competitionTeamRepository.Save();
                return RedirectToAction("Index");
            }

            return View(competitionTeamViewModel);
        }

        public IActionResult Edit(int competitonTeamId)
        {
            var compTeam = _competitionTeamRepository.GetById(competitonTeamId);
            ViewData["Teams"] = new SelectList(_teamRepository.GetAll(), "Id", "Name");
            var result = _mapper.Map<CompetitionTeamViewModel>(compTeam);
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(CompetitionTeamViewModel competitionTeamViewModel)
        {
            if (ModelState.IsValid)
            {
                _competitionTeamRepository.Update(_mapper.Map<CompetitionTeam>(competitionTeamViewModel));
                _competitionTeamRepository.Save();
                return RedirectToAction("Index");
            }

            return View(competitionTeamViewModel);
        }

        public IActionResult Delete(int competitonTeamId)
        {
            var compTeam = _competitionTeamRepository.GetById(competitonTeamId);
            _competitionTeamRepository.Delete(compTeam);
            _competitionTeamRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
