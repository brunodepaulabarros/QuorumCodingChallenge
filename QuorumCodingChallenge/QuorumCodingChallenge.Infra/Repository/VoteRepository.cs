using QuorumCodingChallenge.Domain.Entities;
using QuorumCodingChallenge.Domain.Repository;
using QuorumCodingChallenge.Infra.Extensions;

namespace QuorumCodingChallenge.Infra.Repository
{
    public class VoteRepository : IVoteRepository
    {
        private readonly CsvHelperExtension _csvHelperExtension;
        public VoteRepository(CsvHelperExtension csvHelperExtension)
        {
            _csvHelperExtension = csvHelperExtension;
        }
        public List<Vote> GetAll()
        {
            return _csvHelperExtension.ReadCsv<Vote>("..\\QuorumCodingChallenge.Infra\\Data\\votes.csv");
        }
    }
}
