using Api.Dtos.Dependent;

namespace Api.Repository
{
    public interface IDependentsRepository
    {
        List<DependentDto> GetAllDependents();
    }
}
