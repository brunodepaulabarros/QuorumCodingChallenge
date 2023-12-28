using QuorumCodingChallenge.Domain.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuorumCodingChallenge.Domain.Entities
{
    public class VoteResult
    {
        public int id { get; set; }
        public int legislator_id { get; set; }
        public int vote_id { get; set; }
        public VoteTypeEnumerator vote_type { get; set; }
    }
}
