using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;
using SQLite;

namespace OwlReadingRoom.Services;

public interface IPackageService
{
    List<PackageType> GetPackages();

    List<PackageListViewModel> GetPackageList();

    void SavePackage(PackageType packageType);

    TableQuery<PackageType> TableQuery { get; }

    PackageType GetPackageById(int? Id);
}
