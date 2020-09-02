using AutoMapper;
using Hajrat2020.Models;
using Hajrat2020.ViewModel;

namespace Hajrat2020.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Donation, DonationViewModel>()
                .ForMember(x => x.TypeOfHelpName, m => m.MapFrom(n => n.TypeOfHelp.Name))
                .ForMember(x => x.Address, m => m.MapFrom(n => n.FamilyInNeed.Address))
                .ForMember(x => x.FamilyInNeedFullname, m => m.MapFrom(n => n.FamilyInNeed.FirstName + " " + n.FamilyInNeed.LastName))
                .ForMember(x => x.CityName, m => m.MapFrom(n => n.FamilyInNeed.City.Name))
                .ForMember(x => x.UserName, m => m.MapFrom(n => n.ApplicationUser.FullName))
                .ForMember(x => x.CurrencyName, m => m.MapFrom(n => n.Currency.Name));
            Mapper.CreateMap<DonationViewModel, Donation>();
            Mapper.CreateMap<UserViewModel, ApplicationUser>()
                .ForMember(x => x.UserName, m => m.MapFrom(n => n.Email))
                .ForMember(x => x.FullName, m => m.MapFrom(n => n.FirstName + " " + n.LastName))
                .ForMember(x => x.Id, m => m.Ignore())
                .ForMember(x => x.Active, m => m.Ignore())
                .ForMember(x => x.RoleName, m => m.Ignore())
                .ForMember(x => x.DateOfAddingAdmin, m => m.Ignore());
            Mapper.CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(x => x.CityName, m => m.MapFrom(n => n.City.Name))
                .ForMember(x => x.GenderName, m => m.MapFrom(n => n.Gender.Name));
            Mapper.CreateMap<FamilyInNeed, FamilyViewModel>()
                .ForMember(x => x.CityName, m => m.MapFrom(n => n.City.Name))
                .ForMember(x => x.AdminName, m => m.MapFrom(n => n.ApplicationUser.FullName));
            Mapper.CreateMap<FamilyViewModel, FamilyInNeed>()
                .ForMember(x => x.DateOfInsert, m => m.Ignore())
                .ForMember(x => x.DateOfLastUpdate, m => m.Ignore());
        }
    }

}