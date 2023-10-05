using BusinessLogicService.Base;
using Contracts.Dto.Address;
using DataAccess.Data;

namespace BusinessLogicService.Services.AddressSvc
{
    public class AddressValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly AddressSimpleDto addressDto;
        public AddressValidator(VendorDbContext vendorDbContext, AddressSimpleDto addressDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.addressDto = addressDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(addressDto.HouseNumber))
                return "Address HouseNumber is required";

            if (string.IsNullOrEmpty(addressDto.Street))
                return "Address Street is required";

            if (addressDto.VendorId == null)
                return "Address must be connected with a Vendor";

            if (addressDto.CityId == null)
                return "Address must be connected with a City";

            if (addressDto.ZipId == null)
                return "Address must be connected with a Zip";

            if (CheckIfAlreadyExists(addressDto))
                return "Address already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(AddressSimpleDto addressDto)
        {
            var address = vendorDbContext.Addresses.FirstOrDefault(x => 
            x.HouseNumber == addressDto.HouseNumber
            && x.VendorId == addressDto.VendorId
            && x.Street == addressDto.Street
            && x.CityId == addressDto.CityId
            && x.ZipId == addressDto.ZipId
            );
            if (address == null) return false;
            return address.Id != addressDto.Id;
        }
    }
}