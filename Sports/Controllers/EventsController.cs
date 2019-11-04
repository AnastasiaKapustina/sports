using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sports.Models;
using Sports.ViewModels;

namespace Sports.Controllers
{
    public class EventsController : Controller
    {
        private readonly Context _context;

        public EventsController(Context context)
        {
            _context = context;
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameEventSinglePlayer = await _context.EventsSinglePlayer
                .Include(g => g.Game)
                .Include(g => g.EventType)
                .Include(g => g.Player)
                .ThenInclude(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gameEventSinglePlayer != null)
            {
                var model = new EventViewModel()
                {
                    Id = gameEventSinglePlayer.Id,
                    EventTypeId = gameEventSinglePlayer.EventTypeId,
                    EventTypeName = gameEventSinglePlayer.EventType.Name,
                    Player1Id = gameEventSinglePlayer.PlayerId,
                    Player1Name = gameEventSinglePlayer.Player.NameAndTeam,
                    GameId = gameEventSinglePlayer.GameId,
                    Time = gameEventSinglePlayer.Time,
                    IsTwoPlayer = false
                };

                return View(model);
            }

            var gameEventDualPlayer = await _context.EventsDualPlayer
                .Include(g => g.Game)
                .Include(g => g.EventType)
                .Include(g => g.Player1)
                .ThenInclude(p => p.Team)
                .Include(g => g.Player2)
                .ThenInclude(p => p.Team)
                .FirstOrDefaultAsync(m => m.Id == id);

            var test = _context.EventsDualPlayer.ToList();

            if (gameEventDualPlayer != null)
            {
                var model = new EventViewModel()
                {
                    Id = gameEventDualPlayer.Id,
                    EventTypeId = gameEventDualPlayer.EventTypeId,
                    EventTypeName = gameEventDualPlayer.EventType.Name,
                    Player1Id = gameEventDualPlayer.Player1Id,
                    Player1Name = gameEventDualPlayer.Player1.NameAndTeam,
                    Player2Id = gameEventDualPlayer.Player2Id,
                    Player2Name = gameEventDualPlayer.Player2.NameAndTeam,
                    GameId = gameEventDualPlayer.GameId,
                    Time = gameEventDualPlayer.Time,
                    IsTwoPlayer = true
                };

                return View(model);
            }

            return NotFound();
        }

