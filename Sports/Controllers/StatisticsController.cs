using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sports.Models;
using Sports.ViewModels;

namespace Sports.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly Context _context;

        public StatisticsController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var players = _context.Players.Include(x => x.Team).ToList();
            ViewData["players"] = new SelectList(players, "Id", "NameAndTeam");

            var teams = _context.Teams.ToList();
            ViewData["teams"] = new SelectList(teams, "Id", "Name");

            var sports = _context.Sports.ToList();
            ViewData["sports"] = new SelectList(sports, "Id", "Name");

            var eventTypes = _context.EventTypes.Include(et => et.Sport).Where(x => !x.IsDualPlayer).ToList();
            ViewData["eventTypes"] = new SelectList(eventTypes, "Id", "NameAndSport");

            return View();
        }

        public IActionResult PlayerStatistics(Guid? playerId)
        {
            if (playerId == null)
                return NotFound();

            var player = _context.Players
                .Include(x => x.Team)
                .Single(x => x.Id == playerId);

            if (player == null)
                return NotFound();

            var playerEvents = _context.Events
                .OfType<EventSinglePlayer>()
                .Where(x => x.PlayerId == playerId)
                .Cast<Event>()
                .Union(
                    _context.Events
                    .OfType<EventDualPlayer>()
                    .Where(x => x.Player1Id == playerId || x.Player2Id == playerId)
                    .Cast<Event>()
                )
                .ToList();

            var playerEventStats = playerEvents
                .GroupBy(x => x.EventTypeId)
                .Select(x => new Statistics()
                {
                    Id = x.Key,
                    Count = x.Count()
                })
                .ToList();

            var eventTypes = _context.EventTypes;
            foreach (var playerStat in playerEventStats)
            {
                playerStat.Name = eventTypes.Single(x => x.Id == playerStat.Id).Name;
            }

            ViewData["Name"] = player.NameAndTeam;

            return View(playerEventStats);
        }

        public IActionResult TeamStatistics(Guid? teamId)
        {
            if (teamId == null)
                return NotFound();

            var team = _context.Teams.Find(teamId);

            if (team == null)
                return NotFound();

            var playerIds = _context.Players
                .Where(p => p.TeamId == teamId)
                .Select(p => p.Id)
                .ToList();

            var teamEvents = _context.Events
                .OfType<EventSinglePlayer>()
                .Where(x => playerIds.Contains(x.PlayerId))
                .Cast<Event>()
                .Union(
                    _context.Events
                    .OfType<EventDualPlayer>()
                    .Where(x => playerIds.Contains(x.Player1Id) || playerIds.Contains(x.Player2Id))
                    .Cast<Event>()
                )
                .ToList();

            var teamEventStats = teamEvents
                .GroupBy(x => x.EventTypeId)
                .Select(x => new Statistics()
                {
                    Id = x.Key,
                    Count = x.Count()
                })
                .ToList();

            var eventTypes = _context.EventTypes;
            foreach (var teamStat in teamEventStats)
            {
                teamStat.Name = eventTypes.Single(x => x.Id == teamStat.Id).Name;
            }

            ViewData["Name"] = team.Name;

            return View(teamEventStats);
        }

        public IActionResult SportStatistics(Guid? sportId)
        {
            if (sportId == null)
                return NotFound();

            var sport = _context.Sports.Find(sportId);

            if (sport == null)
                return NotFound();

            var sportEventStats = _context.Events
                .Include(e => e.Game)
                .Include(e => e.EventType)
                .Where(e => e.Game.SportId == sportId)
                .GroupBy(x => new { Id = x.EventTypeId, x.EventType.Name })
                .Select(x => new Statistics()
                {
                    Name = x.Key.Name,
                    Count = x.Count()
                })
                .ToList();

            ViewData["Name"] = sport.Name;

            return View(sportEventStats);
        }

        public IActionResult SingleEventTypeStatistics(Guid? eventTypeId)
        {
            if (eventTypeId == null)
                return NotFound();

            var eventType = _context.EventTypes.Find(eventTypeId);

            if (eventType == null)
                return NotFound();

            var eventStats = _context.EventsSinglePlayer
                .Where(e => e.EventTypeId == eventTypeId)
                .Include(e => e.Player)
                .ThenInclude(p => p.Team)
                .GroupBy(x => new { x.PlayerId, x.Player.Name, Team = x.Player.Team.Name })
                .Select(x => new Statistics()
                {
                    Id = x.Key.PlayerId,
                    Name = x.Key.Name + " (" + x.Key.Team + ")",
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            ViewData["Name"] = eventType.Name;

            return View(eventStats);
        }

        public IActionResult PopularityStatistics()
        {
            var popularityStats = _context.Games
                .Include(e => e.Sport)
                .GroupBy(x => new { x.SportId, x.Sport.Name})
                .Select(x => new Statistics()
                {
                    Id = x.Key.SportId,
                    Name = x.Key.Name,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return View(popularityStats);
        }

        public IActionResult ActivityStatistics()
        {
            var activitiyStats = _context.Events
                .Include(e => e.Game)
                .ThenInclude(g => g.Sport)
                .GroupBy(x => new { x.GameId, x.Game.SportId, x.Game.Sport.Name })
                .Select(x => new
                {
                    x.Key.GameId,
                    x.Key.SportId,
                    x.Key.Name,
                    Count = x.Count()
                })
                .GroupBy(x => new { x.SportId, x.Name})
                .Select(x => new FrequencyStatistics()
                {
                    Id = x.Key.SportId,
                    Name = x.Key.Name,
                    Frequency = x.Average(p => p.Count)
                })
                .OrderByDescending(x => x.Frequency)
                .ToList();

            return View(activitiyStats);
        }
    }
}