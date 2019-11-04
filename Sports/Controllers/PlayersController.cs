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
    public class PlayersController : Controller
    {
        private readonly Context _context;

        public PlayersController(Context context)
        {
            _context = context;
        }

        // GET: Players/Create
        public IActionResult Create(Guid? teamId)
        {
            if (teamId == null)
            {
                return NotFound();
            }

            ViewData["teamId"] = teamId;

            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(PlayerEditModel model)
        {
            if (ModelState.IsValid)
            {
                var player = new Player()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    TeamId = model.TeamId
                };

                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Teams", new { id = model.TeamId });
            }
            ViewData["teamId"] = model.TeamId;
            return View(model);
        }

        // GET: Players/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            var model = new PlayerEditModel()
            {
                Id = player.Id,
                Name = player.Name,
                TeamId = player.TeamId
            };

            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id, PlayerEditModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var player = await _context.Players.FindAsync(id);
                player.Name = model.Name;
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Teams", new { id = model.TeamId });
            }
            ViewData["teamId"] = model.TeamId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var player = _context.Players.Find(id);

            if (player == null)
                return NotFound();

            _context.RemoveRange(_context.EventsDualPlayer.Where(x => x.Player1Id == id || x.Player2Id == id));
            _context.RemoveRange(_context.EventsSinglePlayer.Where(x => x.PlayerId == id));
            _context.Remove(player);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Teams", new { id = player.TeamId });
        }
    }
}
