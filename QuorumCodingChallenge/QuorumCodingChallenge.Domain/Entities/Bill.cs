using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuorumCodingChallenge.Domain.Entities
{
    public class Bill
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PrimarySponsor { get; set; }
    }
}
