using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Constants;
using SQLite;
using System.Diagnostics;


namespace OwlReadingRoom.Services.Database
{
    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private SQLiteConnection _connection;
        public SQLiteConnection Connection { get => _connection; }

        public void Init<T>() where T : BaseModel, new()
        {
            try
            {
                _connection = new SQLiteConnection(DataBaseConstants.DatabasePath, DataBaseConstants.Flags);

                // Ensure that tables are created or initialized
                _connection.CreateTable<T>();
                
            }
            catch (Exception ex)
            {
                // Log other exceptions: the exception will be later caught by global handler
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void CloseConnection()
        {
            _connection?.Close();  // Close the connection if it's open
            _connection?.Dispose();  // Dispose the connection to release resources
            _connection = null; // Set to null to avoid accidental usage after closing
        }
    }
}

