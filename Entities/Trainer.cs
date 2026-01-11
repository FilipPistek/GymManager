namespace GymManager.Entities
{
    /// <summary>
    /// Výčet definující možné specializace trenérů.
    /// </summary>
    public enum TrainerSpecialization
    {
        Yoga,
        Silovy,
        Cardio,
        Crossfit
    }

    /// <summary>
    /// Třída reprezentující entitu Trenéra.
    /// </summary>
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public TrainerSpecialization Specialization { get; set; }
        public string FullName => $"{Name} {Surname}";
    }
}
