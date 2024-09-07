using OwlReadingRoom.Models;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Database;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.ViewModels;

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

        [Transactional(readOnly: true)]
        public List<PackageListViewModel> GetPackageList()
        {
            return (from package in GetPackages()
            select new PackageListViewModel
            {
                Id = package.Id,
                Name = package.Name,
                Price = package.Price,
                RoomType = package.RoomType == RoomType.NON_AC ? "Non-AC Room" : "AC Room"
            }).ToList();
                
        }

        [Transactional(readOnly: true)]
        public List<PackageType> GetPackages()
        {
           return _packageRepository.GetItems();
        }

        [Transactional]
        public void SavePackage(PackageType packageType)
        {
            _packageRepository.SaveItem(packageType);
        }
    }
}
