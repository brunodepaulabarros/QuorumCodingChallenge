using QuorumCodingChallenge.Domain.DTO;
using QuorumCodingChallenge.Domain.Entities;

namespace QuorumCodingChallenge.Domain.Repository
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
        void SaveLegislator(List<LegislatorsSupportOpposeCountDTO> listLegislatorsSupportOpposeCountDTO);
    }
}
