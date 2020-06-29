using System.ComponentModel.DataAnnotations;

namespace Phonebook.API.Dtos
{
  public class PhonebookForUpdateDto
  {
    [Required]
    [StringLength(24, MinimumLength = 4, ErrorMessage = "You must specify a phonebook name of 4 to 24 characters")]
    public string Name { get; set; }
  }
}