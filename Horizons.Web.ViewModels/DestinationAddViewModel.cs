using Horizons.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizons.Web.ViewModels
{
    public class DestinationAddViewModel
    {
        [Required]
        [StringLength(80, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Required]
        [Display(Name = "Terrain")]
        public int TerrainId { get; set; }

        public IEnumerable<Terrain>? Terrains { get; set; }
        public DateTime PublishedOn { get; set; }
    }
}
