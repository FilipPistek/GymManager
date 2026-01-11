using System.Configuration;
using Microsoft.Data.SqlClient;

namespace GymManager.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        public BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NAZEV_DATABAZE"].ConnectionString; // Zde zadej jméno databáze
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
