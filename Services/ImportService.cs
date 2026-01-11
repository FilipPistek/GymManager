using GymManager.Entities;
using GymManager.Repositories;

namespace GymManager.Services
{
    /// <summary>
    /// Služba zajišťující hromadný import dat z externích souborů (CSV).
    /// </summary>
    public class ImportService
    {
        private readonly LessonRepository lessonRepository = new LessonRepository();
        private readonly TrainerRepository trainerRepository = new TrainerRepository();

        /// <summary>
        /// Načte lekce z CSV souboru a uloží je do databáze.
        /// Očekávaný formát řádku: Name,DatumCas,Kapacita,TrenerId
        /// </summary>
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

        /// <summary>
        /// Importuje trenéry z CSV souboru.
        /// Řeší převod textové reprezentace specializace na výčtový typ (Enum).
        /// Formát: Jmeno,Prijmeni,Specializace
        /// </summary>
        public void ImportTrainers(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Soubor neexistuje.");
                return;
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                int count = 0;

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length < 3) continue;

                    string name = parts[0].Trim();
                    string surname = parts[1].Trim();
                    string specString = parts[2].Trim();

                    if (Enum.TryParse(specString, true, out TrainerSpecialization parsedSpecialization))
                    {
                        var trainer = new Trainer
                        {
                            Name = name,
                            Surname = surname,
                            Specialization = parsedSpecialization
                        };

                        trainerRepository.Add(trainer);
                        count++;
                    }
                    else
                    {
                        Console.WriteLine($"Chyba na řádku: '{line}'. Neznámá specializace: {specString}");
                    }
                }
                Console.WriteLine($"Importováno {count} trenérů.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kritická chyba importu trenérů: " + ex.Message);
            }
        }
    }
}
