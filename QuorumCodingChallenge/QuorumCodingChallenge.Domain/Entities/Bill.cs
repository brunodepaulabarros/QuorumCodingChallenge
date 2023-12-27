using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuorumCodingChallenge.Domain.Entities
{
    public class Bill
    {
        public int id { get; set; }
        public string title { get; set; }
        public int sponsor_id { get; set; }
    }
}
