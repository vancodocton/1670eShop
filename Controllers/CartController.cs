using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = Role.Customer)]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<CartController> logger;

        public CartController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<CartController> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);

            List<CartItem> items = await context.CartItem
                .Include(c => c.Book)
                .ThenInclude(b => b.Store)
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Book.StoreId)
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

            var items = await context.CartItem
                .Include(c => c.Book)
                .ThenInclude(c => c.Store)
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            var orders = items.AsQueryable()
                .GroupBy(c => c.Book.StoreId)
                .Select(x => new Order()
                {
                    UserId = user.Id,
                    StoreId = x.Key,
                    Items = x.Select(i => new OrderItem()
                    {
                        BookIsBn = i.BookIsBn,
                        Quantity = i.Quantity,
                        Price = i.Book.Price
                    }).ToList(),
                })
                .ToList();

            user.Orders.AddRange(orders);

            context.CartItem.RemoveRange(items);

            var row = await context.SaveChangesAsync();

            foreach (var order in orders)
                logger.LogInformation($"Checkouted order'Id={order.Id}' with price: {order.TotalPrice}.");

            return RedirectToAcionIndex;
        }

        private IActionResult RedirectToAcionIndex => RedirectToAction(nameof(Index));
    }
}
