using Domain.DTOs.Card;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using SmartCardApi.BLL.Services;
using SmartCardApi.Models.Cards;
using Tests.Infrastructure;

namespace Tests.ApplicationCore
{
    public class CardServiceTests
    {
        [Test]
        public void CardService_GetAll_ReturnsAllCards()
        {
            // Arrange
            var expected = CardModels.ToList();
            var mockCardRepository = new Mock<ICardRepository>();

            mockCardRepository
                .Setup(x => x.GetAll())
                .Returns(CardEntities.AsEnumerable());

            var cardService = new CardService(mockCardRepository.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = cardService.GetAllCards();

            // Assert
            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.UserId));
        }

        [TestCase("2298735a-9353-4dc1-b076-08dcab45d85b")]
        [TestCase("24d6f1c3-325b-465b-6c8c-08dcabd89020")]
        public void CardService_GetCardsByUserId_ReturnsAllCardsForUser(string userId)
        {
            // Arrange
            var parsedId = Guid.Parse(userId);
            var expected = CardModels.Where(card => card.UserId == parsedId).ToList();
            var mockCardRepository = new Mock<ICardRepository>();

            mockCardRepository
                .Setup(x => x.GetAllByUserId(parsedId))
                .Returns(CardEntities.Where(card => card.UserId == parsedId).AsEnumerable());

            var cardService = new CardService(mockCardRepository.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = cardService.GetCardsByUserId(parsedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase("5f964b01-1f6c-4715-acfb-08dcab4aab7c")]
        [TestCase("f03dae69-63a4-4607-acfc-08dcab4aab7c")]
        [TestCase("d023ae69-32d4-5217-acfc-08dcab4aab7c")]
        public void CardService_GetCardById_ReturnsCard(string cardId)
        {
            // Arrange
            var parsedId = Guid.Parse(cardId);
            var expected = CardModels.FirstOrDefault(card => card.Id == parsedId);
            var mockCardRepository = new Mock<ICardRepository>();

            mockCardRepository
                .Setup(x => x.GetById(parsedId))
                .Returns(CardEntities.FirstOrDefault(card => card.Id == parsedId));

            var cardService = new CardService(mockCardRepository.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = cardService.GetCardById(parsedId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CardService_UpdateCard_UpdatesCard()
        {
            // Arrange
            var expected = CardModels.First();
            expected.Word = "newWord";

            var mockCardRepository = new Mock<ICardRepository>();

            mockCardRepository
                .Setup(x => x.GetById(expected.Id))
                .Returns(CardEntities.First());

            var cardService = new CardService(mockCardRepository.Object, UnitTestHelper.CreateMapperProfile());
            var cardToUpdate = new CardUpdateDTO { Id = expected.Id, Word = expected.Word, Translation = expected.Translation };

            // Act
            var actual = cardService.UpdateCard(cardToUpdate, expected.UserId);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CardService_CreateCard_CreatesCard()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cardCreateDto = new CardCreateDTO { Word = "test", Translation = "test" };
            var card = new Card
            {
                Id = Guid.NewGuid(),
                Word = cardCreateDto.Word,
                Translation = cardCreateDto.Translation,
                LearningRate = 0,
                UserId = userId
            };

            var mockCardRepository = new Mock<ICardRepository>();

            mockCardRepository
                .Setup(x => x.Create(It.IsAny<Card>(), userId))
                .Returns(card);

            var cardService = new CardService(mockCardRepository.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var actual = cardService.CreateCard(cardCreateDto, userId);

            // Assert
            actual.Should().NotBeNull();
        }

        [TestCase("5f964b01-1f6c-4715-acfb-08dcab4aab7c")]
        [TestCase("f03dae69-63a4-4607-acfc-08dcab4aab7c")]
        [TestCase("d023ae69-32d4-5217-acfc-08dcab4aab7c")]
        public void CardService_DeleteCard_DeletesCard(string cardId)
        {
            // Arrange
            var parsedId = Guid.Parse(cardId);
            var expected = CardModels.FirstOrDefault(card => card.Id == parsedId);
            var mockCardRepository = new Mock<ICardRepository>();

            mockCardRepository
                .Setup(x => x.Delete(parsedId));

            var cardService = new CardService(mockCardRepository.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            cardService.DeleteCard(parsedId);
            var deletedCard = cardService.GetCardById(parsedId);

            // Assert
            Assert.That(deletedCard, Is.Null);
        }

        [TestCase("5f964b01-1f6c-4715-acfb-08dcab4aab7c")]
        [TestCase("f03dae69-63a4-4607-acfc-08dcab4aab7c")]
        [TestCase("d023ae69-32d4-5217-acfc-08dcab4aab7c")]
        public void CardService_IncreaseLearningRate_IncreasesLearningRate(string cardId)
        {
            // Arrange
            var parsedId = Guid.Parse(cardId);
            var expected = CardModels.FirstOrDefault(card => card.Id == parsedId);
            expected.LearningRate += 1;
            var mockCardRepository = new Mock<ICardRepository>();

            mockCardRepository
                .Setup(x => x.GetById(parsedId))
                .Returns(CardEntities.FirstOrDefault(card => card.Id == parsedId));

            var cardService = new CardService(mockCardRepository.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var card = cardService.IncreaseLearningRate(parsedId);

            // Assert
            Assert.That(card.LearningRate, Is.EqualTo(expected.LearningRate));
        }

        [TestCase("5f964b01-1f6c-4715-acfb-08dcab4aab7c")]
        [TestCase("f03dae69-63a4-4607-acfc-08dcab4aab7c")]
        [TestCase("d023ae69-32d4-5217-acfc-08dcab4aab7c")]
        public void CardService_DecreaseLearningRate_DecreasesLearningRate(string cardId)
        {
            // Arrange
            var parsedId = Guid.Parse(cardId);
            var expected = CardModels.FirstOrDefault(card => card.Id == parsedId);
            var mockCardRepository = new Mock<ICardRepository>();

            mockCardRepository
                .Setup(x => x.GetById(parsedId))
                .Returns(CardEntities.FirstOrDefault(card => card.Id == parsedId));

            var cardService = new CardService(mockCardRepository.Object, UnitTestHelper.CreateMapperProfile());

            // Act
            var card = cardService.DecreaseLearningRate(parsedId);

            // Assert
            Assert.That(card.LearningRate, Is.EqualTo(expected.LearningRate));
        }

        internal static List<CardGetDTO> CardModels =>
            new List<CardGetDTO>()
            {
                new CardGetDTO
                 {
                     Id = Guid.Parse("5f964b01-1f6c-4715-acfb-08dcab4aab7c"),
                     Word = "hello",
                     Translation = "hola",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                 },
                 new CardGetDTO
                 {
                     Id = Guid.Parse("f03dae69-63a4-4607-acfc-08dcab4aab7c"),
                     Word = "sun",
                     Translation = "sol",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                 },
                 new CardGetDTO
                 {
                     Id = Guid.Parse("d023ae69-32d4-5217-acfc-08dcab4aab7c"),
                     Word = "sky",
                     Translation = "cielo",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                 },
                 new CardGetDTO
                 {
                     Id = Guid.Parse("fd6ec800-6d2b-40f6-9124-a742d167206e"),
                     Word = "night",
                     Translation = "noche",
                     LearningRate = 0,
                     UserId = Guid.Parse("24d6f1c3-325b-465b-6c8c-08dcabd89020"),
                 },
                 new CardGetDTO
                 {
                     Id = Guid.Parse("b07e0326-6070-4d1a-977c-3e93971f1fcf"),
                     Word = "focus",
                     Translation = "enfoque",
                     LearningRate = 0,
                     UserId = Guid.Parse("24d6f1c3-325b-465b-6c8c-08dcabd89020"),
                 }
            };

        internal static List<Card> CardEntities =>
            new List<Card>()
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
    };
}