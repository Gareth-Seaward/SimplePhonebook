using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Phonebook.API.Controllers;
using Phonebook.API.Data;
using Phonebook.API.Dtos;
using Phonebook.API.Utils;
using Models = Phonebook.API.Models;

namespace phonebook.API.Tests.Controller
{
  [TestFixture]
  public class AuthControllerTests
  {
    private Mock<IAuthRepository> mockAuthRepo;
    private Mock<IPhonebookRepository> mockPhonebookRepo;
    private Mock<IJwtTokenGenerator> mockTokenGen;
    private Mock<IMapper> mockMapper;

    [SetUp]
    public void SetUp()
    {
      mockAuthRepo = new Mock<IAuthRepository>();
      mockPhonebookRepo = new Mock<IPhonebookRepository>();
      mockTokenGen = new Mock<IJwtTokenGenerator>();
      mockMapper = new Mock<IMapper>();
    }

    [Test]
    public async Task RegistersNewUser()
    {
      var userForRegister = new UserForRegisterDto
      { Username = "Username", Password = "password", PhonebookName = "phonebook" };
      var testUser = new Models.User { Username = "username" };
      var createdUser = testUser;
      var testPhonebook = new Models.Phonebook { Name = "phonebook name" };
      var controller = new AuthController(mockAuthRepo.Object,
        mockPhonebookRepo.Object, mockTokenGen.Object, mockMapper.Object);

      mockAuthRepo.Setup(mar => mar.DoesUserExist("username")).Returns(Task.FromResult(false));
      mockMapper.Setup(mm => mm.Map<Models.User>(userForRegister)).Returns(testUser);
      mockAuthRepo.Setup(mar => mar.Register(testUser, "password")).Returns(Task.FromResult(createdUser));
      mockMapper.Setup(mm => mm.Map<Models.Phonebook>(userForRegister)).Returns(testPhonebook);

      var result = await controller.Register(userForRegister);

      mockPhonebookRepo.Verify(mpr => mpr.CreatePhonebook(testPhonebook));
      Assert.That(result, Is.InstanceOf<StatusCodeResult>());
    }

    [Test]
    public async Task ReturnsBadRequest_For_USerAlreadyExisting()
    {
      var controller = new AuthController(mockAuthRepo.Object,
        mockPhonebookRepo.Object, mockTokenGen.Object, mockMapper.Object);

      var userForRegister = new UserForRegisterDto
      { Username = "Username", Password = "password", PhonebookName = "phonebook" };

      mockAuthRepo.Setup(mar => mar.DoesUserExist("username")).Returns(Task.FromResult(true));

      var result = await controller.Register(userForRegister);

      Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CallsGetJwt_For_Token_OnLogin()
    {
      var userForLogin = new UserForLoginDto { Username = "username", Password = "password" };
      var testUser = new Models.User { Id = 1, Username = "username" };
      var testPhonebook = new Models.Phonebook { Name = "phonebookName" };
      var testResponse = new UserForResponseDto { Username = "username", PhonebookName = "phonebookName" };

      var controller = new AuthController(mockAuthRepo.Object,
        mockPhonebookRepo.Object, mockTokenGen.Object, mockMapper.Object);

      mockAuthRepo.Setup(mar => mar.Login("username", "password")).Returns(Task.FromResult(testUser));
      mockTokenGen.Setup(mt => mt.GetJwt(testUser)).Returns("TestToken");
      mockPhonebookRepo.Setup(mpr => mpr.GetPhonebookForUser(1)).Returns(Task.FromResult(testPhonebook));
      mockMapper.Setup(mm => mm.Map<Models.Phonebook, UserForResponseDto>(testPhonebook)).Returns(testResponse);

      var result = await controller.Login(userForLogin);

      mockTokenGen.Verify(mt => mt.GetJwt(testUser));
      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task ReturnsUnauthorised_For_FailedToLogin()
    {
      var userForLogin = new UserForLoginDto { Username = "username", Password = "password" };
      var controller = new AuthController(mockAuthRepo.Object,
        mockPhonebookRepo.Object, mockTokenGen.Object, mockMapper.Object);

      var result = await controller.Login(userForLogin);

      Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
    }
  }
}