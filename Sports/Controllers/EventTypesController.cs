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
    public class EventTypesController : Controller
    {
        private readonly Context _context;

        public EventTypesController(Context context)
        {
            _context = context;
        }

        // GET: EventTypes
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
            ViewData["sportName"] = (await _context.Sports.FindAsync(sportId)).Name;

            var eventTypes = _context.EventTypes
                .Where(t => t.SportId == sportId)
                .Include(t => t.Sport);
            return View(await eventTypes.ToListAsync());
        }

        // GET: EventTypes/Create
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

        // POST: EventTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(EventTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventType = new EventType()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    IsDualPlayer = model.IsDualPlayer,
                    SportId = model.SportId,
                };
                _context.EventTypes.Add(eventType);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { sportId = model.SportId });
            }
            ViewData["sportId"] = model.SportId;
            return View(model);
        }

        // GET: EventTypes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventType = await _context.EventTypes
                .Include(e => e.Sport)
                .SingleOrDefaultAsync(e => e.Id == id);
            if (eventType == null)
            {
                return NotFound();
            }

            var model = new EventTypeViewModel()
            {
                Id = eventType.Id,
                Name = eventType.Name,
                IsDualPlayer = eventType.IsDualPlayer,
                SportId = eventType.SportId,
                SportName = eventType.Sport.Name
            };

            return View(model);
        }

        // POST: EventTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id, EventTypeViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var eventType = await _context.EventTypes.FindAsync(id);

                if (eventType == null)
                {
                    return NotFound();
                }

                eventType.Name = model.Name;

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

            var eventType = _context.EventTypes.Find(id);

            if (eventType == null)
                return NotFound();

            _context.RemoveRange(_context.Events.Where(x => x.EventTypeId == id));
            _context.Remove(eventType);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "EventTypes", new { sportId = eventType.SportId });
        }
    }
}
