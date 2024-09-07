using OwlReadingRoom.Models;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services;

public interface IPackageService
{
    List<PackageType> GetPackages();

    List<PackageListViewModel> GetPackageList();

    void SavePackage(PackageType packageType);
}
