using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using phonebook.API.Tests.Fakes;
using Phonebook.API.Data;
using Phonebook.API.Models;
using Phonebook.API.Utils;

namespace Tests
{
  [TestFixture]
  public class AuthRepositoryTests
  {
    private Mock<IPasswordHasher> mockHasher;
    private DataContext fakeContext;

    [SetUp]
    public void Setup()
    {
      mockHasher = new Mock<IPasswordHasher>();
      fakeContext = FakeContextGenerator.Context;
    }

    [Test]
    public async Task ReturnsTrue_For_DoesUserExist_With_ValidUser()
    {

      var repo = new AuthRepository(fakeContext, mockHasher.Object);

      var result = await repo.DoesUserExist("Test1");

      Assert.True(result);
    }

    [Test]
    public async Task ReturnsFalse_For_DoesUserExist_With_InvalidUser()
    {
      var repo = new AuthRepository(fakeContext, mockHasher.Object);

      var result = await repo.DoesUserExist("InvalidTest");

      Assert.False(result);
    }

    [Test]
    public async Task Calls_VerifyPasswordHash_On_PasswordHasherToLogin()
    {
      var repo = new AuthRepository(fakeContext, mockHasher.Object);

      var result = await repo.Login("Test1", "TestPassword");

      mockHasher.Verify(mh => mh.VerifyPasswordHash("TestPassword", It.IsAny<byte[]>(), It.IsAny<byte[]>()));
    }

    [Test]
    public async Task Calls_CreatePasswordHash_On_Register()
    {
      var callbackresult = false;
      var testPassword = "TestPassword";
      byte[] testHash, testSalt;
      mockHasher.Setup(mh => mh.CreatePasswordHash(testPassword, out testHash, out testSalt))
      .Callback(() => { callbackresult = true; });

      var repo = new AuthRepository(fakeContext, mockHasher.Object);
      var testUser = new User { Username = "TestRegister" };

      var result = await repo.Register(testUser, testPassword);

      Assert.IsTrue(callbackresult);
    }
  }


}