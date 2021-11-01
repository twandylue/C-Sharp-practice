using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var code = Request.Query["code"];
            if (!String.IsNullOrEmpty(code))
            {
                Console.WriteLine("code: " + code);

                // get token
                var token = Utility.GetTokenFromCode(
                    code,
                    "536062935773-e1hvscne4ead0kk62fho999kc179rhhj.apps.googleusercontent.com",
                    "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",
                    "https://localhost:5001/"
                    );
                Console.WriteLine(JsonConvert.SerializeObject(token, Formatting.Indented));

                var userInfo = Utility.GetUserInfo(token.access_token);
                var DisplayJSON = Newtonsoft.Json.JsonConvert.SerializeObject(userInfo, Formatting.Indented); // display
                Console.WriteLine(DisplayJSON);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
