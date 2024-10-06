using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Constants;
using SQLite;
using System.Diagnostics;


namespace OwlReadingRoom.Services.Database
{
    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private readonly object _lock = new object();

        private SQLiteConnection _connection;
        public SQLiteConnection Connection { get => _connection; } 
        public void OpenConnection()
        {
            lock (_lock)
            {
                try
                {
                    if (_connection is not null) return;

                    _connection = new SQLiteConnection(DataBaseConstants.DatabasePath, DataBaseConstants.Flags);

                }
                catch (Exception ex)
                {
                    // Log other exceptions: the exception will be later caught by global handler
                    Debug.WriteLine($"An error occurred: {ex.Message}");
                }
            }    
        }

        public SQLiteConnection OpenNewConnection()
        {
            return new SQLiteConnection(DataBaseConstants.DatabasePath, DataBaseConstants.Flags);
        }

        public void Init<T>() where T : BaseModel, new()
        {
            try
            {
                if (_connection is null)
                {
                    OpenConnection();
                } 
                   
                _connection.CreateTable<T>();

            }          
            catch (Exception ex)
            {
                // Log other exceptions: the exception will be later caught by global handler
                Debug.WriteLine($"An error occurred while creating the table for {typeof(T).Name}: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Debug.WriteLine("Closing database connection");
            _connection?.Close();  // Close the connection if it's open
            _connection?.Dispose();  // Dispose the connection to release resources
            _connection = null; // Set to null to avoid accidental usage after closing
        }

        public void BeginTransaction()
        {
            OpenConnection();
            _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _connection.Commit();
        }

        public void RollBack()
        {
            _connection.Rollback();
        }
    }
}

