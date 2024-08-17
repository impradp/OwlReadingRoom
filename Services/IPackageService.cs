using OwlReadingRoom.Models;

namespace OwlReadingRoom.Services;

public interface IPackageService
{
    List<PackageType> GetPackages();
}