        // GET: Games/Create
        [Authorize]
        public IActionResult Create(Guid? gameId)
        {
            if (gameId == null)
                return NotFound();

            var game = _context.Games.Find(gameId);

            if (game == null)
                return NotFound();

            ViewData["game"] = game;

            var types = _context.EventTypes
                .Where(x => x.SportId == game.SportId);
            var players = _context.Players
                .Where(x => x.TeamId == game.Team1Id || x.TeamId == game.Team2Id)
                .Include(x => x.Team)
                .OrderBy(x => x.Name);
            ViewData["EventType"] = new SelectList(types, "Id", "Name");
            ViewData["Players"] = new SelectList(players, "Id", "NameAndTeam");
            ViewData["DualPlayer"] = "[\""+String.Join("\",\"", types.Where(x => x.IsDualPlayer).Select(x => x.Name).ToList())+"\"]";
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Guid? gameId, EventViewModel model)
        {
            if (gameId == null)
                return NotFound();

            var game = _context.Games.Find(gameId);

            if (game == null)
                return NotFound();

            var types = _context.EventTypes
                .Where(x => x.SportId == game.SportId);
            var players = _context.Players
                .Where(x => x.TeamId == game.Team1Id || x.TeamId == game.Team2Id)
                .Include(x => x.Team)
                .OrderBy(x => x.Name);

            if (ModelState.IsValid)
            {
                if (model.IsTwoPlayer)
                {
                    var gameEvent = new EventDualPlayer()
                    {
                        Id = Guid.NewGuid(),
                        GameId = model.GameId,
                        Player1Id = model.Player1Id,
                        Player2Id = model.Player2Id,
                        EventTypeId = model.EventTypeId,
                        Time = model.Time
                    };

                    _context.EventsDualPlayer.Add(gameEvent);
                }
                else
                {
                    var gameEvent = new EventSinglePlayer()
                    {
                        Id = Guid.NewGuid(),
                        GameId = model.GameId,
                        PlayerId = model.Player1Id,
                        EventTypeId = model.EventTypeId,
                        Time = model.Time
                    };

                    _context.EventsSinglePlayer.Add(gameEvent);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Games", new { id = model.GameId });
            }

            ViewData["game"] = game;
            ViewData["EventType"] = new SelectList(types, "Id", "Name");
            ViewData["Players"] = new SelectList(players, "Id", "NameAndTeam");
            ViewData["DualPlayer"] = "[\"" + String.Join("\",\"", types.Where(x => x.IsDualPlayer).Select(x => x.Name).ToList()) + "\"]";
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

            var gameEventSinglePlayer = await _context.EventsSinglePlayer
                .Include(g => g.Game)
                .Include(g => g.EventType)
                .Include(g => g.Player)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gameEventSinglePlayer != null)
            {
                var model = new EventViewModel()
                {
                    Id = gameEventSinglePlayer.Id,
                    EventTypeId = gameEventSinglePlayer.EventTypeId,
                    EventTypeName = gameEventSinglePlayer.EventType.Name,
                    Player1Id = gameEventSinglePlayer.PlayerId,
                    Player1Name = gameEventSinglePlayer.Player.Name,
                    GameId = gameEventSinglePlayer.GameId,
                    Time = gameEventSinglePlayer.Time,
                    IsTwoPlayer = false
                };

                var game = gameEventSinglePlayer.Game;
                ViewData["game"] = game;

                var types = _context.EventTypes
                .Where(x => x.SportId == game.SportId);
                var players = _context.Players
                    .Where(x => x.TeamId == game.Team1Id || x.TeamId == game.Team2Id)
                    .Include(x => x.Team)
                    .OrderBy(x => x.Name);
                ViewData["EventType"] = new SelectList(types, "Id", "Name");
                ViewData["Players"] = new SelectList(players, "Id", "NameAndTeam");
                ViewData["DualPlayer"] = "[\"" + String.Join("\",\"", types.Where(x => x.IsDualPlayer).Select(x => x.Name).ToList()) + "\"]";

                return View(model);
            }

            var gameEventDualPlayer = await _context.EventsDualPlayer
                .Include(g => g.Game)
                .Include(g => g.EventType)
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gameEventDualPlayer != null)
            {
                var model = new EventViewModel()
                {
                    Id = gameEventDualPlayer.Id,
                    EventTypeId = gameEventDualPlayer.EventTypeId,
                    EventTypeName = gameEventDualPlayer.EventType.Name,
                    Player1Id = gameEventDualPlayer.Player1Id,
                    Player1Name = gameEventDualPlayer.Player1.Name,
                    Player2Id = gameEventDualPlayer.Player2Id,
                    Player2Name = gameEventDualPlayer.Player2.Name,
                    GameId = gameEventDualPlayer.GameId,
                    Time = gameEventDualPlayer.Time,
                    IsTwoPlayer = true
                };

                var game = gameEventDualPlayer.Game;
                ViewData["game"] = game;

                var types = _context.EventTypes
                .Where(x => x.SportId == game.SportId);
                var players = _context.Players
                    .Where(x => x.TeamId == game.Team1Id || x.TeamId == game.Team2Id)
                    .Include(x => x.Team)
                    .OrderBy(x => x.Name);
                ViewData["EventType"] = new SelectList(types, "Id", "Name");
                ViewData["Players"] = new SelectList(players, "Id", "NameAndTeam");
                ViewData["DualPlayer"] = "[\"" + String.Join("\",\"", types.Where(x => x.IsDualPlayer).Select(x => x.Name).ToList()) + "\"]";

                return View(model);
            }

            return NotFound();
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id, EventViewModel model)
        {
            if (id == null)
                return NotFound();

            var gameEvent = _context.Events.Find(id);

            if (gameEvent == null)
                return NotFound();

            _context.Remove(gameEvent);

            if (model.GameId == null)
                return NotFound();

            var game = _context.Games.Find(model.GameId);

            if (game == null)
                return NotFound();

            var types = _context.EventTypes
                .Where(x => x.SportId == game.SportId);
            var players = _context.Players
                .Where(x => x.TeamId == game.Team1Id || x.TeamId == game.Team2Id)
                .Include(x => x.Team)
                .OrderBy(x => x.Name);

            if (ModelState.IsValid)
            {
                if (model.IsTwoPlayer)
                {
                    var newGameEvent = new EventDualPlayer()
                    {
                        Id = model.Id,
                        GameId = model.GameId,
                        Player1Id = model.Player1Id,
                        Player2Id = model.Player2Id,
                        EventTypeId = model.EventTypeId,
                        Time = model.Time
                    };

                    _context.EventsDualPlayer.Add(newGameEvent);
                }
                else
                {
                    var newGameEvent = new EventSinglePlayer()
                    {
                        Id = model.Id,
                        GameId = model.GameId,
                        PlayerId = model.Player1Id,
                        EventTypeId = model.EventTypeId,
                        Time = model.Time
                    };

                    _context.EventsSinglePlayer.Add(newGameEvent);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Games", new { id = model.GameId });
            }

            ViewData["game"] = game;
            ViewData["EventType"] = new SelectList(types, "Id", "Name");
            ViewData["Players"] = new SelectList(players, "Id", "NameAndTeam");
            ViewData["DualPlayer"] = "[\"" + String.Join("\",\"", types.Where(x => x.IsDualPlayer).Select(x => x.Name).ToList()) + "\"]";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var gameEvent = _context.Events.Find(id);

            if (gameEvent == null)
                return NotFound();

            _context.Remove(gameEvent);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Games", new { id = gameEvent.GameId });
        }
    }
}
