using QuorumCodingChallenge.Domain.DTO;
using QuorumCodingChallenge.Domain.Entities;
using QuorumCodingChallenge.Domain.Repository;
using QuorumCodingChallenge.Infra.Extensions;

namespace QuorumCodingChallenge.Infra.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly CsvHelperExtension _csvHelperExtension;
        public BillRepository(CsvHelperExtension csvHelperExtension)
        {
            _csvHelperExtension = csvHelperExtension;
        }
        public List<Bill> GetAll()
        {
            return _csvHelperExtension.ReadCsv<Bill>("..\\QuorumCodingChallenge.Infra\\Data\\bills.csv");
        }

        public void SaveBill(List<BillDTO> listBillDTO)
        {
            _csvHelperExtension.WriteCsv("bills.csv", listBillDTO);
        }
    }
}
