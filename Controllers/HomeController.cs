﻿using INTEX_3_11.Models;
using INTEX_3_11.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public IActionResult Delete (int id)
        {
            
            var burial = context.Burialmain.Single(x => x.Id == id);
            return View(burial);
        }

        [HttpPost]
        public IActionResult Delete (Burialmain ar)
        {
            context.Remove(ar);
            context.SaveChanges();
            return RedirectToAction("BurialList");
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var Editor = context.Burialmain.FirstOrDefault(x => x.Id == id);
            if (Editor == null)
            {
                return NotFound();
            }
            return View("AddData", Editor);
        }


        [HttpPost]
        public IActionResult EditData(int id)
        {
            var Editor = context.Burialmain.FirstOrDefault(x => x.Id == id);
            if (Editor == null)
            {
                return NotFound();
            }

            // Populate the fields of the Editor object
            Editor.Squarenorthsouth = Editor.Squarenorthsouth;
            Editor.Northsouth = Editor.Northsouth;
            Editor.Squareeastwest = Editor.Squareeastwest;
            Editor.Eastwest = Editor.Eastwest;
            Editor.Area = Editor.Area;
            Editor.Burialnumber = Editor.Burialnumber;
            Editor.Headdirection = Editor.Headdirection;
            Editor.Westtohead = Editor.Westtohead;
            Editor.Southtohead = Editor.Southtohead;
            Editor.Westtofeet = Editor.Westtofeet;
            Editor.Southtofeet = Editor.Southtofeet;
            Editor.Depth = Editor.Depth;
            Editor.Sex = Editor.Sex;
            Editor.Ageatdeath = Editor.Ageatdeath;
            Editor.Wrapping = Editor.Wrapping;
            Editor.Facebundles = Editor.Facebundles;
            Editor.Preservation = Editor.Preservation;
            Editor.Haircolor = Editor.Haircolor;
            Editor.Text = Editor.Text;
            Editor.Fieldbookpage = Editor.Fieldbookpage;


            return View("AddData", Editor);
        }

    }
}

