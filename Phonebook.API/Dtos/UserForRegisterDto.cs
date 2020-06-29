using System.ComponentModel.DataAnnotations;

namespace Phonebook.API.Dtos
{
  public class UserForRegisterDto
  {
    [Required]
    public string Username { get; set; }

    [Required]
    [StringLength(12, MinimumLength = 4, ErrorMessage = "You must specify a password of 4 to 12 characters")]
    public string Password { get; set; }

    [Required]
    [StringLength(24, MinimumLength = 4, ErrorMessage = "You must specify a phonebook name of 4 to 24 characters")]
    public string PhonebookName { get; set; }
  }
}