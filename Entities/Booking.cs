namespace GymManager.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int LessonId { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
