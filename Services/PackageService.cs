using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;

namespace OwlReadingRoom.Services
{
    public class PackageService : IPackageService
    {
        private readonly IRepository<PackageType> _packageRepository;

        private readonly IDatabaseConnectionService _databaseConnectionService;
        public PackageService(IRepository<PackageType> packageRepository, IDatabaseConnectionService databaseConnectionService)
        {
            this._packageRepository = packageRepository;
            _databaseConnectionService = databaseConnectionService;
        }

        public List<PackageType> GetPackages()
        {
            using (_databaseConnectionService)
            {
                return _packageRepository.GetItems();
            }
        }
    }
}
