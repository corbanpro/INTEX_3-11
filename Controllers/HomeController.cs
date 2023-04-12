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

        public IActionResult BurialList(string ageAtDeath, string sex, string depth, string Headdirection, string haircolor, int pageNum = 1)
        {
            ViewBag.AgeAtDeath = ageAtDeath;
            ViewBag.Sex = sex;
            ViewBag.BurialDepth = depth;
            ViewBag.Headdirection = Headdirection;
            ViewBag.haircolor = haircolor;
            int pageSize = 40;
           

            var PageInfo = new PageInfo()
            {
                TotalNumBurials = context.Burialmain.Count(),
                BurialsPerPage = pageSize,
                CurrentPage = pageNum
                
            };
           
            PageModel PageModel = new PageModel
            {

                BurialList = context.Burialmain
                .OrderBy(x => x.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(), 
                
                PageInfo = PageInfo
                       
               
            };

            if (!String.IsNullOrEmpty(ageAtDeath))
            {
                PageModel.BurialList = PageModel.BurialList.Where(x => x.Ageatdeath == ageAtDeath).ToList();
            }
            if (!String.IsNullOrEmpty(sex))
            {
                PageModel.BurialList = PageModel.BurialList.Where(x => x.Sex == sex).ToList();
            }
            if (!String.IsNullOrEmpty(Headdirection))
            {
                PageModel.BurialList = PageModel.BurialList.Where(x => x.Headdirection == Headdirection).ToList();
            }
            if (!String.IsNullOrEmpty(depth))
            {
                PageModel.BurialList = PageModel.BurialList.Where(x => x.Depth == depth).ToList();
            }
            if (!String.IsNullOrEmpty(haircolor))
            {
                PageModel.BurialList = PageModel.BurialList.Where(x => x.Haircolor == haircolor).ToList();
            }



            return View(PageModel);
        }

        public IActionResult Search(string searching)
        {
            var Ageatdeath = from a in context.Burialmain select a;
            if (!String.IsNullOrEmpty(searching))
            {
                Ageatdeath = Ageatdeath.Where(a => a.Ageatdeath.Contains(searching));
            }
            return View("BurialList", Ageatdeath.ToList());
        }

        public IActionResult BurialView(long id)
        {
            FullBurial FullBurial = new FullBurial();

            FullBurial.Burialmain = context.Burialmain.Where(x => x.Id == id).FirstOrDefault();

            try {
                FullBurial.BurialmainTextile = context.BurialmainTextile.Where(x => x.MainBurialmainid == id).FirstOrDefault();
                FullBurial.Textile = context.Textile.Where(x => x.Textileid == FullBurial.BurialmainTextile.MainTextileid).FirstOrDefault();
            }catch 
            {
                FullBurial.BurialmainTextile = new BurialmainTextile(); 
                FullBurial.Textile = new Textile(); 
            }

            try
            {
                FullBurial.TextilefunctionTextile = context.TextilefunctionTextile.Where(x => x.MainTextileid == FullBurial.Textile.Id).FirstOrDefault();
                FullBurial.Textilefunction = context.Textilefunction.Where(x => x.Id == FullBurial.TextilefunctionTextile.MainTextilefunctionid).FirstOrDefault();
            }catch 
            {
                FullBurial.TextilefunctionTextile = new TextilefunctionTextile();
                FullBurial.Textilefunction = new Textilefunction();
            }

            try
            {
                FullBurial.ColorTextile = context.ColorTextile.Where(x => x.MainColorid == FullBurial.Textile.Id).FirstOrDefault();
                FullBurial.Color = context.Color.Where(x => x.Colorid == FullBurial.ColorTextile.MainColorid).FirstOrDefault();
            }catch 
            {
                FullBurial.ColorTextile = new ColorTextile();
                FullBurial.Color = new Color();
            }

            try
            {
                FullBurial.StructureTextile = context.StructureTextile.Where(x => x.MainTextileid == FullBurial.Textile.Id).FirstOrDefault();
                FullBurial.Structure = context.Structure.Where(x => x.Structureid == FullBurial.StructureTextile.MainStructureid).FirstOrDefault();
            }catch 
            {
                FullBurial.StructureTextile = new StructureTextile();
                FullBurial.Structure = new Structure(); 
            }

            try
            {
                FullBurial.Bodyanalysischart = context.Bodyanalysischart
                    .Where(
                        x => x.Northsouth == FullBurial.Burialmain.Northsouth & 
                        x.Eastwest == FullBurial.Burialmain.Eastwest & 
                        x.Squareeastwest == FullBurial.Burialmain.Squareeastwest &
                        x.Squarenorthsouth == FullBurial.Burialmain.Squarenorthsouth)
                    .FirstOrDefault();
            }
            catch
            {
                FullBurial.Bodyanalysischart = new Bodyanalysischart();
            }


            return View(FullBurial);
        }

        [Authorize]
        public IActionResult AddData()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddData(Burialmain newEntry)
        {
            //make sure its valid
            if (ModelState.IsValid)
            {
                newEntry.Id = context.Burialmain.OrderBy(x => x.Id).Last().Id + 1;
                context.Add(newEntry);
                context.SaveChanges();
                return View("Confirmation");
            }
            else
            {
                return View(newEntry);
            }

        }

        

    }
}
