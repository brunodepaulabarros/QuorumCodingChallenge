using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuorumCodingChallenge.Domain.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public int BillId { get; set; }
    }
}
