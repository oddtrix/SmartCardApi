using Microsoft.EntityFrameworkCore;
using SmartCardApi.Contexts;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.User;

namespace Tests.Infrastructure
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<AppDomainDbContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<AppDomainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDomainDbContext(options))
            {
                SeedData(context);
            }

            return options;
        }

        public static void SeedData(AppDomainDbContext context)
        {
            var users = new DomainUser[]
            {
                new DomainUser
                {
                    Id = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                    Name = "John",
                    Surname = "Doe",
                    UserName = "JohnDoe123",
                    Email = "johndoe@gmail.com"
                },
                new DomainUser
                {
                    Id = Guid.Parse("24d6f1c3-325b-465b-6c8c-08dcabd89020"),
                    Name = "Dave",
                    Surname = "Johnson",
                    UserName = "DaveJohnson123",
                    Email = "davejohnson@gmail.com"
                }
            };
            var cards = new Card[] {
                 new Card
                 {
                     Id = Guid.Parse("5f964b01-1f6c-4715-acfb-08dcab4aab7c"),
                     Word = "hello",
                     Translation = "hola",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                     User = users[0]
                 },
                 new Card
                 {
                     Id = Guid.Parse("f03dae69-63a4-4607-acfc-08dcab4aab7c"),
                     Word = "sun",
                     Translation = "sol",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                     User = users[0]
                 },
                 new Card
                 {
                     Id = Guid.Parse("d023ae69-32d4-5217-acfc-08dcab4aab7c"),
                     Word = "sky",
                     Translation = "cielo",
                     LearningRate = 0,
                     UserId = Guid.Parse("2298735a-9353-4dc1-b076-08dcab45d85b"),
                     User = users[0]
                 },
                 new Card
                 {
                     Id = Guid.Parse("fd6ec800-6d2b-40f6-9124-a742d167206e"),
                     Word = "night",
                     Translation = "noche",
                     LearningRate = 0,
                     UserId = Guid.Parse("24d6f1c3-325b-465b-6c8c-08dcabd89020"),
                     User = users[1]
                 },
                 new Card
                 {
                     Id = Guid.Parse("b07e0326-6070-4d1a-977c-3e93971f1fcf"),
                     Word = "focus",
                     Translation = "enfoque",
                     LearningRate = 0,
                     UserId = Guid.Parse("24d6f1c3-325b-465b-6c8c-08dcabd89020"),
                     User = users[1]
                 }
            };

            context.DomainUsers.AddRange(users);
            context.Cards.AddRange(cards);

            context.SaveChanges();
        }
    }

}
