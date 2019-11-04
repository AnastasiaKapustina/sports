using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sports.Models;
using Sports.ViewModels;

namespace Sports.Controllers
{
    public class GamesController : Controller
    {
        private readonly Context _context;

        public GamesController(Context context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index(Guid? sportId)
        {
            if (sportId == null)
                return NotFound();

            var sport = _context.Sports.Find(sportId);

            if (sport == null)
                return NotFound();

            ViewData["sport"] = sport;
            var context = _context.Games
                .Where(g => g.SportId == sportId)
                .Include(g => g.Sport)
                .Include(g => g.Team1)
                .Include(g => g.Team2)
                .OrderByDescending(g => g.Date);
            return View(await context.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Sport)
                .Include(g => g.Team1)
                .Include(g => g.Team2)
                .Include(g => g.Events)
                .ThenInclude(e => e.EventType)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (game == null)
            {
                return NotFound();
            }

            var model = new GameViewModel()
            {
                Id = game.Id,
                SportId = game.SportId,
                SportName = game.Sport.Name,
                Team1Id = game.Team1Id,
                Team1Name = game.Team1.Name,
                Team2Id = game.Team2Id,
                Team2Name = game.Team2.Name,
                Date = game.Date
            };

            model.Events = new Dictionary<Guid, string>();
            foreach (var gameEvent in game.Events.OrderBy(x => x.Time))
            {
                model.Events.Add(gameEvent.Id, gameEvent.EventType.Name);
            }
            
            return View(model);
        }

        // GET: Games/Create
        [Authorize]
        public IActionResult Create(Guid? sportId)
        {
            if (sportId == null)
                return NotFound();

            var sport = _context.Sports.Find(sportId);

            if (sport == null)
                return NotFound();

            ViewData["sport"] = sport;

            var teams = _context.Teams.Where(x => x.SportId == sportId);
            ViewData["Team1Id"] = new SelectList(teams, "Id", "Name");
            ViewData["Team2Id"] = new SelectList(teams, "Id", "Name");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Guid? sportId, GameViewModel model)
        {
            var teams = _context.Teams.Where(x => x.SportId == sportId);

            if (sportId == null)
                return NotFound();

            var sport = _context.Sports.Find(sportId);

            if (sport == null)
                return NotFound();

            if (model.Team1Id == model.Team2Id)
            {
                ModelState.AddModelError("", "Команды должны быть разными");
            }

            if (ModelState.IsValid)
            {
                var game = new Game()
                {
                    Id = Guid.NewGuid(),
                    SportId = model.SportId,
                    Team1Id = model.Team1Id,
                    Team2Id = model.Team2Id,
                    Date = model.Date
                };

                _context.Games.Add(game);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { sportId = model.SportId });
            }

            ViewData["sport"] = sport;
            ViewData["Team1Id"] = new SelectList(teams, "Id", "Name", model.Team1Id);
            ViewData["Team2Id"] = new SelectList(teams, "Id", "Name", model.Team2Id);
            return View(model);
        }

        // GET: Games/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(x => x.Sport)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            var sport = _context.Sports.Find(game.SportId);

            if (sport == null)
                return NotFound();

            var model = new GameViewModel()
            {
                Id = game.Id,
                SportId = game.SportId,
                SportName = game.Sport.Name,
                Team1Id = game.Team1Id,
                Team1Name = game.Team1.Name,
                Team2Id = game.Team2Id,
                Team2Name = game.Team2.Name,
                Date = game.Date
            };

            ViewData["sport"] = sport;
            ViewData["Team1Id"] = new SelectList(_context.Teams, "Id", "Name", game.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Teams, "Id", "Name", game.Team2Id);
            return View(model);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id, GameViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sport = _context.Sports.Find(model != null && model.SportId != null ? model.SportId : Guid.Empty);

            if (sport == null)
                return NotFound();

            if (model.Team1Id == model.Team2Id)
            {
                ModelState.AddModelError("", "Команды должны быть разными");
            }

            if (ModelState.IsValid)
            {
                var game = await _context.Games.FindAsync(id);

                if (game == null)
                {
                    return NotFound();
                }

                game.Team1Id = model.Team1Id;
                game.Team2Id = model.Team2Id;
                game.Date = model.Date;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { sportId = model.SportId });
            }
            ViewData["sport"] = sport;
            ViewData["Team1Id"] = new SelectList(_context.Teams, "Id", "Name", model.Team1Id);
            ViewData["Team2Id"] = new SelectList(_context.Teams, "Id", "Name", model.Team2Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var game = _context.Games.Find(id);

            if (game == null)
                return NotFound();

            _context.RemoveRange(_context.Events.Where(x => x.GameId == id));
            _context.Remove(game);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Games", new { sportId = game.SportId });
        }
    }
}
