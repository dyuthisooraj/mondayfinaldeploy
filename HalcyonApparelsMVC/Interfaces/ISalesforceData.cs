using HalcyonApparelsMVC.Models;

namespace HalcyonApparelsMVC.Interfaces
{
    public interface ISalesforceData
    {
        public List<CustomerDetailsMVC> SalesforceCustomerDetails(string access_token);

        public bool Post(List<CustomerDetailsMVC> custdet);
    }
}
