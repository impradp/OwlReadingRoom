using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;
using SQLite;

namespace OwlReadingRoom.Services;

public interface IPackageService
{
    List<PackageType> GetPackages();

    List<PackageListViewModel> GetPackageList();

    /// <summary>
    /// Retrieves the package list view model by the package ID.
    /// </summary>
    /// <param name="packageId">The ID of the package to retrieve.</param>
    /// <returns>The package list view model with the specified ID.</returns>
    PackageListViewModel GetPackageListViewModelById(int? packageId);

    void SavePackage(PackageType packageType);

    TableQuery<PackageType> TableQuery { get; }

    PackageType GetPackageById(int? Id);
    void UpdatePackage(PackageListViewModel package);
    void DeletePackage(int id);
}
