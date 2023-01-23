using HalcyonApparelsApplication.DTO;

namespace HalcyonApparelsApplication.Interfaces
{
    public interface ISalesforceCrud
    {
        public bool SalesforcePost(List<CustomerDTO> customerDTO);
    }
}
