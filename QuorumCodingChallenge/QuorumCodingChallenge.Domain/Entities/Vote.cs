using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuorumCodingChallenge.Domain.Entities
{
    public class Vote
    {
        public int id { get; set; }
        public int bill_id { get; set; }
    }
}
