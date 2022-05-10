using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);

            List<CartItem> items = await context.CartItem
                .Include(c => c.Book)
                .ThenInclude(b => b.Store)
                .Where(c => c.UserId == userId)
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Add(string isbn)
        {
            var book = await context.Book.FindAsync(isbn);

            if (book == null)
                return BadRequest();

            var user = await userManager.GetUserAsync(User);

            var cartItem = await context.CartItem
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.BookIsBn == book.Isbn);
            if (cartItem == null)
            {
                cartItem = new CartItem()
                {
                    BookIsBn = book.Isbn,
                    Quantity = 1,
                };

                user.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            var row = await context.SaveChangesAsync();

            return RedirectToAcionIndex;
        }

        public async Task<IActionResult> Remove(string isbn)
        {
            var book = await context.Book.FindAsync(isbn);

            if (book == null)
                return RedirectToAcionIndex;

            var user = await userManager.GetUserAsync(User);

            var cartItem = await context.CartItem
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.BookIsBn == book.Isbn);

            if (cartItem != null)
            {
                context.CartItem.Remove(cartItem);
                var row = await context.SaveChangesAsync();
            }

            return RedirectToAcionIndex;
        }

        public async Task<IActionResult> IncreaseQuantity(string isbn)
        {
            var book = await context.Book.FindAsync(isbn);

            if (book == null)
                return BadRequest();

            var user = await userManager.GetUserAsync(User);

            var cartItem = await context.CartItem
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.BookIsBn == book.Isbn);
            if (cartItem == null)
                return BadRequest();
            else
            {
                cartItem.Quantity++;
                var row = await context.SaveChangesAsync();
            }

            return RedirectToAcionIndex;
        }

        public async Task<IActionResult> DecreaseQuantity(string isbn)
        {
            var book = await context.Book.FindAsync(isbn);

            if (book == null)
                return BadRequest();

            var user = await userManager.GetUserAsync(User);

            var cartItem = await context.CartItem
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.BookIsBn == book.Isbn);
            if (cartItem == null)
                return BadRequest();
            else if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                var row = await context.SaveChangesAsync();
            }

            return RedirectToAcionIndex;
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await userManager.GetUserAsync(User);

            var a = new Dictionary<int, List<CartItem>>();

            var items = await context.CartItem
                .Include(c => c.Book)
                .ThenInclude(c => c.Store)
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            var aa = items.AsQueryable()
                .GroupBy(c => c.Book.StoreId)
                .Select(x => new Order()
                {
                    UserId = user.Id,
                    StoreId = x.Key,
                    Items = x.Select(i => new OrderItem()
                    {
                        BookIsBn = i.BookIsBn
                    }).ToList()
                })
                .ToList();

            return RedirectToAcionIndex;
        }

        private IActionResult RedirectToAcionIndex => RedirectToAction(nameof(Index));
    }
}
