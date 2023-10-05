using Contracts.Dto.Person;

namespace BusinessLogicService.Services.PersonSvc
{
    public interface IPersonService : IBase<PersonSimpleDto, PersonDto>
    {
    }
}
