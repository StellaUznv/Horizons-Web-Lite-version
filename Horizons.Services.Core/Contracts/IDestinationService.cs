using Horizons.Web.ViewModels;

namespace Horizons.Services.Core.Contracts
{
    public interface IDestinationService
    {
        Task<IEnumerable<DestinationsViewModel>> GetAllDestinationsAsync(string? userId);

        Task AddDestinationAsync(DestinationAddViewModel model,string userId);

        Task AddToFavoritesAsync(int destinationId, string userId);
        Task RemoveFromFavoritesAsync(int destinationId, string userId);
        Task<IEnumerable<DestinationsViewModel>> GetUserFavoritesAsync(string userId);

        Task<DestinationDetailsViewModel> GetDestinationDetailsAsync(int id, string userId);
        Task EditAsync(DestinationEditViewModel model);
        Task<DestinationEditViewModel?> GetByIdAsync(int id);
        Task DeleteAsync(int id);

    }
}
