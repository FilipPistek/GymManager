namespace GymManager.Entities
{
    public enum TrainerSpecialization
    {
        Yoga,
        Silovy,
        Cardio,
        Crossfit
    }

    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Specialization { get; set; }

        public string FullName => $"{Name} {Surname}";
    }
}
