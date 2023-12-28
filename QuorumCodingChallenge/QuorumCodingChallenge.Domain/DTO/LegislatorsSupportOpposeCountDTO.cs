using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuorumCodingChallenge.Domain.DTO
{
    public class LegislatorsSupportOpposeCountDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public int num_supported_bills { get; set; }
        public int num_opposed_bills { get; set; }
    }
}
