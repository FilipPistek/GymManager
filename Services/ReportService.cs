using Microsoft.Data.SqlClient;
using GymManager.Repositories;

namespace GymManager.Services
{
    /// <summary>
    /// Služba pro generování statistických reportů.
    /// Slouží k přehledu vytíženosti trenérů a lekcí.
    /// </summary>
    public class ReportService : BaseRepository
    {
        /// <summary>
        /// Vygeneruje a vypíše do konzole statistiku pro každého trenéra.
        /// </summary>
        public void GenerateStats()
        {
            Console.WriteLine("\n=== STATISTIKA TRENÉRŮ ===");
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        T.name, 
                        T.surname,
                        COUNT(DISTINCT L.id) as lessons_count,
                        COUNT(B.id) as bookings_count
                    FROM Trainers T
                    LEFT JOIN Lessons L ON T.id = L.trainer_id
                    LEFT JOIN Bookings B ON L.id = B.lesson_id
                    GROUP BY T.name, T.surname";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("{0,-25} {1,-10} {2,-10}", "Trenér", "Lekcí", "Rezervací");
                        Console.WriteLine(new string('-', 50));
                        while (reader.Read())
                        {
                            string fullName = $"{reader["name"]} {reader["surname"]}";
                            Console.WriteLine("{0,-25} {1,-10} {2,-10}",
                                fullName,
                                reader["lessons_count"],
                                reader["bookings_count"]);
                        }
                    }
                }
            }
        }
    }
}
