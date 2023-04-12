using INTEX_3_11.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX_3_11.Controllers
{
    public class SearchController : Controller
    {
        INTEXW23Context db = new INTEXW23Context();
        public IActionResult Index(string searching)
        {
            var Ageatdeath = from a in db.Burialmain select a;
            if (!String.IsNullOrEmpty(searching))
            {
                Ageatdeath = Ageatdeath.Where(a => a.Ageatdeath.Contains(searching));
            }
            return View("BurialList", Ageatdeath.ToList());
        }
    }
}
