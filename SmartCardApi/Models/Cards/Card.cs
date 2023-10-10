using SmartCardApi.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace SmartCardApi.Models.Cards
{
    public class Card
    {
        public Guid Id { get; set; }

        [Required]
        public string Word { get; set; }

        [Required]
        public string Translation { get; set; }

        public int LearningRate { get; set; } = 0;

        /*public string AppUserId { get; set; }

        public AppUser User { get; set; }*/
    }
}
