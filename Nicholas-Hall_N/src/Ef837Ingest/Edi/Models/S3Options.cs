using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef837Ingest.Edi.Models
{
    public class S3Options
    {
        public string Bucket { get; set; } = "";
        public string InboundPrefix { get; set; } = "incoming/";
        public string ArchivePrefix { get; set; } = "archive/";
        public int PollSeconds { get; set; } = 5;
    }
}
