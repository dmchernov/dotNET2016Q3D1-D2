using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using MvcMusicStore.Models;
using MvcMusicStore.Performance;
using MvcMusicStore.ViewModels;
using NLog;

namespace MvcMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

	    private ILog _logger;

		private PerformanceCounterHelper.CounterHelper<Counters> counter = PerformanceCounterHelper.PerformanceHelper.CreateCounterHelper<Counters>("MusicStore");

		public ShoppingCartController(ILog logger)
	    {
		    _logger = logger;
	    }

        // GET: /ShoppingCart/
        public async Task<ActionResult> Index()
        {
            var cart = ShoppingCart.GetCart(_storeContext, this);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await cart.GetCartItems().ToListAsync(),
                CartTotal = await cart.GetTotal()
            };
			_logger.Debug("Go to shopping cart");

            return View(viewModel);
        }

        // GET: /ShoppingCart/AddToCart/5
        public async Task<ActionResult> AddToCart(int id)
        {
            var cart = ShoppingCart.GetCart(_storeContext, this);

            await cart.AddToCart(await _storeContext.Albums.SingleAsync(a => a.AlbumId == id));

            await _storeContext.SaveChangesAsync();

			_logger.Info($"Item \"{_storeContext.Albums.Single(a => a.AlbumId == id).Title}\" was added to cart");

	        counter.Increment(Counters.ItemsInCard);

            return RedirectToAction("Index");
        }

        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(_storeContext, this);

            var albumName = await _storeContext.Carts
                .Where(i => i.RecordId == id)
                .Select(i => i.Album.Title)
                .SingleOrDefaultAsync();

            var itemCount = await cart.RemoveFromCart(id);

            await _storeContext.SaveChangesAsync();

            var removed = (itemCount > 0) ? " 1 copy of " : string.Empty;

            var results = new ShoppingCartRemoveViewModel
            {
                Message = removed + albumName + " has been removed from your shopping cart.",
                CartTotal = await cart.GetTotal(),
                CartCount = await cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

	        if (counter.GetInstance(Counters.ItemsInCard).RawValue > 0)
		        counter.Decrement(Counters.ItemsInCard);

            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(_storeContext, this);

            var cartItems = cart.GetCartItems()
                .Select(a => a.Album.Title)
                .OrderBy(x => x)
                .ToList();

            ViewBag.CartCount = cartItems.Count();
            ViewBag.CartSummary = string.Join("\n", cartItems.Distinct());

            return PartialView("CartSummary");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
