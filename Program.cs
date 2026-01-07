using GymManager.Entities;
using GymManager.Repositories;
using GymManager.Services;

namespace GymManager
{
    internal class Program
    {
        private static readonly ClientRepository clientRepository = new ClientRepository();
        private static readonly LessonRepository lessonRepository = new LessonRepository();
        private static readonly BookingRepository bookingRepository = new BookingRepository();
        private static readonly ReportService reportService = new ReportService();
        private static readonly ImportService importService = new ImportService();

        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== GYM MANAGER DATABASE ===");
                Console.WriteLine("1. Seznam klientů");
                Console.WriteLine("2. Přidat klienta");
                Console.WriteLine("3. Upravit klienta");
                Console.WriteLine("4. Smazat klienta");
                Console.WriteLine("5. Rozvrh lekcí");
                Console.WriteLine("6. Vytvořit rezervaci (Transakce)");
                Console.WriteLine("7. Statistiky rezervací (Report)");
                Console.WriteLine("8. Import Lekcí (CSV)");
                Console.WriteLine("9. Import Trenérů (CSV)");
                Console.WriteLine("0. Konec");
                Console.Write("\nVyber číslo pro akci: ");

                switch (Console.ReadLine())
                {
                    case "1": ShowClients(); break;
                    case "2": AddClient(); break;
                    case "3": UpdateClient(); break;
                    case "4": DeleteClient(); break;
                    case "5": ShowSchedule(); break;
                    case "6": MakeBooking(); break;
                    case "7": reportService.GenerateStats(); break;
                    case "8": ImportLessons(); break;
                    case "9": ImportTrainers(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Neplatná volba."); break;
                }

                if (running)
                {
                    Console.WriteLine("\nStiskni Enter pro pokračování...");
                    Console.ReadLine();
                }
            }
        }

        static void ShowClients()
        {
            try
            {
                var clients = clientRepository.GetAll();
                Console.WriteLine("\n{0,-5} {1,-30} {2,-10}", "ID", "Jméno Příjmení", "Kredit");
                Console.WriteLine(new string('-', 50));

                foreach (var c in clients)
                {
                    Console.WriteLine("{0,-5} {1,-30} {2,-10}", c.Id, c.FullName, c.Credit);
                }
            }
            catch (Exception ex) { Console.WriteLine("Chyba DB: " + ex.Message); }
        }

        static void AddClient()
        {
            try
            {
                var c = new Client();
                Console.WriteLine("\n--- Přidání Klienta ---");
                Console.Write("Jméno (Name): "); c.Name = Console.ReadLine();
                Console.Write("Příjmení (Surname): "); c.Surname = Console.ReadLine();
                Console.Write("Email: "); c.Email = Console.ReadLine();
                Console.Write("Datum narození (rrrr-mm-dd): "); c.DateOfBirth = DateTime.Parse(Console.ReadLine());
                c.Credit = 1000;
                c.IsActive = true;

                clientRepository.Add(c);
                Console.WriteLine("Klient uložen.");
            }
            catch (Exception ex) { Console.WriteLine("Chyba vstupu: " + ex.Message); }
        }

        static void UpdateClient()
        {
            Console.Write("Zadej ID klienta pro úpravu: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var client = clientRepository.GetById(id);
                if (client == null) { Console.WriteLine("Klient nenalezen."); return; }

                Console.WriteLine($"Upravuješ: {client.FullName}");
                Console.Write("Nové jméno (Enter = beze změny): ");
                string val = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(val)) client.Name = val;

                Console.Write("Nové příjmení (Enter = beze změny): ");
                val = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(val)) client.Surname = val;

                Console.Write("Nový email (Enter = beze změny): ");
                val = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(val)) client.Email = val;

                try
                {
                    clientRepository.Update(client);
                    Console.WriteLine("Klient aktualizován.");
                }
                catch (Exception ex) { Console.WriteLine("Chyba: " + ex.Message); }
            }
        }

        static void DeleteClient()
        {
            Console.Write("Zadej ID klienta ke smazání: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    clientRepository.Delete(id);
                    Console.WriteLine("Klient smazán.");
                }
                catch (Exception ex) { Console.WriteLine("Nelze smazat (má rezervace?): " + ex.Message); }
            }
        }

        static void ShowSchedule()
        {
            try
            {
                var lessons = lessonRepository.GetSchedule();
                Console.WriteLine("\n{0,-5} {1,-20} {2,-20} {3,-25}", "ID", "Lekce", "Datum", "Trenér");
                Console.WriteLine(new string('-', 75));
                foreach (var l in lessons)
                {
                    // Použijeme l.TrainerFullName
                    Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-25}",
                        l.Id, l.Name, l.DateAndTime, l.TrainerFullName);
                }
            }
            catch (Exception ex) { Console.WriteLine("Chyba DB: " + ex.Message); }
        }

        static void MakeBooking()
        {
            Console.WriteLine("\n--- Nová Rezervace ---");
            ShowClients();
            ShowSchedule();

            try
            {
                Console.Write("\nZadej ID Klienta: "); int cid = int.Parse(Console.ReadLine());
                Console.Write("Zadej ID Lekce: "); int lid = int.Parse(Console.ReadLine());

                bookingRepository.CreateBooking(cid, lid, 100.0);
            }
            catch (Exception ex) { Console.WriteLine("CHYBA REZERVACE: " + ex.Message); }
        }

        static void ImportLessons()
        {
            Console.Write("Zadej cestu k CSV (např. C:\\Data\\lekce.csv): ");
            string path = Console.ReadLine();
            importService.ImportLessons(path);
        }

        static void ImportTrainers()
        {
            Console.Write("Cesta k CSV trenérů (format: Jmeno,Prijmeni,Specializace): ");
            importService.ImportTrainers(Console.ReadLine());
        }
    }
}
