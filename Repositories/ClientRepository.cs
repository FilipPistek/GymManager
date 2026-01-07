using Microsoft.Data.SqlClient;
using GymManager.Entities;

namespace GymManager.Repositories
{
    public class ClientRepository : BaseRepository
    {
        public List<Client> GetAll()
        {
            var list = new List<Client>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Clients";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Client
                            {
                                Id = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(), // Čteme příjmení
                                Email = reader["email"].ToString(),
                                DateOfBirth = (DateTime)reader["date_of_birth"],
                                Credit = Convert.ToDouble(reader["credit"]),
                                IsActive = (bool)reader["is_active"]
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void Add(Client client)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Clients (name, surname, email, date_of_birth, credit, is_active) 
                               VALUES (@name, @surname, @email, @date_of_birth, @credit, @is_active)";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", client.Name);
                    cmd.Parameters.AddWithValue("@surname", client.Surname);
                    cmd.Parameters.AddWithValue("@email", client.Email);
                    cmd.Parameters.AddWithValue("@date_of_birth", client.DateOfBirth);
                    cmd.Parameters.AddWithValue("@credit", client.Credit);
                    cmd.Parameters.AddWithValue("@is_active", client.IsActive); 
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Client client)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE Clients 
                               SET name = @name, surname = @surname, email = @email 
                               WHERE id = @id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", client.Id);
                    cmd.Parameters.AddWithValue("@name", client.Name);
                    cmd.Parameters.AddWithValue("@surname", client.Surname);
                    cmd.Parameters.AddWithValue("@email", client.Email);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public void Delete(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Clients WHERE id = @id";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Client GetById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Clients WHERE id = @id";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Client
                            {
                                Id = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Surname = reader["surname"].ToString(),
                                Email = reader["email"].ToString(),
                                DateOfBirth = (DateTime)reader["date_of_birth"],
                                Credit = Convert.ToDouble(reader["credit"]),
                                IsActive = (bool)reader["is_active"]
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
