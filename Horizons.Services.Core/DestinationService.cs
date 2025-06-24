using Horizons.Data;
using Horizons.Data.Models;
using Horizons.Services.Core.Contracts;
using Horizons.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizons.Services.Core
{
    public class DestinationService : IDestinationService
    {
        private readonly ApplicationDbContext _context;

        public DestinationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DestinationsViewModel>> GetAllDestinationsAsync(string? userId)
        {
            var destinations = await _context.Destinations
                .Where(d => !d.IsDeleted)
                .Include(d => d.Terrain)
                .Include(d => d.UsersDestinations)
                .ToListAsync();

            var result = destinations.Select(d => new DestinationsViewModel
            {
                Id = d.Id,
                Name = d.Name,
                ImageUrl = d.ImageUrl,
                Terrain = d.Terrain.Name,
                FavoritesCount = d.UsersDestinations.Count,
                IsFavorite = userId != null && d.UsersDestinations.Any(ud => ud.UserId == userId),
                IsPublisher = userId != null && d.PublisherId == userId
            });

            return result;
        }

        public async Task AddDestinationAsync(DestinationAddViewModel model, string userId)
        {
            var destination = new Destination
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                TerrainId = model.TerrainId,
                PublisherId = userId,
                PublishedOn = model.PublishedOn
            };

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();
        }

        public async Task AddToFavoritesAsync(int destinationId, string userId)
        {
            if (!_context.UserDestinations.Any(ud => ud.DestinationId == destinationId && ud.UserId == userId))
            {
                _context.UserDestinations.Add(new UserDestination
                {
                    DestinationId = destinationId,
                    UserId = userId
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromFavoritesAsync(int destinationId, string userId)
        {
            var favorite = await _context.UserDestinations
                .FirstOrDefaultAsync(ud => ud.DestinationId == destinationId && ud.UserId == userId);

            if (favorite != null)
            {
                _context.UserDestinations.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DestinationsViewModel>> GetUserFavoritesAsync(string userId)
        {
            return await _context.UserDestinations
                .Where(ud => ud.UserId == userId)
                .Select(ud => new DestinationsViewModel
                {
                    Id = ud.Destination.Id,
                    Name = ud.Destination.Name,
                    ImageUrl = ud.Destination.ImageUrl,
                    Terrain = ud.Destination.Terrain.Name,
                    FavoritesCount = ud.Destination.UsersDestinations.Count()
                }).ToListAsync();
        }
        public async Task<DestinationDetailsViewModel> GetDestinationDetailsAsync(int id, string userId)
        {
            var destination = await _context.Destinations
                .Include(d => d.Terrain)
                .Include(d => d.Publisher)
                .Include(d => d.UsersDestinations)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (destination == null)
            {
                throw new ArgumentException("Destination not found.");
            }

            return new DestinationDetailsViewModel
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                ImageUrl = destination.ImageUrl,
                Terrain = destination.Terrain.Name,
                PublisherName = destination.Publisher.UserName,
                PublishedOn = destination.PublishedOn.ToString("dd-MM-yyyy"),
                IsFavorite = destination.UsersDestinations.Any(ud => ud.UserId == userId),
                IsPublisher = destination.PublisherId == userId,
                Publisher = destination.Publisher.Email // or Email

            };
        }
        public async Task EditAsync(DestinationEditViewModel model)
        {
            var destination = await _context.Destinations.FindAsync(model.Id);
            if (destination == null) return;

            destination.Name = model.Name;
            destination.Description = model.Description;
            destination.ImageUrl = model.ImageUrl;
            destination.TerrainId = model.TerrainId;
            destination.PublishedOn = model.PublishedOn;

            await _context.SaveChangesAsync();
        }
        public async Task<DestinationEditViewModel?> GetByIdAsync(int id)
        {
            var destination = await _context.Destinations
                .Where(d => !d.IsDeleted)
                .Include(d => d.Terrain)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null)
            {
                return null;
            }

            var terrains = await _context.Terrains.ToListAsync();
            return new DestinationEditViewModel
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                ImageUrl = destination.ImageUrl,
                TerrainId = destination.TerrainId,
                PublishedOn = destination.PublishedOn,
                Terrains = terrains, // Optional, for dropdown
                PublisherId = destination.PublisherId
            };
        }
        public async Task DeleteAsync(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                throw new InvalidOperationException("Destination not found.");
            }

            destination.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

    }
}
