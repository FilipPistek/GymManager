using Microsoft.Data.SqlClient;
using GymManager.Entities;

namespace GymManager.Repositories
{
    /// <summary>
    /// Repozitář pro správu dat trenérů.
    /// Dědí od BaseRepository pro sdílení logiky připojení k DB.
    /// </summary>
    public class TrainerRepository : BaseRepository
    {
        /// <summary>
        /// Načte všechny trenéry z databáze.
        /// </summary>
        public List<Trainer> GetAll()
        {
            var list = new List<Trainer>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Trainers";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string specString = reader["specialization"].ToString();
                            TrainerSpecialization parsedSpec = (TrainerSpecialization)Enum.Parse(typeof(TrainerSpecialization), specString);

                            list.Add(new Trainer
                            {
                                Id = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Specialization = parsedSpec
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Vloží nového trenéra do databáze.
        /// </summary>
        public void Add(Trainer trainer)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO Trainers (name, surname, specialization) VALUES (@name, @surname, @specialization)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", trainer.Name);
                    cmd.Parameters.AddWithValue("@surname", trainer.Surname);
                    cmd.Parameters.AddWithValue("@specialization", trainer.Specialization.ToString());

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
