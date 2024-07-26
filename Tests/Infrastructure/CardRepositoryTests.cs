using SmartCardApi.Contexts;
using SmartCardApi.Models.Cards;

namespace Tests.Infrastructure
{
    [TestFixture]
    public class CardRepositoryTests
    {
        [Test]
        public void CardRepository_GetAll_ReturnsAllValues()
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var cardRepository = new CardRepository(context);
            var cards = cardRepository.GetAll();

            // Assert 
            Assert.That(cards, Is.EqualTo(ExpectedCards).Using(new CardEqualityComparer()), message: "GetAll method works incorrect");
        }

        [TestCase("5f964b01-1f6c-4715-acfb-08dcab4aab7c")]
        [TestCase("f03dae69-63a4-4607-acfc-08dcab4aab7c")]
        [TestCase("d023ae69-32d4-5217-acfc-08dcab4aab7c")]
        public void CardRepository_GetById_ReturnsSingleValue(string id)
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var cardRepository = new CardRepository(context);
            var card = cardRepository.GetById(Guid.Parse(id));

            var expected = ExpectedCards.FirstOrDefault(x => x.Id == Guid.Parse(id));

            // Assert
            Assert.That(card, Is.EqualTo(expected).Using(new CardEqualityComparer()), message: "GetById method works incorrect");
        }

        [TestCase("5f964b01-1f6c-4715-acfb-08dcab4aab7c")]
        [TestCase("f03dae69-63a4-4607-acfc-08dcab4aab7c")]
        [TestCase("d023ae69-32d4-5217-acfc-08dcab4aab7c")]
        public void CardRepository_Delete_DeletesEntity(string id)
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var cardRepository = new CardRepository(context);
            cardRepository.Delete(Guid.Parse(id));

            var card = cardRepository.GetById(Guid.Parse(id));

            // Assert
            Assert.That(card, Is.EqualTo(null).Using(new CardEqualityComparer()), message: "Delete method works incorrect");
        }

        [Test]
        public void CardRepository_Update_UpdatesEntity()
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var cardRepository = new CardRepository(context);
            var cardToUpdate = ExpectedCards.First();
            cardToUpdate.Word = "newWord";
            cardToUpdate.LearningRate = 1;

            var card = cardRepository.Update(cardToUpdate);

            // Assert
            Assert.That(card, Is.EqualTo(cardToUpdate).Using(new CardEqualityComparer()), message: "Update method works incorrect");
        }

        [TestCase("3e242bae-2404-4503-a9be-c67e4889bfb7")]
        public void CardRepository_Create_CreatesEntity(string userId)
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var cardRepository = new CardRepository(context);
            var newCard = new Card()
            {
                Id = Guid.NewGuid(),
                Word = "test",
                Translation = "test",
                LearningRate = 0,
                UserId = Guid.Parse(userId)
            };
            var card = cardRepository.Create(newCard, Guid.Parse(userId));
            var expected = cardRepository.GetById(card.Id);

            // Assert
            Assert.That(card, Is.EqualTo(expected).Using(new CardEqualityComparer()), message: "Create method works incorrect");
        }

        internal static IEnumerable<Card> ExpectedCards =>
            new[]
            {
                new Card
                 {
                     Id = Guid.Parse("5f964b01-1f6c-4715-acfb-08dcab4aab7c"),
                     Word = "hello",
                     Translation = "hola",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                     User = AppDomainRepositoryTests.ExpectedUsers.First()
                 },
                 new Card
                 {
                     Id = Guid.Parse("f03dae69-63a4-4607-acfc-08dcab4aab7c"),
                     Word = "sun",
                     Translation = "sol",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                     User = AppDomainRepositoryTests.ExpectedUsers.First()
                 },
                 new Card
                 {
                     Id = Guid.Parse("d023ae69-32d4-5217-acfc-08dcab4aab7c"),
                     Word = "sky",
                     Translation = "cielo",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                     User = AppDomainRepositoryTests.ExpectedUsers.First()
                 },
                 new Card
                 {
                     Id = Guid.Parse("fd6ec800-6d2b-40f6-9124-a742d167206e"),
                     Word = "night",
                     Translation = "noche",
                     LearningRate = 0,
                     UserId = Guid.Parse("24d6f1c3-325b-465b-6c8c-08dcabd89020"),
                     User = AppDomainRepositoryTests.ExpectedUsers.Last()
                 },
                 new Card
                 {
                     Id = Guid.Parse("b07e0326-6070-4d1a-977c-3e93971f1fcf"),
                     Word = "focus",
                     Translation = "enfoque",
                     LearningRate = 0,
                     UserId = Guid.Parse("24d6f1c3-325b-465b-6c8c-08dcabd89020"),
                     User = AppDomainRepositoryTests.ExpectedUsers.Last()
                 }
            };
    }
}
