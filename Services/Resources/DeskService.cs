using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Repository;
using SQLite;

namespace OwlReadingRoom.Services.Resources;

public class DeskService : IDeskService
{
    private readonly IRepository<Desk> _deskRepository;

    public DeskService(IRepository<Desk> deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public TableQuery<Desk> TableQuery => _deskRepository.Table;
}
