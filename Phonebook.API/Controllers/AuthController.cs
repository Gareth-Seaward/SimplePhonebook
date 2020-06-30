
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Phonebook.API.Data;
using Phonebook.API.Dtos;
using Phonebook.API.Utils;

namespace Phonebook.API.Controllers
{
  [Route("v1/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _authRepo;
    private readonly IPhonebookRepository _phonebookRepo;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;

    public AuthController(IAuthRepository authRepo,
      IPhonebookRepository phonebookRepo,
      IJwtTokenGenerator tokenGenerator,
      IMapper mapper)
    {
      _authRepo = authRepo;
      _phonebookRepo = phonebookRepo;
      _tokenGenerator = tokenGenerator;
      _mapper = mapper;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {
      userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

      if (await _authRepo.DoesUserExist(userForRegisterDto.Username))
        return BadRequest("username already exists");

      var userToCreate = _mapper.Map<Models.User>(userForRegisterDto);

      var createdUser = await _authRepo.Register(userToCreate, userForRegisterDto.Password);

      var phonebookToCreate = _mapper.Map<Models.Phonebook>(userForRegisterDto);
      phonebookToCreate.User = createdUser;

      await _phonebookRepo.CreatePhonebook(phonebookToCreate);

      return StatusCode(201);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
    {
      var userFormrepo = await _authRepo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

      if (userFormrepo is null) return Unauthorized();

      var jwtToken = _tokenGenerator.GetJwt(userFormrepo);
      var phonebookFromRepo = await _phonebookRepo.GetPhonebookForUser(userFormrepo.Id);
      var userForResponse = _mapper.Map<Models.Phonebook, UserForResponseDto>(phonebookFromRepo);

      return Ok(new {
         token = jwtToken,
         userForResponse
         });
    }
  }
}