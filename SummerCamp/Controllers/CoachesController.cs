using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class CoachesController : Controller
    {
        private readonly ICoachRepository _coachRepository;
        private readonly IMapper _mapper;
        public CoachesController(ICoachRepository coachRepository, IMapper mapper)
        {
            _coachRepository = coachRepository;
            _mapper = mapper;
        }

        public IActionResult Index(CoachViewModel coachViewModel)
        {
            var coaches = _coachRepository.GetAll();
            var coachModels = _mapper.Map<IList<CoachViewModel>>(coaches);
            return View(coachModels);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CoachViewModel coachViewModel)
        {
            if (ModelState.IsValid)
            {
                _coachRepository.Add(_mapper.Map<Coach>(coachViewModel));
                _coachRepository.Save();
                return RedirectToAction("Index");
            }

            return View(coachViewModel);
        }

        public IActionResult Edit(int coachId)
        {
            var coach = _coachRepository.GetById(coachId);
            var result = _mapper.Map<CoachViewModel>(coach);
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(CoachViewModel coachViewModel)
        {
            if (ModelState.IsValid)
            {
                _coachRepository.Update(_mapper.Map<Coach>(coachViewModel));
                _coachRepository.Save();
                return RedirectToAction("Index");
            }

            return View(coachViewModel);
        }

        public IActionResult Delete(int coachId)
        {
            var coach = _coachRepository.Get(x => x.Id == coachId).FirstOrDefault();
            if (coach != null)
            {
                foreach (var team in coach.Teams)
                {
                    team.CoachId = null;
                }
                _coachRepository.Delete(coach);
                _coachRepository.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
