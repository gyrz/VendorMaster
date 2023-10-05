using Contracts.Dto.Person;

namespace BusinessLogicService.Services.PersonSvc
{
    public interface IPersonService : IBaseService<PersonSimpleDto, PersonDto>
    {
    }
}
