using QuorumCodingChallenge.Domain.DTO;
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

        public Boolean Result()
        {
            var bills = _billRepository.GetAll();
            var legislators = _personRepository.GetAll();
            var voteResults = _voteResultRepository.GetAll();
            var votes = _voteRepository.GetAll();

            var countLegislatorSupportOpposer = new List<LegislatorsSupportOpposeCountDTO>();

            foreach (var legislator in legislators)
            {
                var supportBills = voteResults.Where((r) => r.legislator_id == legislator.id && r.vote_type == VoteTypeEnumerator.yes).Count();
                var opposeBills = voteResults.Where((r) => r.legislator_id == legislator.id && r.vote_type == VoteTypeEnumerator.no).Count();
                countLegislatorSupportOpposer.Add(new LegislatorsSupportOpposeCountDTO
                {
                    id = legislator.id,
                    name = legislator.name,
                    num_opposed_bills = opposeBills,
                    num_supported_bills = supportBills
                });
            }

            _personRepository.SaveLegislator(countLegislatorSupportOpposer);

            var countBillSupportOppose = new List<BillDTO>();

            foreach (var bill in bills)
            {
                var legislatorsSupportCount = (from b in bills
                                               join v in votes on b.id equals v.bill_id
                                               join vs in voteResults on v.id equals vs.vote_id
                                               join p in legislators on vs.legislator_id equals p.id
                                               where b.id == bill.id && vs.vote_type == VoteTypeEnumerator.yes
                                               select p).Count();

                var legislatorsOpposerCount = (from b in bills
                                               join v in votes on b.id equals v.bill_id
                                               join vs in voteResults on v.id equals vs.vote_id
                                               join p in legislators on vs.legislator_id equals p.id
                                               where b.id == bill.id && vs.vote_type == VoteTypeEnumerator.no
                                               select p).Count();

                countBillSupportOppose.Add(new BillDTO
                {
                    id = bill.id,
                    title = bill.title,
                    supporter_count = legislatorsSupportCount,
                    opposer_count = legislatorsOpposerCount,
                    primary_sponsor = "Unknown"
                });
            }

            _billRepository.SaveBill(countBillSupportOppose);

            return true;
        }


    }
}
