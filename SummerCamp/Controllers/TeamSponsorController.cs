using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class TeamSponsorController : Controller
    {
        private readonly ITeamSponsorRepository _teamSponsorRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;
        public TeamSponsorController(ITeamSponsorRepository teamSponsorRepository, ISponsorRepository sponsorRepository, ITeamRepository teamRepository, IMapper mapper)
        {
            _teamSponsorRepository = teamSponsorRepository;
            _sponsorRepository = sponsorRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public IActionResult Index(TeamSponsorViewModel teamSponsorViewModel)
        {
            var teamSponsor = _teamSponsorRepository.GetAll();
            var teamSponsorModels = _mapper.Map<IList<TeamSponsorViewModel>>(teamSponsor);
            return View(teamSponsorModels);
        }

        public IActionResult Add()
        {
            ViewData["Teams"] = new SelectList(_teamRepository.GetAll(), "Id", "Name");
            ViewData["Sponsors"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Add(TeamSponsorViewModel teamSponsorViewModel)
        {
            if (ModelState.IsValid)
            {
                _teamSponsorRepository.Add(_mapper.Map<TeamSponsor>(teamSponsorViewModel));
                _teamSponsorRepository.Save();
                return RedirectToAction("Index");
            }

            return View(teamSponsorViewModel);
        }

        public IActionResult Edit(int coachId)
        {
            var teamSponsor = _teamSponsorRepository.GetById(coachId);
            ViewData["Teams"] = new SelectList(_teamRepository.GetAll(), "Id", "Name");
            ViewData["Sponsors"] = new SelectList(_sponsorRepository.GetAll(), "Id", "Name");

            var result = _mapper.Map<TeamSponsorViewModel>(teamSponsor);
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(TeamSponsorViewModel teamSponsorViewModel)
        {
            if (ModelState.IsValid)
            {
                _teamSponsorRepository.Update(_mapper.Map<TeamSponsor>(teamSponsorViewModel));
                _teamSponsorRepository.Save();
                return RedirectToAction("Index");
            }

            return View(teamSponsorViewModel);
        }

        public IActionResult Delete(int teamSponsorId)
        {
            var teamSponsor = _teamSponsorRepository.GetById(teamSponsorId);
            _teamSponsorRepository.Delete(teamSponsor);
            _teamSponsorRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
