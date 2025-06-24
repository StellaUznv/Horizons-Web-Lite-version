using Horizons.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Horizons.Web.ViewModels
{
    public class DestinationAddToFavouritesViewModel
    {
        public Destination Destination { get; set; } = null!;
        public IdentityUser User { get; set; } = null!; 
    }
}
