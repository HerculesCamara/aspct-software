using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.DTOs.Atividade;
using ASPCTS.DTOs.Crianca;
using ASPCTS.DTOs.Responsavel;
using ASPCTS.DTOs.Psicologo;
using ASPCTS.Models;
using AutoMapper;

namespace ASPCTS.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Pai
            CreateMap<Responsavel, ResponsavelDTO>().ReverseMap();
            CreateMap<Responsavel, ResponsavelCreateDTO>().ReverseMap();
            CreateMap<Responsavel, ResponsavelUpdateDTO>().ReverseMap();

            // Psicólogo
            CreateMap<Psicologo, PsicologoDTO>().ReverseMap();
            CreateMap<Psicologo, PsicologoCreateDTO>().ReverseMap();
            CreateMap<Psicologo, PsicologoUpdateDTO>().ReverseMap();

            // Criança
            CreateMap<Crianca, CriancaDTO>()
                .ForMember(dest => dest.Idade, opt => opt.MapFrom(src =>
                    DateTime.UtcNow.Year - src.DataNascimento.Year)); // idade dinâmica

            CreateMap<CriancaCreateDTO, Crianca>();
            CreateMap<CriancaUpdateDTO, Crianca>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Crianca, CriancaUpdateDTO>();
            CreateMap<Crianca, CriancaDTO>().ForMember(dest => dest.DataNascimento, opt => opt.MapFrom(src => src.DataNascimento.DateTime));

            // Atividade
            CreateMap<Atividade, AtividadeDTO>().ReverseMap();
            CreateMap<Atividade, AtividadeCreateDTO>().ReverseMap();
            CreateMap<Atividade, AtividadeUpdateDTO>().ReverseMap();
        }

    }
}