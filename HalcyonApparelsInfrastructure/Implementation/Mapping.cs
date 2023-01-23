using HalcyonApparelsApplication.DTO;
using HalcyonApparelsApplication.Interfaces;
using HalcyonApparelsDomain.Entities;
using HalcyonApparelsInfrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonApparelsInfrastructure.Implementation
{
    public class Mapping : IMapping
    {
        private readonly AppDBContext _dbobj;

        public Mapping(AppDBContext dbobj)
        {
            _dbobj = dbobj;
        }


        public List<CustomerDetails> GetCustomer()
        {

            
            return _dbobj.CustomerDetails.ToList();

        }
        



        public List<AccessoryDetails> GetAccessoryType()
        {
            return _dbobj.AccessoryDetails.ToList();
        }

        public List<ProductType> GetProductType()
        {
            return _dbobj.Products.ToList();
        }

        public bool Addaccsry(string atype, string ptype)
        {
            var prodtype = _dbobj.Products.ToList().Where(c => c.ProdType.Equals(ptype)).ToList().FirstOrDefault();
            if (prodtype == null)
            {
                prodtype.Id = 0;
                prodtype.ProdType = ptype;
            }
            var acclist = prodtype.accessoryTypes;
            if (acclist != null)
            {
                acclist.Add(new AccessoryType { AccsryType = atype });
            }
            else
            {
                acclist = new List<AccessoryType>();
                acclist.Add(new AccessoryType { AccsryType = atype });

            }
            prodtype.accessoryTypes = acclist;
            if (prodtype.Id == 0)
            {
                _dbobj.Products.Add(prodtype);
                _dbobj.SaveChanges();
            }
            else
            {
                _dbobj.Products.Update(prodtype);
                _dbobj.SaveChanges();
            }
            
            return true;
        }
        public List<MarketingList> GetMailingList()
        {
            var cutlist = _dbobj.CustomerDetails.ToList();
            var orderlist= _dbobj.OrderDetails.ToList();
            var protypelist = _dbobj.Products.Include(c => c.accessoryTypes).ToList();
            var acclistitems = _dbobj.AccessoryDetails.ToList();
            var cutomeIdList= new List<string>();
            foreach(var item in cutlist)
            {
                cutomeIdList.Add(item.ContactId);
            }
            List<MarketingList> marketingLists = new List<MarketingList>();
            foreach(var item in cutomeIdList)
            {
                MarketingList marketing = new MarketingList();
                marketing.ContactId = item;
                marketing.Parent_Order_Id__c = orderlist.Where(c => c.Contact__c.Equals(item)).FirstOrDefault().Parent_Order_Id__c;
                marketing.Email = cutlist.Where(c => c.ContactId.Equals(item)).FirstOrDefault().Email;
                marketing.Product_Type__c= orderlist.Where(c => c.Contact__c.Equals(item)).FirstOrDefault().Product_Type__c;
                List<AccessoryDetails> acclist = new List<AccessoryDetails>();
                var productDetails = protypelist.Where(c => c.ProdType.Equals(marketing.Product_Type__c)).FirstOrDefault();
                foreach(var accitem in productDetails.accessoryTypes.ToList())
                {
                    AccessoryDetails accessoryDetails = new AccessoryDetails();
                    accessoryDetails.AccessoryId = acclistitems.Where(c => c.AccessoryType.Equals(accitem.AccsryType)).FirstOrDefault().AccessoryId;
                    accessoryDetails.ImageUrl = acclistitems.Where(c => c.AccessoryType.Equals(accitem.AccsryType)).FirstOrDefault().ImageUrl;
                    accessoryDetails.AccessoryType = acclistitems.Where(c => c.AccessoryType.Equals(accitem.AccsryType)).FirstOrDefault().AccessoryType;
                    acclist.Add(accessoryDetails);
                }
                marketing.AccessoryDetailsList = acclist;
                marketingLists.Add(marketing);
            }
            return marketingLists;
        }
    }
}
