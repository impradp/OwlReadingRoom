using OwlReadingRoom.Models;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.ViewModels;
using SQLite;

namespace OwlReadingRoom.Services
{
    public class PackageService : IPackageService
    {
        private readonly IRepository<PackageType> _packageRepository;
        public PackageService(IRepository<PackageType> packageRepository)
        {
            this._packageRepository = packageRepository;
        }

        public TableQuery<PackageType> TableQuery => _packageRepository.Table;

        [Transactional]
        public void DeletePackage(int id)
        {
            PackageType packageType = _packageRepository.GetItem(id);
            packageType.Enabled = false;
            _packageRepository.SaveItem(packageType);
        }

        [Transactional(readOnly: true)]
        public PackageType GetPackageById(int? Id)
        {
            return _packageRepository.GetItem(Id);
        }

        [Transactional(readOnly: true)]
        public List<PackageListViewModel> GetPackageList()
        {
            return (from package in GetPackages()
                    where package.Enabled is true
                    select new PackageListViewModel
            {
                Id = package.Id,
                Name = package.Name,
                Price = package.Price,
                RoomType = package.RoomType,
                Days = package.Days
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

        [Transactional]
        public void UpdatePackage(PackageListViewModel package)
        {
            PackageType packageType = _packageRepository.GetItem(package.Id);
            packageType.Name = package.Name;
            packageType.Price = package.Price;
            packageType.RoomType = package.RoomType;
            packageType.Days = package.Days;
            packageType.Enabled = true;
            _packageRepository.SaveItem(packageType);
        }
    }
}
