using System.Configuration;
using Microsoft.Data.SqlClient;

namespace GymManager.Repositories
{
    /// <summary>
    /// Abstraktní bázová třída pro ostatní třídy repozitáře.
    /// Slouží pro získávání Connection Stringu a vytváření spojení.
    /// </summary>
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Konstruktor, který se zavolá automaticky při vytváření potomka.
        /// Načte nastavení připojení z externího souboru App.config.
        /// </summary>
        public BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString; // Zde zadej název databáze
        }

        /// <summary>
        /// Tovární metoda pro vytvoření nové instance spojení.
        /// </summary>
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
