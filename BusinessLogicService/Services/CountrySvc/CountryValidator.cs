using BusinessLogicService.Base;
using Contracts.Dto.Country;
using DataAccess.Data;

namespace BusinessLogicService.Services.CountrySvc
{
    public class CountryValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly CountryDto countryDto;
        public CountryValidator(VendorDbContext vendorDbContext, CountryDto countryDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.countryDto = countryDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(countryDto.Name))
                return "Country name is required";

            if (CheckIfAlreadyExists(countryDto))
                return "Country already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(CountryDto countryDto)
        {
            var country = vendorDbContext.Countries.FirstOrDefault(x => x.Name == countryDto.Name);
            if (country == null) return false;
            return country.Id != countryDto.Id;
        }
    }
}