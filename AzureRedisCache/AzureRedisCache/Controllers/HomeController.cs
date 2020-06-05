using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AzureRedisCache.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace AzureRedisCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IDistributedCache cache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            cache = distributedCache;
        }

        public IActionResult Index()
        {
            if(string.IsNullOrEmpty( cache.GetString("user")))
            {
                cache.SetString("user", "Sonu");
                ViewBag.Message = "Data stored in Cache, Key=user, value=Sonu";
            }
            ViewBag.Message = "Data stored in Cache, Key=user, value=Sonu . Goto Privacy page to view the cached value";
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.Name = cache.GetString("user");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
