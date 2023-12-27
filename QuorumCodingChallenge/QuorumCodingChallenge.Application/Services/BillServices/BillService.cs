using CsvHelper;
using CsvHelper.Configuration;
using QuorumCodingChallenge.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

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


            foreach (var legislator in legislators)
            {
                var supportBills = voteResults.Where((r) => r.legislator_id == legislator.id && r.vote_type == 1).Count();
                var opposeBills = voteResults.Where((r) => r.legislator_id == legislator.id && r.vote_type == 2).Count();
            }

            // Process legislators' votes
            var legislatorVotes = new Dictionary<int, Tuple<int, int>>();
            foreach (var voteResult in voteResults)
            {
                if (!legislatorVotes.ContainsKey(voteResult.legislator_id))
                    legislatorVotes[voteResult.legislator_id] = Tuple.Create(0, 0);

                if (voteResult.vote_type == 1)
                    legislatorVotes[voteResult.legislator_id] = Tuple.Create(
                        legislatorVotes[voteResult.legislator_id].Item1 + 1,
                        legislatorVotes[voteResult.legislator_id].Item2
                    );
                else if (voteResult.vote_type == 2)
                    legislatorVotes[voteResult.legislator_id] = Tuple.Create(
                        legislatorVotes[voteResult.legislator_id].Item1,
                        legislatorVotes[voteResult.legislator_id].Item2 + 1
                    );
            }

            // Process bills and primary sponsor
            var billInfo = new Dictionary<int, Tuple<int, int, string>>();
            foreach (var vote in votes)
            {
                if (!billInfo.ContainsKey(vote.bill_id))
                    billInfo[vote.bill_id] = Tuple.Create(0, 0, "Unknown");

                billInfo[vote.bill_id] = Tuple.Create(
                    billInfo[vote.bill_id].Item1 + (legislatorVotes.ContainsKey(vote.bill_id) ? legislatorVotes[vote.bill_id].Item1 : 0),
                    billInfo[vote.bill_id].Item2 + (legislatorVotes.ContainsKey(vote.bill_id) ? legislatorVotes[vote.bill_id].Item2 : 0),
                    GetPrimarySponsorName(bills, legislators, vote.bill_id)
                );
            }

            // Generate Output CSV Files
            WriteCsv("legislators-support-oppose-count.csv", legislatorVotes.Select(kv => new { Id = kv.Key, Name = GetLegislatorName(legislators, kv.Key), NumSupportedBills = kv.Value.Item1, NumOpposedBills = kv.Value.Item2 }));
            WriteCsv("bills-result.csv", billInfo.Select(kv => new { Id = kv.Key, Title = GetBillTitle(bills, kv.Key), SupporterCount = kv.Value.Item1, OpposerCount = kv.Value.Item2, PrimarySponsor = kv.Value.Item3 }));

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
            var legislator = legislators.FirstOrDefault(l => l.id == legislatorId);
            return legislator != null ? legislator.name : "Unknown";
        }

        static string GetBillTitle(List<Bill> bills, int billId)
        {
            var bill = bills.FirstOrDefault(b => b.id == billId);
            return bill != null ? bill.title : "Unknown";
        }

        static string GetPrimarySponsorName(List<Bill> bills, List<Person> legislators, int billId)
        {
            var bill = bills.FirstOrDefault(b => b.id == billId);
            return bill != null ? GetLegislatorName(legislators, bill.sponsor_id) : "Unknown";
        }
    }
}
