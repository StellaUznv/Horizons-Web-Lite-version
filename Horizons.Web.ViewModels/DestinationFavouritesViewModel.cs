using Horizons.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horizons.Web.ViewModels
{
    public class DestinationFavouritesViewModel
    {
        public IEnumerable<DestinationsViewModel> Favourites { get; set; } = new List<DestinationsViewModel>();
    }
}
