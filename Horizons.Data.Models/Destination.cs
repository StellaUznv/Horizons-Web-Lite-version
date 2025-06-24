using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizons.Data.Models
{
    public class Destination
    {
        [Key]
        public int Id { get; set; }

        [StringLength(80, MinimumLength = 3)]
        [Required]
        public string Name { get; set; } = null!;

        [StringLength(250, MinimumLength = 10)]
        [Required]
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }

        [Required]
        public string PublisherId { get; set; } = null!;

        [ForeignKey(nameof(PublisherId))]
        public IdentityUser Publisher { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }

        [Required]
        public int TerrainId { get; set; }

        [ForeignKey(nameof(TerrainId))]
        public Terrain Terrain { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public ICollection<UserDestination> UsersDestinations { get; set; } = new List<UserDestination>();
    }
}
