using OwlReadingRoom.Models;
using SQLite;

namespace OwlReadingRoom.Services.Resources;

public interface IDeskService
{
    TableQuery<Desk> TableQuery { get; }
}
