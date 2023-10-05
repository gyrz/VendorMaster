using BusinessLogicService.Base;
using Contracts.Dto.Person;
using DataAccess.Data;
using Models.Entities;
using System.Text.RegularExpressions;

namespace BusinessLogicService.Services.PersonSvc
{
    public class PersonValidator : Validator
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly PersonSimpleDto personDto;
        public PersonValidator(VendorDbContext vendorDbContext, PersonSimpleDto personDto)
        {
            this.vendorDbContext = vendorDbContext;
            this.personDto = personDto;
        }

        public override string Handle(object request = null)
        {
            if (string.IsNullOrEmpty(personDto.FirstName) || string.IsNullOrEmpty(personDto.LastName))
                return "Person name is required";

            if (CheckIfAlreadyExists(personDto))
                return "Person number already exists";

            return base.Handle(request);
        }

        public bool CheckIfAlreadyExists(PersonSimpleDto personDto)
        {
            var person = vendorDbContext
                .Persons
                .FirstOrDefault(x => 
                x.FirstName == personDto.FirstName
                && x.LastName == personDto.LastName
                && x.VendorId == personDto.VendorId);

            if (person == null) return false;
            return person.Id != personDto.Id;
        }
    }
}