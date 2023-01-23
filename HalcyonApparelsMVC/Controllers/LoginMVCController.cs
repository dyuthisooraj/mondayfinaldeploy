using Microsoft.AspNetCore.Mvc;
using HalcyonApparelsMVC.Models;
using HalcyonApparelsMVC.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace HalcyonApparelsMVC.Controllers
{
    public class LoginMVCController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ISalesforceData _salesforcedata;
        private readonly IAuthenticate _authenticate;
        public LoginMVCController(IConfiguration config, ISalesforceData salesforcedata, IAuthenticate authenticate)
        {
            _config = config;
            _salesforcedata = salesforcedata;
            _authenticate = authenticate;
        }
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginMVC loginDetails)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_config["WebConfig:UserApi"]);
            var postTask = client.PostAsJsonAsync("api/Login", loginDetails);
            postTask.Wait();
            var Result = postTask.Result;
            if (!Result.IsSuccessStatusCode)
            {
                ViewData["LoginFlag"] = "Invalid Login";
                return View();
            }
            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(loginDetails.UserName)),
                        new Claim(ClaimTypes.Name, loginDetails.UserName),
                        new Claim(ClaimTypes.Role, "admin"),
                        new Claim("FavoriteDrink", "Tea")
                };
               
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
       
            var principal = new ClaimsPrincipal(identity);
       
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true
            }
            );
            //CustomerAdd();
            //return RedirectToAction("Mail", "Marketing");
            return RedirectToAction("AccessoryView", "Home");
        }


        public bool CustomerAdd()
        {
            //var access_token = HttpContext.Session.GetString("Acces_token").ToString();
            var access_token = _authenticate.Authenticate();
            var response = _salesforcedata.SalesforceCustomerDetails(access_token);
            var isTrue = _salesforcedata.Post(response);
            
            //return response;
            return true;
        }

        public ActionResult SalesforceApiCall()
        {
            CustomerAdd();
            return Ok();
        }
         
    }

}
