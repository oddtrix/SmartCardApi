using SmartCardApi.Models.User;

namespace SmartCardApi.Models.Cards
{
    public class Card
    {
        private int databaseLearningRate;

        public Guid Id { get; set; }

        public string Word { get; set; }

        public string Translation { get; set; }

        public int LearningRate
        {
            get => this.databaseLearningRate;
            set
            {
                if (this.databaseLearningRate > 0 || this.databaseLearningRate < 100)
                {
                    this.databaseLearningRate = value;
                }
            }
        }

        public Guid UserId { get; set; }

        public DomainUser User { get; set; }
    }
}
