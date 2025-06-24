using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizons.Web.ViewModels
{
    public class DestinationsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Terrain { get; set; } = null!; // Name only 

        public int FavoritesCount { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsPublisher { get; set; }
    }
}
