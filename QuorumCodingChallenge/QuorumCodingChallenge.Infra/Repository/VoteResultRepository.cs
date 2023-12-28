using QuorumCodingChallenge.Domain.Entities;
using QuorumCodingChallenge.Domain.Repository;
using QuorumCodingChallenge.Infra.Extensions;

namespace QuorumCodingChallenge.Infra.Repository
{
    public class VoteResultRepository : IVoteResultRepository
    {
        private readonly CsvHelperExtension _csvHelperExtension;
        public VoteResultRepository(CsvHelperExtension csvHelperExtension)
        {
            _csvHelperExtension = csvHelperExtension;
        }
        public List<VoteResult> GetAll()
        {
            return _csvHelperExtension.ReadCsv<VoteResult>("..\\QuorumCodingChallenge.Infra\\Data\\vote_results.csv");
        }
    }
}
