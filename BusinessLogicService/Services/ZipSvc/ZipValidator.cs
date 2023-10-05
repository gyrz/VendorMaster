using BusinessLogicService.Base;
using Contracts.Dto.Zip;
using DataAccess.Data;

namespace BusinessLogicService.Services.ZipSvc
{
    public class ZipValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly ZipSimpleDto zipDto;
        public ZipValidator(VendorDbContext vendorDbContext, ZipSimpleDto zipDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.zipDto = zipDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(zipDto.Code))
                return "Zip code is required";

            if (CheckIfAlreadyExists(zipDto))
                return "Zip already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(ZipSimpleDto zipDto)
        {
            var country = vendorDbContext.ZipCodes.FirstOrDefault(x => x.Code == zipDto.Code && x.CountryId == zipDto.CountryId);
            if (country == null) return false;
            return country.Id != zipDto.Id;
        }
    }
}