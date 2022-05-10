using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = Role.Seller)]
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public StoreController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        private async Task<IActionResult> Index()
        {
            var applicationDbContext = context.Store.Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        private async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await context.Store
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        [HttpGet]
        public async Task<IActionResult> Me()
        {
            var userId = userManager.GetUserId(User);

            var store = await context.Store
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (store == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(store);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Store/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name")] Store store)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);
                store.UserId = userId;

                context.Add(store);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Me));
            }

            return View(store);
        }

        // GET: Store/Edit/5
        public async Task<IActionResult> Edit()
        {
            var userId = userManager.GetUserId(User);

            var store = await context.Store
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,UserId,Name")] Store store)
        {
            var userId = userManager.GetUserId(User);
            var existed = await context.Store.AnyAsync(m => m.UserId == userId);

            if (!existed)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(store);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Me));
            }
            return View(store);
        }

        // GET: Store/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await context.Store
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await context.Store.FindAsync(id);
            context.Store.Remove(store);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return context.Store.Any(e => e.Id == id);
        }
    }
}
