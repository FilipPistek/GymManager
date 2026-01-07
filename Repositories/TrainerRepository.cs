using Microsoft.Data.SqlClient;
using GymManager.Entities;

namespace GymManager.Repositories
{
    public class TrainerRepository : BaseRepository
    {
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
                            list.Add(new Trainer
                            {
                                Id = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Specialization = reader["specialization"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

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
                    cmd.Parameters.AddWithValue("@specialization", trainer.Specialization);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
