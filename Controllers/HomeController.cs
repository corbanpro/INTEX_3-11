using INTEX_3_11.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX_3_11.Controllers
{
    public class HomeController : Controller
    {
        private INTEXW23Context context { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            context = new INTEXW23Context();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult BurialList()
        {
            List<Burialmain> Burials = new List<Burialmain>();
            Burials = context.Burialmain.ToList();
            return View(Burials);
        }

        public IActionResult BurialView()
        {
            
            return View();
        }

        [Authorize]
        public IActionResult AddData()
        {
            return View();
        }

    }
}
