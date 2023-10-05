using BusinessLogicService.Base;
using Contracts.Dto.Phone;
using DataAccess.Data;
using Models.Entities;
using System.Text.RegularExpressions;

namespace BusinessLogicService.Services.PhoneSvc
{
    public class PhoneValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly PhoneDto phoneDto;
        public PhoneValidator(VendorDbContext vendorDbContext, PhoneDto phoneDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.phoneDto = phoneDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(phoneDto.PhoneNumber))
                return "Phone number is required";

            if (!CheckIfValidPhone(phoneDto))
                return "Phone number format is invalid";

            if (CheckIfAlreadyExists(phoneDto))
                return "Phone number already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(PhoneDto phoneDto)
        {
            var phone = vendorDbContext
                .Phones
                .FirstOrDefault(x => 
                x.PhoneNumber == phoneDto.PhoneNumber
                && x.PersonId == phoneDto.PersonId 
                && x.VendorId == phoneDto.VendorId);

            if (phone == null) return false;
            return phone.Id != phoneDto.Id;
        }

        public bool CheckIfValidPhone(PhoneDto phoneDto)
        {
            string pattern = @"^\+\d{1,4}\d{1,14}$";
            return Regex.IsMatch(phoneDto.PhoneNumber, pattern);
        }
    }
}