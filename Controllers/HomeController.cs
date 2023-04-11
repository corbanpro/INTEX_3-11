using INTEX_3_11.Models;
using INTEX_3_11.Models.ViewModels;
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

        public IActionResult BurialList(string burialFilter, int pageNum = 1)
        {
            int pageSize = 40;
            PageModel PageModel = new PageModel
            {
                BurialList = context.Burialmain
                .OrderBy(x => x.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(), 

                PageInfo = new PageInfo()
                {
                    TotalNumBurials = 
                        (burialFilter == null
                            ? context.Burialmain.Count()
                            : context.Burialmain.Count()),
                    BurialsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };

            return View(PageModel);
        }

        public IActionResult BurialView(long id)
        {
            Burialmain burial = context.Burialmain.Where(x => x.Id == id).FirstOrDefault();
            return View(burial);
        }

        [Authorize]
        public IActionResult AddData()
        {
            return View();
        }

    }
}
