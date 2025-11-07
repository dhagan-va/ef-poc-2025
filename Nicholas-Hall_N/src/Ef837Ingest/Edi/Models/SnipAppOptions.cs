using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Edi.Models
{
    public sealed class SnipAppOptions
    {
        public bool Enabled { get; set; } = true;
        public string Level { get; set; } = "1-4";    
        public bool FailOnError { get; set; } = false; 
    }
}
