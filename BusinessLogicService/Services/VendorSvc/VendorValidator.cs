using BusinessLogicService.Base;
using Contracts.Dto.Vendor;
using DataAccess.Data;

namespace BusinessLogicService.Services.VendorSvc
{
    public class VendorValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly VendorDto vendorDto;
        public VendorValidator(VendorDbContext vendorDbContext, VendorDto vendorDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.vendorDto = vendorDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(vendorDto.Name) || string.IsNullOrEmpty(vendorDto.Name2))
                return "Vendor name is required";

            if (CheckIfAlreadyExists(vendorDto))
                return "Vendor already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(VendorDto vendorDto)
        {
            var vendor = vendorDbContext.Vendors.FirstOrDefault(x => x.Name == vendorDto.Name && x.Name2 == vendorDto.Name2);
            if (vendor == null) return false;
            return vendor.Id != vendorDto.Id;
        }
    }
}