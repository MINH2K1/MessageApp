namespace MessageApplication.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}
