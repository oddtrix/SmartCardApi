using SmartCardApi.Contexts;
using SmartCardApi.Models.User;

namespace Tests.Infrastructure.DataTests
{
    [TestFixture]
    public class AppDomainRepositoryTests
    {
        [Test]
        public void AppDomainRepository_GetAll_ReturnsAllValues()
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var userRepository = new AppDomainRepository(context);
            var users = userRepository.GetAll();

            // Assert
            Assert.That(users, Is.EqualTo(ExpectedUsers).Using(new DomainUserEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCase("2298735a-9353-4dc1-b076-08dcab45d85b")]
        [TestCase("24d6f1c3-325b-465b-6c8c-08dcabd89020")]
        public void AppDomainRepository_GetById_ReturnsSingleValue(string id)
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var userRepository = new AppDomainRepository(context);
            var user = userRepository.GetById(Guid.Parse(id));

            var expected = ExpectedUsers.FirstOrDefault(x => x.Id == Guid.Parse(id));

            // Assert
            Assert.That(user, Is.EqualTo(expected).Using(new DomainUserEqualityComparer()), message: "GetById method works incorrect");
        }

        [TestCase("2298735a-9353-4dc1-b076-08dcab45d85b")]
        [TestCase("24d6f1c3-325b-465b-6c8c-08dcabd89020")]
        public void AppDomainRepository_Delete_DeletesEntity(string id)
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var userRepository = new AppDomainRepository(context);
            userRepository.Delete(Guid.Parse(id));

            var user = userRepository.GetById(Guid.Parse(id));

            // Assert
            Assert.That(user, Is.EqualTo(null).Using(new DomainUserEqualityComparer()), message: "Delete method works incorrect");
        }

        [Test]
        public void AppDomainRepository_Update_UpdatesEntity()
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var userRepository = new AppDomainRepository(context);
            var userToUpdate = ExpectedUsers.First();
            userToUpdate.UserName = "newUserName";
            var user = userRepository.Update(userToUpdate);

            // Assert
            Assert.That(user, Is.EqualTo(userToUpdate).Using(new DomainUserEqualityComparer()), message: "Update method works incorrect");
        }

        [Test]
        public void AppDomainRepository_Create_CreatesEntity()
        {
            // Arrange
            using var context = new AppDomainDbContext(UnitTestHelper.GetUnitTestDbOptions());

            // Act
            var userRepository = new AppDomainRepository(context);
            var newUser = new DomainUser()
            {
                Id = Guid.NewGuid(),
                Email = "newEmail@gmail.com",
                Name = "Test",
                UserName = "Test",
                Surname = "Test"
            };
            var user = userRepository.Create(newUser);
            var expected = userRepository.GetById(user.Id);

            // Assert
            Assert.That(user, Is.EqualTo(expected).Using(new DomainUserEqualityComparer()), message: "Create method works incorrect");
        }
        internal static IEnumerable<DomainUser> ExpectedUsers =>
            new[]
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
    }
}
