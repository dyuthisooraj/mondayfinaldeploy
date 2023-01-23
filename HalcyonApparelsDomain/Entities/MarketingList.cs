using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonApparelsDomain.Entities
{
    public class MarketingList
    {

        public string ContactId { get; set; }


        public string Fname { get; set; } = null!;


        public string Lname { get; set; } = null!;


        public string? Email { get; set; }


        public string Parent_Order_Id__c { get; set; }


        public string date_of_order__c { get; set; } = null!;


        public string Product_Type__c { get; set; } = null!;


        public List<AccessoryDetails> AccessoryDetailsList { get; set; } = null!;



    }
}
