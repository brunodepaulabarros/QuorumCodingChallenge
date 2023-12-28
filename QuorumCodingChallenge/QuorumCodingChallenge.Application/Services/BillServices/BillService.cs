using QuorumCodingChallenge.Domain.DTO;
using QuorumCodingChallenge.Domain.Entities;
using QuorumCodingChallenge.Domain.Enumerator;
using QuorumCodingChallenge.Domain.Repository;

namespace QuorumCodingChallenge.Application.Services.BillServices
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IVoteResultRepository _voteResultRepository;

        public BillService(IBillRepository billRepository,
            IPersonRepository personRepository,
            IVoteRepository voteRepository,
            IVoteResultRepository voteResultRepository)
        {
            _billRepository = billRepository;
            _personRepository = personRepository;
            _voteRepository = voteRepository;
            _voteResultRepository = voteResultRepository;
        }

        public bool Result()
        {
            var bills = _billRepository.GetAll();
            var legislators = _personRepository.GetAll();
            var voteResults = _voteResultRepository.GetAll();
            var votes = _voteRepository.GetAll();

            ProcessLegislators(legislators, voteResults);

            ProcessBillsAsync(bills, votes, voteResults, legislators);

            return true;
        }

        private void ProcessLegislators(IEnumerable<Person> legislators, IEnumerable<VoteResult> voteResults)
        {
            var countLegislatorSupportOpposer = legislators.Select(legislator => new LegislatorsSupportOpposeCountDTO
            {
                id = legislator.id,
                name = legislator.name,
                num_opposed_bills = voteResults.Count(r => r.legislator_id == legislator.id && r.vote_type == VoteTypeEnumerator.no),
                num_supported_bills = voteResults.Count(r => r.legislator_id == legislator.id && r.vote_type == VoteTypeEnumerator.yes)
            }).ToList();

            _personRepository.SaveLegislator(countLegislatorSupportOpposer);
        }

        private void ProcessBillsAsync(IEnumerable<Bill> bills, IEnumerable<Vote> votes, IEnumerable<VoteResult> voteResults, IEnumerable<Person> legislators)
        {
            var countBillSupportOppose = bills.Select(bill => new BillDTO
            {
                id = bill.id,
                title = bill.title,
                supporter_count = (from v in votes
                                   join vs in voteResults on v.id equals vs.vote_id
                                   join p in legislators on vs.legislator_id equals p.id
                                   where v.bill_id == bill.id && vs.vote_type == VoteTypeEnumerator.yes
                                   select p).Count(),
                opposer_count = (from v in votes
                                 join vs in voteResults on v.id equals vs.vote_id
                                 join p in legislators on vs.legislator_id equals p.id
                                 where v.bill_id == bill.id && vs.vote_type == VoteTypeEnumerator.no
                                 select p).Count(),
                primary_sponsor = "Unknown"
            }).ToList();

            _billRepository.SaveBill(countBillSupportOppose);
        }
    }
}
