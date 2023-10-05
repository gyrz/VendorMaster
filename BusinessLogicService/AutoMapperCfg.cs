using AutoMapper;
using Contracts.Dto.City;
using Contracts.Dto.Country;
using Contracts.Dto.Zip;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService
{
    public class AutoMapperCfg
    {
        public Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Country, CountryDto>();
                cfg.CreateMap<CountryDto, Country>();

                cfg.CreateMap<City, CityDto>();
                cfg.CreateMap<CitySimpleDto, City>();

                cfg.CreateMap<Zip, ZipDto>();
                cfg.CreateMap<ZipSimpleDto, Zip>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
