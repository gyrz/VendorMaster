using BusinessLogicService.Base;
using Contracts.Dto.City;
using DataAccess.Data;

namespace BusinessLogicService.Services.CitySvc
{
    public class CityValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly CitySimpleDto cityDto;
        public CityValidator(VendorDbContext vendorDbContext, CitySimpleDto cityDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.cityDto = cityDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(cityDto.Name))
                return "City name is required";

            if (CheckIfAlreadyExists(cityDto))
                return "City already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(CitySimpleDto cityDto)
        {
            var city = vendorDbContext.Cities.FirstOrDefault(x => x.Name == cityDto.Name && x.CountryId == cityDto.CountryId);
            if (city == null) return false;
            return city.Id != cityDto.Id;
        }
    }
}