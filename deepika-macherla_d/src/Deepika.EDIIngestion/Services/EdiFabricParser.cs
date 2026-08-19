using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.Hipaa5010;
using Deepika.EDIIngestion.Models;
using System.IO;

namespace Deepika.EDIIngestion.Services
{
    // Uses EdiFabric to read TS837P transactions and maps key segments to our entities.
    public class EdiFabricParser : IEdiFabricParser
    {
        public EdiFabricParser()
        {
        }

        public IEnumerable<EdiInterchange>? ParseFile(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                // Read full text and split into segments
                var text = File.ReadAllText(path);
                var segments = text.Split('~', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                   .Select(s => s.Trim())
                                   .ToList();

                // Capture ISA and GS (envelope level) if present
                var isaSeg = segments.FirstOrDefault(s => s.StartsWith("ISA", StringComparison.OrdinalIgnoreCase));
                var gsSeg = segments.FirstOrDefault(s => s.StartsWith("GS", StringComparison.OrdinalIgnoreCase));

                // Group transaction segments by ST (start of transaction)
                var transactions = new List<List<string>>();
                List<string>? current = null;
                foreach (var seg in segments)
                {
                    if (seg.StartsWith("ST", StringComparison.OrdinalIgnoreCase))
                    {
                        if (current != null) transactions.Add(current);
                        current = new List<string> { seg };
                        continue;
                    }
                    if (current != null)
                    {
                        current.Add(seg);
                    }
                }
                if (current != null) transactions.Add(current);

                var results = new List<EdiInterchange>();

                // If no ST segments found, try to parse entire file as a single interchange (backwards compatible)
                if (!transactions.Any())
                {
                    transactions.Add(segments);
                }

                foreach (var txSegs in transactions)
                {
                    var interchange = new EdiInterchange();
                    Provider? provider = null;
                    var claims = new List<Claim>();

                    // Apply envelope info
                    if (!string.IsNullOrWhiteSpace(isaSeg))
                    {
                        interchange.RawIsa = isaSeg;
                        var parts = isaSeg.Split('*', StringSplitOptions.None).Select(p => p.Trim()).ToArray();
                        if (parts.Length > 6) interchange.SenderId = parts[6];
                        if (parts.Length > 8) interchange.ReceiverId = parts[8];
                        if (parts.Length > 13) interchange.IsaControlNumber = parts[13];
                    }
                    if (!string.IsNullOrWhiteSpace(gsSeg))
                    {
                        interchange.RawGs = gsSeg;
                        var parts = gsSeg.Split('*', StringSplitOptions.None).Select(p => p.Trim()).ToArray();
                        if (parts.Length > 6) interchange.GsControlNumber = parts[6];
                    }

                    foreach (var seg in txSegs)
                    {
                        var parts = seg.Split('*', StringSplitOptions.None).Select(p => p.Trim()).ToArray();
                        if (parts.Length == 0) continue;
                        switch (parts[0].ToUpperInvariant())
                        {
                            case "ST":
                                interchange.RawSt = seg;
                                if (parts.Length > 1) interchange.TransactionSetId = parts[1];
                                if (parts.Length > 2) interchange.TransactionSetControlNumber = parts[2];
                                break;
                            case "BHT":
                                interchange.RawBht = seg;
                                if (parts.Length > 3) interchange.BhtReference = parts[3];
                                break;
                            case "NM1":
                                if (parts.Length > 1 && parts[1] == "85")
                                {
                                    provider = new Provider
                                    {
                                        ProviderType = parts.Length > 1 ? parts[1] : null,
                                        Name = parts.Length > 3 ? parts[3] : null,
                                        Identifier = parts.Length > 9 ? parts[9] : null
                                    };
                                }
                                break;
                            case "CLM":
                                var claim = new Claim();
                                if (parts.Length > 1) claim.ClaimNumber = parts[1];
                                if (parts.Length > 2 && decimal.TryParse(parts[2], out var amt)) claim.TotalChargeAmount = amt;
                                claims.Add(claim);
                                break;
                        }
                    }

                    if (provider != null) interchange.Provider = provider;
                    if (claims.Count > 0)
                    {
                        interchange.Claims = claims;
                        foreach (var c in claims) c.Provider = provider;
                    }

                    results.Add(interchange);
                }

                return results;
            }
            catch
            {
                return null;
            }
        }
    }
}
