using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Repository;

namespace OwlReadingRoom.Services
{
    public class PackageService : IPackageService
    {
        private readonly IRepository<PackageType> _packageRepository;
        public PackageService(IRepository<PackageType> packageRepository)
        {
            this._packageRepository = packageRepository;
        }

        public List<PackageType> GetPackages()
        {
            return _packageRepository.GetItems();
        }
    }
}
