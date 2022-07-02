namespace Bunkograph.Models
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Name { get; set; }

        public Publisher(string name)
        {
            Name = name;
        }
    }
}
