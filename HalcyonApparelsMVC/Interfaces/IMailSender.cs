using HalcyonApparelsMVC.Models;

namespace HalcyonApparelsMVC.Interfaces
{
    public interface IMailSender
    {
 
        void SendBulkMail(List<MarketingList> marketingList);

    }
}

