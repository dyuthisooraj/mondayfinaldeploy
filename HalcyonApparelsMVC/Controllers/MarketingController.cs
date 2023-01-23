using FluentEmail.Core;
using HalcyonApparelsMVC.DTO;
using HalcyonApparelsMVC.Interfaces;
using HalcyonApparelsMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HalcyonApparelsMVC.Controllers
{
    [Authorize]
    public class MarketingController : Controller
    {

        private readonly IMailSender _mailSender;
        private readonly IConfiguration _config;
      
        public MarketingController(IMailSender mailSender, IConfiguration config, ISalesforceData salesforcedata)
        {
            _mailSender = mailSender;
            _config = config;
            
        }


        [HttpGet("CustomerView")]
        public List<GetCustomer> CustomerView()
        {
            HttpClientHandler clienthandler = new HttpClientHandler();
            clienthandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslpolicyerrors) => { return true; };

            HttpClient client = new HttpClient(clienthandler);
            client.BaseAddress = new Uri(_config["WebConfig:UserApi"]);
            List<GetCustomer>? customer = new List<GetCustomer>();

            HttpResponseMessage res = client.GetAsync("api/Mapping/GetCustomer").Result;
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                customer = JsonConvert.DeserializeObject<List<GetCustomer>>(result);
            }
            return customer;

        }



        [HttpGet]
        public IActionResult MarketingView()
        {
            
            HttpClientHandler clienthandler = new HttpClientHandler();
            clienthandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslpolicyerrors) => { return true; };

            HttpClient client = new HttpClient(clienthandler);
            client.BaseAddress = new Uri(_config["WebConfig:UserApi"]);
            List<AccessoryDetailsMVC>? acctype = new List<AccessoryDetailsMVC>();
            List<ProductType>? ptype = new List<ProductType>();

            HttpResponseMessage res = client.GetAsync("api/Mapping/GetAccessoryType").Result;

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                acctype = JsonConvert.DeserializeObject<List<AccessoryDetailsMVC>>(result);
            }

            HttpResponseMessage resultp = client.GetAsync("api/Mapping/GetProductType").Result;
            if (res.IsSuccessStatusCode)
            {
                var result = resultp.Content.ReadAsStringAsync().Result;
                ptype = JsonConvert.DeserializeObject<List<ProductType>>(result);
            }
            ViewBag.acclist = acctype.Select(c => c.AccessoryType).ToList().Distinct();
            ViewBag.ptypelist = ptype.Select(c => c.ProdType).ToList();
            
            return View();
           

        }



        [HttpPost]
        public async Task<IActionResult> MarketingView(MapDTO map)
        {
           
            HttpClientHandler clienthandler = new HttpClientHandler();
            clienthandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslpolicyerrors) => { return true; };
            HttpClient client = new HttpClient(clienthandler);
            client.BaseAddress = new Uri(_config["WebConfig:UserApi"]);

            var postTask = client.PostAsJsonAsync<MapDTO>("api/Mapping/PostMarketing", map);

            postTask.Wait();
            var Result = postTask.Result;
            if (Result.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = " Mapping updated ";
                return RedirectToAction("AccessoryView", "Home");

            }

            return View();
        }


    

        public async Task<IActionResult> Mail([FromServices] IFluentEmail mailer)
        {
            HttpClientHandler clienthandler = new HttpClientHandler();
            clienthandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslpolicyerrors) => { return true; };

            HttpClient client = new HttpClient(clienthandler);
            client.BaseAddress = new Uri(_config["WebConfig:UserApi"]);
            List<GetCustomer>? customer = new List<GetCustomer>();

            HttpResponseMessage res = client.GetAsync("api/Mapping/GetCustomer").Result;

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                customer = JsonConvert.DeserializeObject<List<GetCustomer>>(result);
            }
            List<MarketingList>? mailtype = new List<MarketingList>();
            HttpResponseMessage resmail = client.GetAsync("api/Mapping/GetMailingList").Result;

            if (resmail.IsSuccessStatusCode)
            {
                var result = resmail.Content.ReadAsStringAsync().Result;
                mailtype = JsonConvert.DeserializeObject<List<MarketingList>>(result);
            }
  
            _mailSender.SendBulkMail(mailtype);
            return RedirectToAction("AccessoryView", "Home");
            
        }


    }
}
