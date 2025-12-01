using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edi837Ingester.Data.Entities
{
    public class ClaimType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
