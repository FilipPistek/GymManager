using GymManager.Entities;
using GymManager.Repositories;
using GymManager.Services;

namespace GymManager
{
    /// <summary>
    /// Hlavní třída aplikace. Zajišťuje uživatelské rozhraní v konzoli
    /// a deleguje požadavky na příslušné repozitáře a služby.
    /// </summary>
    internal class Program
    {
        private static readonly ClientRepository clientRepository = new ClientRepository();
        private static readonly TrainerRepository trainerRepository = new TrainerRepository();
        private static readonly LessonRepository lessonRepository = new LessonRepository();
        private static readonly BookingRepository bookingRepository = new BookingRepository();
        private static readonly ReportService reportService = new ReportService();
        private static readonly ImportService importService = new ImportService();

        /// <summary>
        /// Vstupní bod programu. Obsahuje hlavní smyčku aplikace a vykreslení menu.
        /// </summary>
        static void Main(string[] args)
        {
            bool running = true;
            // Hlavní cyklus aplikace, který běží, dokud uživatel nezvolí ukončení (0).
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== GYM MANAGER DATABASE ===");
                Console.WriteLine("1. Seznam klientů");
                Console.WriteLine("2. Přidat klienta");
                Console.WriteLine("3. Upravit klienta");
                Console.WriteLine("4. Smazat klienta");
                Console.WriteLine("5. Seznam Trenérů");
                Console.WriteLine("6. Rozvrh lekcí");
                Console.WriteLine("7. Vytvořit rezervaci (Transakce)");
                Console.WriteLine("8. Statistiky rezervací (Report)");
                Console.WriteLine("9. Import Lekcí (CSV)");
                Console.WriteLine("10. Import Trenérů (CSV)");
                Console.WriteLine("0. Konec");
                Console.Write("\nVyber číslo pro akci: ");

                // Zpracování volby uživatele
                switch (Console.ReadLine())
                {
                    case "1": ShowClients(); break;
                    case "2": AddClient(); break;
                    case "3": UpdateClient(); break;
                    case "4": DeleteClient(); break;
                    case "5": ShowTrainers(); break;
                    case "6": ShowSchedule(); break;
                    case "7": MakeBooking(); break;
                    case "8": reportService.GenerateStats(); break;
                    case "9": ImportLessons(); break;
                    case "10": ImportTrainers(); break;
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

        /// <summary>
        /// Získá seznam všech klientů z databáze a vypíše je do tabulky.
        /// </summary>
        static void ShowClients()
        {
            try
            {
                var clients = clientRepository.GetAll();
                // Formátovaný výpis hlavičky tabulky (zarovnání sloupců)
                Console.WriteLine("\n{0,-5} {1,-30} {2,-10}", "ID", "Jméno Příjmení", "Kredit");
                Console.WriteLine(new string('-', 50));

                foreach (var c in clients)
                {
                    Console.WriteLine("{0,-5} {1,-30} {2,-10}", c.Id, c.FullName, c.Credit);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba DB: " + ex.Message);
            }
        }

        /// <summary>
        /// Interaktivní formulář pro vytvoření nového klienta.
        /// </summary>
        static void AddClient()
        {
            try
            {
                var c = new Client();
                Console.WriteLine("\n--- Přidání Klienta ---");

                Console.Write("Jméno (Name): "); c.Name = Console.ReadLine();
                Console.Write("Příjmení (Surname): "); c.Surname = Console.ReadLine();
                Console.Write("Email: "); c.Email = Console.ReadLine();
                Console.Write("Datum narození (rrrr-mm-dd): "); c.DateOfBirth = DateTime.Parse(Console.ReadLine()); // Může vyhodit výjimku při špatném formátu

                c.Credit = 1000;
                c.IsActive = true;

                clientRepository.Add(c);
                Console.WriteLine("Klient uložen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba vstupu: " + ex.Message);
            }
        }

        /// <summary>
        /// Umožňuje editaci existujícího klienta. Pokud uživatel zadá prázdný řetězec, hodnota se nemění.
        /// </summary>
        static void UpdateClient()
        {
            Console.Write("Zadej ID klienta pro úpravu: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                // Nejprve načteme aktuální data z DB
                var client = clientRepository.GetById(id);
                if (client == null) { Console.WriteLine("Klient nenalezen."); return; }

                Console.WriteLine($"Upravuješ: {client.FullName}");

                // Logika pro částečnou aktualizaci (ponechání původních hodnot při prázdném vstupu)
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

        /// <summary>
        /// Zobrazí seznam tabulky trenérů.
        /// </summary>
        static void ShowTrainers()
        {
            try
            {
                var trainers = trainerRepository.GetAll();
                Console.WriteLine("\n{0,-5} {1,-30} {2,-10}", "ID", "Jméno Příjmení", "Specializace");
                Console.WriteLine(new string('-', 50));

                foreach (var t in trainers)
                {
                    Console.WriteLine("{0,-5} {1,-30} {2,-10}", t.Id, t.FullName, t.Specialization);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chyba DB: " + ex.Message);
            }
        }

        /// <summary>
        /// Smaže klienta podle zadaného ID.
        /// </summary>
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
                catch (Exception ex)
                {
                    // Častá chyba: nelze smazat klienta, pokud na něj existují vazby (např. v tabulce Rezervace)
                    Console.WriteLine("Nelze smazat (má rezervace?): " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Zobrazí rozvrh lekcí včetně jmen trenérů.
        /// </summary>
        static void ShowSchedule()
        {
            try
            {
                var lessons = lessonRepository.GetSchedule();
                Console.WriteLine("\n{0,-5} {1,-20} {2,-20} {3,-25}", "ID", "Lekce", "Datum", "Trenér");
                Console.WriteLine(new string('-', 75));
                foreach (var l in lessons)
                {
                    Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-25}",
                        l.Id, l.Name, l.DateAndTime, l.TrainerFullName);
                }
            }
            catch (Exception ex) { Console.WriteLine("Chyba DB: " + ex.Message); }
        }

        /// <summary>
        /// Vytvoří rezervaci pomocí transakce.
        /// </summary>
        static void MakeBooking()
        {
            Console.WriteLine("\n--- Nová Rezervace ---");
            // Pro přehlednost nejprve vypíšeme možnosti
            ShowClients();
            ShowSchedule();

            try
            {
                Console.Write("\nZadej ID Klienta: "); int cid = int.Parse(Console.ReadLine());
                Console.Write("Zadej ID Lekce: "); int lid = int.Parse(Console.ReadLine());

                // Volání repozitáře, který provede logiku transakce
                bookingRepository.CreateBooking(cid, lid, 100.0);
            }
            catch (Exception ex) { Console.WriteLine("CHYBA REZERVACE: " + ex.Message); }
        }

        /// <summary>
        /// Import lekcí z CSV souboru.
        /// </summary>
        static void ImportLessons()
        {
            Console.Write("Zadej cestu k CSV souboru (např. C:\\Data\\lessons.csv): ");
            string path = Console.ReadLine();
            importService.ImportLessons(path);
        }

        /// <summary>
        /// Import trenérů z CSV souboru.
        /// </summary>
        static void ImportTrainers()
        {
            Console.Write("Zadej cestu k CSV souboru (např. C:\\Data\\trainers.csv): ");
            importService.ImportTrainers(Console.ReadLine());
        }
    }
}
