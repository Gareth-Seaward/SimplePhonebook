using AutoMapper;
using Phonebook.API.Dtos;
using Models = Phonebook.API.Models;

namespace Phonebook.API.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<UserForRegisterDto, Models.User>();
      CreateMap<UserForRegisterDto, Models.Phonebook>()
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PhonebookName));
    }
  }
}