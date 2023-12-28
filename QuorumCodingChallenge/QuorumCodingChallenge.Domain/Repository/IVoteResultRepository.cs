using QuorumCodingChallenge.Domain.Entities;

namespace QuorumCodingChallenge.Domain.Repository
{
    public interface IVoteResultRepository
    {
        List<VoteResult> GetAll();
    }
}
