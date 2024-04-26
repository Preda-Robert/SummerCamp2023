using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class SponsorController : Controller
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IMapper _mapper;
        public SponsorController(ISponsorRepository sponsorRepository, IMapper mapper)
        {
            _sponsorRepository = sponsorRepository;
            _mapper = mapper;
        }

        public IActionResult Index(SponsorViewModel sponsorViewModel)
        {
            var sponsors = _sponsorRepository.GetAll();
            var sponsorModels = _mapper.Map<IList<SponsorViewModel>>(sponsors);
            return View(sponsorModels);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(SponsorViewModel sponsorViewModel)
        {
            if (ModelState.IsValid)
            {
                _sponsorRepository.Add(_mapper.Map<Sponsor>(sponsorViewModel));
                _sponsorRepository.Save();
                return RedirectToAction("Index");
            }

            return View(sponsorViewModel);
        }

        public IActionResult Edit(int sponsorId)
        {
            var sponsor = _sponsorRepository.GetById(sponsorId);
            return View(_mapper.Map<SponsorViewModel>(sponsor));
        }
        [HttpPost]
        public IActionResult Edit(SponsorViewModel sponsorViewModel)
        {
            if (ModelState.IsValid)
            {
                _sponsorRepository.Update(_mapper.Map<Sponsor>(sponsorViewModel));
                _sponsorRepository.Save();
                return RedirectToAction("Index");
            }

            return View(sponsorViewModel);
        }

        public IActionResult Delete(int sponsorId)
        {
            var sponsor = _sponsorRepository.GetById(sponsorId);
            _sponsorRepository.Delete(sponsor);
            _sponsorRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
