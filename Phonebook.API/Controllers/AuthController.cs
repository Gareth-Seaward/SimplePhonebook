using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phonebook.API.Data;
using Phonebook.API.Dtos;
using Phonebook.API.Models;
using Phonebook.API.Utils;

namespace Phonebook.API.Controllers
{
  [Route("v1/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _authRepo;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthController(IAuthRepository authRepo, IJwtTokenGenerator tokenGenerator)
    {
      _authRepo = authRepo;
      _tokenGenerator = tokenGenerator;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {
      userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

      if (await _authRepo.DoesUserExist(userForRegisterDto.Username))
        return BadRequest("username already exists");

      var userToCreate = new User
      {
        Username = userForRegisterDto.Username
      };

      var createdUser = await _authRepo.Register(userToCreate, userForRegisterDto.Password);

      return StatusCode(201);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
    {
      var userFormrepo = await _authRepo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

      if (userFormrepo is null) return Unauthorized();

      var jwtToken = _tokenGenerator.GetJwt(userFormrepo);

      return Ok(new { token = jwtToken });
    }
  }
}