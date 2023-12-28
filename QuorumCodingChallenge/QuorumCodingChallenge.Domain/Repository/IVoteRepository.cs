using QuorumCodingChallenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuorumCodingChallenge.Domain.Repository
{
    public interface IVoteRepository
    {
        List<Vote> GetAll();
    }
}
