using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApplication15.Models;
using WebApplication15.ViewModels;

namespace WebApplication15.Helpers
{
    public class DepositMappingProfile : Profile
    {
        public DepositMappingProfile()
        {
            CreateMap<DepositContract, DepositContractViewModel>()
                .ForMember(vm => vm.BeginDate, src => src.MapFrom(m => m.BeginDate.ToString("dd.MM.yyyy")))
                .ForMember(vm => vm.EndDate, src => src.MapFrom(m => m.EndDate.ToString("dd.MM.yyyy")))
                .ReverseMap()
                .ForMember(m => m.BeginDate, src => src.MapFrom(vm => DateTime.Parse(vm.BeginDate)))
                .ForMember(m => m.EndDate, src => src.Ignore());
        }
    }
}
