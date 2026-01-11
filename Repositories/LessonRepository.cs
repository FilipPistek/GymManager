using Microsoft.Data.SqlClient;
using GymManager.Entities;

namespace GymManager.Repositories
{
    /// <summary>
    /// Repozitář pro správu lekcí.
    /// Zajišťuje načítání rozvrhu a vkládání nových lekcí.
    /// </summary>
    public class LessonRepository : BaseRepository
    {
        /// <summary>
        /// Načte kompletní rozvrh lekcí.
        /// Data jsou načtena z pohhledu 'Lesson_Schedule'.
        /// </summary>
        public List<Lesson> GetSchedule()
        {
            var list = new List<Lesson>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT lesson_id, lesson_name, date_and_time, capacity, name, surname, specialization
                               FROM Lesson_Schedule";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Lesson
                            {
                                Id = (int)reader["lesson_id"],
                                Name = reader["lesson_name"].ToString(),
                                DateAndTime = (DateTime)reader["date_and_time"],
                                Capacity = (int)reader["capacity"],
                                TrainerName = reader["name"].ToString(),
                                TrainerSurname = reader["surname"].ToString(),
                                TrainerSpecialization = reader["specialization"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Vloží novou lekci do databáze.
        /// </summary>
        public void Add(Lesson lesson)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Lessons (name, date_and_time, capacity, trainer_id)
                               VALUES (@name, @date_and_time, @capacity, @trainer_id)";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", lesson.Name);
                    cmd.Parameters.AddWithValue("@date_and_time", lesson.DateAndTime);
                    cmd.Parameters.AddWithValue("@capacity", lesson.Capacity);
                    cmd.Parameters.AddWithValue("@trainer_id", lesson.TrainerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
