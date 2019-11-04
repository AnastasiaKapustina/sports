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
    public class TeamsController : Controller
    {
        private readonly Context _context;

        public TeamsController(Context context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index(Guid? sportId)
        {
            if (sportId == null)
            {
                return NotFound();
            }

            var sport = await _context.Sports.FindAsync(sportId);

            if (sport == null)
            {
                return NotFound();
            }

            ViewData["sportId"] = sportId;
            ViewData["sportName"] = sport.Name.ToLower();

            var teams = _context.Teams
                .Where(t => t.SportId == sportId)
                .Include(t => t.Sport);
            return View(await teams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Sport)
                .Include(t => t.Players)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            var model = new TeamViewModel()
            {
                Id = team.Id,
                Name = team.Name,
                Players = team.Players.ToDictionary(t => t.Id, t => t.Name),
                SportId = team.SportId,
                SportName = team.Sport.Name
            };

            return View(model);
        }

        // GET: Teams/Create
        [Authorize]
        public IActionResult Create(Guid? sportId)
        {
            if (sportId == null)
            {
                return NotFound();
            }

            ViewData["sportId"] = sportId;
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(TeamEditModel model)
        {
            if (ModelState.IsValid)
            {
                var team = new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    SportId = model.SportId
                };
                _context.Teams.Add(team);

                if (!String.IsNullOrEmpty(model.Players))
                {
                    foreach (var playerName in model.Players.Split(",")
                                                .Select(p => p.Trim())
                                                .Where(t => !String.IsNullOrEmpty(t)))
                    {
                        var player = new Player()
                        {
                            Id = Guid.NewGuid(),
                            Name = playerName,
                            TeamId = team.Id
                        };
                        _context.Players.Add(player);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { sportId = model.SportId });
            }
            ViewData["sportId"] = model.SportId;
            return View(model);
        }

        // GET: Teams/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            var model = new TeamEditModel()
            {
                Id = team.Id,
                Name = team.Name,
                SportId = team.SportId
            };

            return View(model);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id, TeamEditModel model)
        { 
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var team = await _context.Teams.FindAsync(id);

                if (team == null)
                {
                    return NotFound();
                }

                team.Name = model.Name;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { sportId = model.SportId });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var team = _context.Teams
                .Include(t => t.Players)
                .SingleOrDefault(t => t.Id == id);

            if (team == null)
                return NotFound();

            var games = _context.Games
                .Where(g => g.Team1Id == id || g.Team2Id == id)
                .ToList();
            foreach (var game in games)
            {
                _context.RemoveRange(_context.Events.Where(x => x.GameId == game.Id));
                _context.Remove(game);
            }
            _context.RemoveRange(_context.Players.Where(x => x.TeamId == id));
            _context.Remove(team);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Teams", new { sportId = team.SportId });
        }
    }
}
