using QuorumCodingChallenge.Domain.DTO;
using QuorumCodingChallenge.Domain.Entities;
using QuorumCodingChallenge.Domain.Repository;
using QuorumCodingChallenge.Infra.Extensions;

namespace QuorumCodingChallenge.Infra.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly CsvHelperExtension _csvHelperExtension;
        public PersonRepository(CsvHelperExtension csvHelperExtension)
        {
            _csvHelperExtension = csvHelperExtension;
        }

        public List<Person> GetAll()
        {
            return _csvHelperExtension.ReadCsv<Person>("..\\QuorumCodingChallenge.Infra\\Data\\legislators.csv");
        }

        public void SaveLegislator(List<LegislatorsSupportOpposeCountDTO> listLegislatorsSupportOpposeCountDTO)
        {
            _csvHelperExtension.WriteCsv("legislators-support-oppose-count.csv", listLegislatorsSupportOpposeCountDTO);
        }
    }
}
