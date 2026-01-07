using Microsoft.Data.SqlClient;

namespace GymManager.Repositories
{
    public class BookingRepository : BaseRepository
    {
        public void CreateBooking(int clientId, int lessonId, double price)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string checkSql = "SELECT credit FROM Clients WHERE id = @id";
                        double currentCredit = 0;
                        using (var cmd = new SqlCommand(checkSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", clientId);
                            var result = cmd.ExecuteScalar();
                            if (result != null) currentCredit = Convert.ToDouble(result);
                        }

                        if (currentCredit < price)
                            throw new Exception($"Nedostatek kreditu (Máš: {currentCredit}, Cena: {price})");

                        string insertSql = "INSERT INTO Bookings (client_id, lesson_id) VALUES (@client_id, @lesson_id)";
                        using (var cmd = new SqlCommand(insertSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@client_id", clientId);
                            cmd.Parameters.AddWithValue("@lesson_id", lessonId);
                            cmd.ExecuteNonQuery();
                        }

                        string updateSql = "UPDATE Clients SET credit = credit - @price WHERE id = @client_id";
                        using (var cmd = new SqlCommand(updateSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@price", price);
                            cmd.Parameters.AddWithValue("@client_id", clientId);
                            cmd.ExecuteNonQuery();
                        }

                        string logSql = "INSERT INTO Logs (message, type) VALUES (@message, 'BOOKING')";
                        using (var cmd = new SqlCommand(logSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@message", $"Client {clientId} booked lesson {lessonId}");
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        Console.WriteLine("Rezervace úspěšně dokončena.");
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
