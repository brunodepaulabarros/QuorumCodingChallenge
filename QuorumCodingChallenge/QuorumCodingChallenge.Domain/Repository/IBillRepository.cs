using QuorumCodingChallenge.Domain.DTO;
using QuorumCodingChallenge.Domain.Entities;

namespace QuorumCodingChallenge.Domain.Repository
{
    public interface IBillRepository
    {
        List<Bill> GetAll();

        void SaveBill(List<BillDTO> listBillDTO);
   }
}
