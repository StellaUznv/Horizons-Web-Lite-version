using Horizons.Data;
using Horizons.Data.Models;
using Horizons.Services.Core.Contracts;
using Horizons.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Horizons.Web.Controllers
{
    public class DestinationController : Controller
    {
        private readonly IDestinationService _destinationService;
        private readonly ApplicationDbContext _context;

        public DestinationController(IDestinationService destinationService, ApplicationDbContext dbContext)
        {
            _destinationService = destinationService;
            _context = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var destinations = await _destinationService.GetAllDestinationsAsync(userId);
            return View(destinations);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new DestinationAddViewModel
            {
                Terrains = _context.Terrains.ToList(),
                PublishedOn = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DestinationAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Terrains = _context.Terrains.ToList();

                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _destinationService.AddDestinationAsync(model, userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _destinationService.AddToFavoritesAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _destinationService.RemoveFromFavoritesAsync(id, userId);
            return RedirectToAction(nameof(Favorites));
        }

        public async Task<IActionResult> Favorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _destinationService.GetUserFavoritesAsync(userId);
            var viewModel = new DestinationFavouritesViewModel
            {
                Favourites = favorites
            };
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.Identity.IsAuthenticated
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)!
                : string.Empty;

            try
            {
                var model = await _destinationService.GetDestinationDetailsAsync(id, userId);
                return View(model);
            }
            catch
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var destination = await _destinationService.GetByIdAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            var model = new DestinationEditViewModel
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                ImageUrl = destination.ImageUrl,
                TerrainId = destination.TerrainId,
                PublishedOn = destination.PublishedOn,
                Terrains = await _context.Terrains.ToListAsync(),
                PublisherId = destination.PublisherId
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DestinationEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Terrains = await _context.Terrains.ToListAsync();
                return View(model);
            }

            await _destinationService.EditAsync(model);

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var destination = await _context.Destinations
                .Include(d => d.Publisher)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (destination == null)
            {
                return NotFound();
            }

            var viewModel = new DestinationDeleteViewModel
            {
                Id = destination.Id,
                Name = destination.Name,
                PublisherId = destination.PublisherId,
                Publisher = destination.Publisher.UserName // or another field
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(DestinationDeleteViewModel model)
        {
            await _destinationService.DeleteAsync(model.Id);
            return RedirectToAction(nameof(Index));
        }


    }
}
