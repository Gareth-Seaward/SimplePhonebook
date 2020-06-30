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

      CreateMap<Models.Phonebook, UserForResponseDto>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
      .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
      .ForMember(dest => dest.PhonebookId, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.PhonebookName, opt => opt.MapFrom(src => src.Name));
    }
  }
}