namespace GymManager.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAndTime { get; set; }
        public int Capacity { get; set; }
        public int TrainerId { get; set; } 
        public string TrainerName { get; set; }
        public string TrainerSurname { get; set; }
        public string TrainerSpecialization { get; set; }
        public string TrainerFullName => $"{TrainerName} {TrainerSurname}";
    }
}
