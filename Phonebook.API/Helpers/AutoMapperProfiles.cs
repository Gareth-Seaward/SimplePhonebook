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

      CreateMap<Models.Phonebook, PhonebookForResponseDto>()
      .ForMember(dest => dest.PhonebookName, opt => opt.MapFrom(src => src.Name));

      CreateMap<Models.Entry, EntryForResponseDto>();

      CreateMap<EntryForUpdateDto, Models.Entry>();
    }
  }
}