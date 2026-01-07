using GymManager.Entities;
using GymManager.Repositories;

namespace GymManager.Services
{
    public class ImportService
    {
        private readonly LessonRepository lessonRepository = new LessonRepository();
        private readonly TrainerRepository trainerRepository = new TrainerRepository();

        public void ImportLessons(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Soubor neexistuje!");
                return;
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                int count = 0;

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length < 4) continue;

                    var lesson = new Lesson
                    {
                        Name = parts[0].Trim(),
                        DateAndTime = DateTime.Parse(parts[1]),
                        Capacity = int.Parse(parts[2]),
                        TrainerId = int.Parse(parts[3])
                    };
                    lessonRepository.Add(lesson);
                    count++;
                }
                Console.WriteLine($"Úspěšně importováno {count} lekcí.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba importu: " + ex.Message);
            }
        }

        public void ImportTrainers(string filePath)
        {
            if (!File.Exists(filePath)) { Console.WriteLine("Soubor neexistuje."); return; }

            try
            {
                var lines = File.ReadAllLines(filePath);
                int count = 0;

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length < 3) continue;

                    var trainer = new Trainer
                    {
                        Name = parts[0].Trim(),
                        Surname = parts[1].Trim(),
                        Specialization = parts[2].Trim()
                    };
                    trainerRepository.Add(trainer);
                    count++;
                }
                Console.WriteLine($"Importováno {count} trenérů.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba importu trenérů: " + ex.Message);
            }
        }
    }
}
