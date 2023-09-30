namespace SmartCardApi.Models
{
    public class Card
    {
        public Guid Id { get; set; }

        public string Word { get; set; }

        public string Translation { get; set; }

        public byte LearningRate { get; set; }
    }
}
