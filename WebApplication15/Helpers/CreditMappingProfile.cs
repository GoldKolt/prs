using AutoMapper;
using System;
using WebApplication15.Models;
using WebApplication15.ViewModels;

namespace WebApplication15.Helpers
{
    public class CreditMappingProfile : Profile
    {
        public CreditMappingProfile()
        {
            CreateMap<CreditContract, CreditContractViewModel>()
                .ForMember(vm => vm.BeginDate, src => src.MapFrom(m => m.BeginDate.ToString("dd.MM.yyyy")))
                .ForMember(vm => vm.EndDate, src => src.MapFrom(m => m.EndDate.ToString("dd.MM.yyyy")))
                // .ForMember(vm => vm.CreditAmount, src => src.MapFrom(m => m.CreditAmount.ToString("F")))
                .ReverseMap()
                // .ForMember(m => m.CreditAmount, src => src.MapFrom(vm => decimal.Parse(vm.CreditAmount)))
                .ForMember(m => m.BeginDate, src => src.MapFrom(vm => DateTime.Parse(vm.BeginDate)))
                .ForMember(m => m.EndDate, src => src.Ignore());
        }
    }
}
