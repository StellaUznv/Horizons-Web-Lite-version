using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizons.Web.ViewModels
{
    public class DestinationDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string Terrain { get; set; } = null!;

        public string PublisherName { get; set; } = null!;

        public string PublishedOn { get; set; } = null!; // formatted as "dd-MM-yyyy"

        public bool IsFavorite { get; set; }

        public bool IsPublisher { get; set; }
        public string Publisher { get; set; } = null!;

    }

}
