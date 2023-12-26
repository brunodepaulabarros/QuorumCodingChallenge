using CsvHelper;
using CsvHelper.Configuration;
using QuorumCodingChallenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuorumCodingChallenge.Application.Services.BillServices
{
    public class BillService : IBillService
    {
        public Boolean Result()
        {
            var legislators = ReadCsv<Person>("legislators.csv");
            var bills = ReadCsv<Bill>("bills.csv");
            var votes = ReadCsv<Vote>("votes.csv");
            var voteResults = ReadCsv<VoteResult>("vote_results.csv");

            // Process legislators' votes
            var legislatorVotes = new Dictionary<int, Tuple<int, int>>();
            foreach (var voteResult in voteResults)
            {
                if (!legislatorVotes.ContainsKey(voteResult.LegislatorId))
                    legislatorVotes[voteResult.LegislatorId] = Tuple.Create(0, 0);

                if (voteResult.VoteType == 1)
                    legislatorVotes[voteResult.LegislatorId] = Tuple.Create(
                        legislatorVotes[voteResult.LegislatorId].Item1 + 1,
                        legislatorVotes[voteResult.LegislatorId].Item2
                    );
                else if (voteResult.VoteType == 2)
                    legislatorVotes[voteResult.LegislatorId] = Tuple.Create(
                        legislatorVotes[voteResult.LegislatorId].Item1,
                        legislatorVotes[voteResult.LegislatorId].Item2 + 1
                    );
            }

            // Process bills and primary sponsor
            var billInfo = new Dictionary<int, Tuple<int, int, string>>();
            foreach (var vote in votes)
            {
                if (!billInfo.ContainsKey(vote.BillId))
                    billInfo[vote.BillId] = Tuple.Create(0, 0, "Unknown");

                billInfo[vote.BillId] = Tuple.Create(
                    billInfo[vote.BillId].Item1 + (legislatorVotes.ContainsKey(vote.BillId) ? legislatorVotes[vote.BillId].Item1 : 0),
                    billInfo[vote.BillId].Item2 + (legislatorVotes.ContainsKey(vote.BillId) ? legislatorVotes[vote.BillId].Item2 : 0),
                    GetPrimarySponsorName(bills, legislators, vote.BillId)
                );
            }

            // Generate Output CSV Files
            WriteCsv("legislators-support-oppose-count.csv", legislatorVotes.Select(kv => new { Id = kv.Key, Name = GetLegislatorName(legislators, kv.Key), NumSupportedBills = kv.Value.Item1, NumOpposedBills = kv.Value.Item2 }));
            WriteCsv("bills.csv", billInfo.Select(kv => new { Id = kv.Key, Title = GetBillTitle(bills, kv.Key), SupporterCount = kv.Value.Item1, OpposerCount = kv.Value.Item2, PrimarySponsor = kv.Value.Item3 }));

            return true;
        }

        static List<T> ReadCsv<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<T>().ToList();
            }
        }

        static void WriteCsv<T>(string filePath, IEnumerable<T> records)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(records);
            }
        }

        static string GetLegislatorName(List<Person> legislators, int legislatorId)
        {
            var legislator = legislators.FirstOrDefault(l => l.Id == legislatorId);
            return legislator != null ? legislator.Name : "Unknown";
        }

        static string GetBillTitle(List<Bill> bills, int billId)
        {
            var bill = bills.FirstOrDefault(b => b.Id == billId);
            return bill != null ? bill.Title : "Unknown";
        }

        static string GetPrimarySponsorName(List<Bill> bills, List<Person> legislators, int billId)
        {
            var bill = bills.FirstOrDefault(b => b.Id == billId);
            return bill != null ? GetLegislatorName(legislators, bill.PrimarySponsor) : "Unknown";
        }
    }
}
