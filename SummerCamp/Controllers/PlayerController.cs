using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;
        public PlayerController(IPlayerRepository playerRepository, ITeamRepository teamRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var players = _playerRepository.GetAll();

            var playerModels = _mapper.Map<IList<PlayerViewModel>>(players);

            return View(playerModels);
        }

        public IActionResult Add()
        {
            ViewData["Teams"] = new SelectList(_teamRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(PlayerViewModel playerViewModel)
        {
            if (ModelState.IsValid)
            {
                _playerRepository.Add(_mapper.Map<Player>(playerViewModel));
                _playerRepository.Save();
                return RedirectToAction("Index");
            }

            return View(playerViewModel);
        }

        public IActionResult Edit(int playerId)
        {
            var player = _playerRepository.GetById(playerId);
            ViewData["Teams"] = new SelectList(_teamRepository.GetAll(), "Id", "Name");
            return View(_mapper.Map<PlayerViewModel>(player));
        }
        [HttpPost]
        public IActionResult Edit(PlayerViewModel playerViewModel)
        {
            if (ModelState.IsValid)
            {
                _playerRepository.Update(_mapper.Map<Player>(playerViewModel));
                _playerRepository.Save();
                return RedirectToAction("Index");
            }

            return View(playerViewModel);
        }

        public IActionResult Delete(int playerId)
        {
            var player = _playerRepository.GetById(playerId);
            _playerRepository.Delete(player);
            _playerRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
