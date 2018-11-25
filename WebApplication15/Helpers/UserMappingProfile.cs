using System;
using AutoMapper;
using WebApplication15.Models;
using WebApplication15.ViewModels;

namespace WebApplication15.Helpers
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.BirthDate, src => src.MapFrom(m => m.BirthDate.ToString("dd.MM.yyyy")))
                .ForMember(vm => vm.IssueDate, src => src.MapFrom(m => m.IssueDate.ToString("dd.MM.yyyy")))
                .ForMember(vm => vm.MonthlyIncome, src => src.MapFrom(m => m.MonthlyIncome == null ? "" : m.MonthlyIncome))
                .ReverseMap()
                .ForMember(m => m.MonthlyIncome, src => src.MapFrom(vm => vm.MonthlyIncome == null ? "" : vm.MonthlyIncome))
                .ForMember(m => m.BirthDate, src => src.MapFrom(vm => DateTime.Parse(vm.BirthDate)))
                .ForMember(m => m.IssueDate, src => src.MapFrom(vm => DateTime.Parse(vm.IssueDate)));
        }
    }
}
