namespace SmartCardApi.Models.DTOs.Card
{
    public class CardGetDTO
    {
        public Guid Id { get; set; }

        public string Word { get; set; }

        public string Translation { get; set; }

        public int LearningRate { get; set; } = 0;

        public Guid UserId { get; set; }
    }
}
