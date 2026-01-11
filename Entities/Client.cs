namespace GymManager.Entities
{

    /// <summary>
    /// Třída reprezentující entitu klient.
    /// </summary>
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Credit { get; set; }
        public bool IsActive { get; set; }

        public string FullName => $"{Name} {Surname}";
    }
}
