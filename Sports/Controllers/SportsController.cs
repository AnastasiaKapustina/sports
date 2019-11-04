using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sports.Models;

namespace Sports.Controllers
{
    public class SportsController : Controller
    {
        private readonly Context _context;

        public SportsController(Context context)
        {
            _context = context;
        }

        // GET: Sports
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sports.ToListAsync());
        }

        // GET: Sports/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name")] Sport sport)
        {
            if (ModelState.IsValid)
            {
                sport.Id = Guid.NewGuid();
                _context.Add(sport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sport);
        }

        // GET: Sports/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sport = await _context.Sports.FindAsync(id);
            if (sport == null)
            {
                return NotFound();
            }
            return View(sport);
        }

        // POST: Sports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Sport sport)
        {
            if (id != sport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(sport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var sport = _context.Sports
                .Include(s => s.Games)
                .Include(s => s.Teams)
                .SingleOrDefault(t => t.Id == id);

            if (sport == null)
                return NotFound();

            var gameIds = sport.Games.Select(x => x.Id).ToList();
            var teamIds = sport.Teams.Select(x => x.Id).ToList();
            _context.RemoveRange(_context.Events.Where(x => gameIds.Contains(x.GameId)));
            _context.RemoveRange(_context.Players.Where(x => teamIds.Contains(x.TeamId)));

            _context.RemoveRange(_context.Teams.Where(x => x.SportId == id));
            _context.RemoveRange(_context.Games.Where(x => x.SportId == id));
            _context.RemoveRange(_context.EventTypes.Where(x => x.SportId == id));
            _context.Remove(sport);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Sports");
        }
    }
}
