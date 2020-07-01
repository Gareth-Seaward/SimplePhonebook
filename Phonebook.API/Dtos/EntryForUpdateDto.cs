using System.ComponentModel.DataAnnotations;

namespace Phonebook.API.Dtos
{
  public class EntryForUpdateDto
  {
    [Required]
    [StringLength(24, MinimumLength = 3, ErrorMessage = "You must specify an entry name of 3 to 24 characters")]
    public string Name { get; set; }

    [Required]
    [RegularExpression("^(\\+27|27|0)[ ]*[0-9]{2}[ ]*[0-9]{3}[ ]*[0-9]{4}$", ErrorMessage = "Please enter valid phone no.")]
    public string PhoneNumber { get; set; }
  }
}