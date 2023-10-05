using BusinessLogicService.Base;
using Contracts.Dto.Email;
using DataAccess.Data;
using Models.Entities;
using System.Text.RegularExpressions;

namespace BusinessLogicService.Services.EmailSvc
{
    public class EmailValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly EmailDto emailDto;
        public EmailValidator(VendorDbContext vendorDbContext, EmailDto emailDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.emailDto = emailDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(emailDto.Address))
                return "Email address is required";

            if (!CheckIfValidEmail(emailDto))
                return "Email format is invalid";

            if (CheckIfAlreadyExists(emailDto))
                return "Email already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(EmailDto emailDto)
        {
            var email = vendorDbContext
                .Emails
                .FirstOrDefault(x => 
                x.Address == emailDto.Address 
                && x.PersonId == emailDto.PersonId 
                && x.VendorId == emailDto.VendorId);

            if (email == null) return false;
            return email.Id != emailDto.Id;
        }

        public bool CheckIfValidEmail(EmailDto emailDto)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return Regex.IsMatch(emailDto.Address, pattern);
        }
    }
}