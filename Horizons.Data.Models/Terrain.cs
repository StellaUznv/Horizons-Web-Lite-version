using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizons.Data.Models
{
    public class Terrain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }
}
