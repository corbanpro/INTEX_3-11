using INTEX_3_11.Models;
using INTEX_3_11.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using System.Text;

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
            int pageSize = 40;

            PageModel PageModel = new PageModel()
            {
                BurialList = context.Burialmain
                .Where(x => x.Ageatdeath == ageAtDeath || ageAtDeath == null)
                .Where(x => x.Sex == sex || sex == null)
                .Where(x => x.Depth == depth || depth == null)
                .Where(x => x.Headdirection == Headdirection || Headdirection == null)
                .Where(x => x.Haircolor == haircolor || haircolor == null)
                .Where(x => x.Id == 19140298416325764)
                .OrderBy(x => x.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

                PageInfo = new PageInfo() {
                    TotalNumBurials = context.Burialmain
                        .Where(x => x.Ageatdeath == ageAtDeath || ageAtDeath == null)
                        .Where(x => x.Sex == sex || sex == null)
                        .Where(x => x.Depth == depth || depth == null)
                        .Where(x => x.Headdirection == Headdirection || Headdirection == null)
                        .Where(x => x.Haircolor == haircolor || haircolor == null)
                        .Count(),
                    BurialsPerPage = pageSize,
                    CurrentPage = pageNum
                },

                Filter = new Filter()
                {
                    ageAtDeath = ageAtDeath,
                    sex = sex,
                    depth = depth,
                    Headdirection = Headdirection,
                    haircolor = haircolor
                }
            };

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

            try
            {
                FullBurial.BurialmainTextile = context.BurialmainTextile.Where(x => x.MainBurialmainid == id).FirstOrDefault();
                FullBurial.Textile = context.Textile.Where(x => x.Id == FullBurial.BurialmainTextile.MainTextileid).FirstOrDefault();
            }
            catch
            {
                FullBurial.BurialmainTextile = new BurialmainTextile();
                FullBurial.Textile = new Textile();
            }

            try
            {
                FullBurial.TextilefunctionTextile = context.TextilefunctionTextile.Where(x => x.MainTextileid == FullBurial.Textile.Id).FirstOrDefault();
                FullBurial.Textilefunction = context.Textilefunction.Where(x => x.Id == FullBurial.TextilefunctionTextile.MainTextilefunctionid).FirstOrDefault();
            }
            catch
            {
                FullBurial.TextilefunctionTextile = new TextilefunctionTextile();
                FullBurial.Textilefunction = new Textilefunction();
            }

            try
            {
                FullBurial.ColorTextile = context.ColorTextile.Where(x => x.MainColorid == FullBurial.Textile.Id).FirstOrDefault();
                FullBurial.Color = context.Color.Where(x => x.Colorid == FullBurial.ColorTextile.MainColorid).FirstOrDefault();
            }
            catch
            {
                FullBurial.ColorTextile = new ColorTextile();
                FullBurial.Color = new Color();
            }

            try
            {
                FullBurial.StructureTextile = context.StructureTextile.Where(x => x.MainTextileid == FullBurial.Textile.Id).FirstOrDefault();
                FullBurial.Structure = context.Structure.Where(x => x.Structureid == FullBurial.StructureTextile.MainStructureid).FirstOrDefault();
            }
            catch
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



        /*
                    
        */

        [Authorize]
        [HttpGet]
        public IActionResult AddData()
        {
            return View();
        }

        [Authorize]
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
        public IActionResult SupervisedAnalysis() 
        {
            ViewData["Prediction"] = null;
            return View();
        }

        [HttpPost]
        public IActionResult SupervisedAnalysis(SupervisedPrediction sp)
        {


            SupervisedPredictionJson spj = new SupervisedPredictionJson();

            spj.depth = sp.depth;
            spj.southtohead = sp.southtohead;
            spj.westtohead = sp.westtohead;
            spj.length = sp.length;
            spj.westtofeet = sp.westtofeet;
            spj.southtofeet = sp.southtofeet;
            spj.FemurHeadDiameter = sp.FemurHeadDiameter;
            spj.HumerusLength = sp.HumerusLength;
            
            if (sp.adultsubadult == "C")
            {
                spj.adultsubadult_C = 1.0;
            } else
            {
                spj.adultsubadult_NLL = 1.0;
            }

            if (sp.haircolor == "B")
            {
                spj.haircolor_B = 1.0;
            }
            else if (sp.haircolor == "K")
            {
                spj.haircolor_K = 1.0;
            }
            else if (sp.haircolor == "D")
            {
                spj.haircolor_D = 1.0;
            }
            else if (sp.haircolor == "R")
            {
                spj.haircolor_R = 1.0;
            }
            else if (sp.haircolor == "U")
            {
                spj.haircolor_U = 1.0;
            }

            if (sp.textilefunction_value == "Fragmentary")
            {
                spj.textilefunction_value_Fragmentary = 1.0;
            } 
            else if (sp.textilefunction_value == "Other")
            {
                spj.textilefunction_value_Other = 1.0;
            }
            else if (sp.textilefunction_value == "Ribbon")
            {
                spj.textilefunction_value_Ribbon = 1.0;
            }
            else if (sp.textilefunction_value == "Tunic")
            {
                spj.textilefunction_value_Tunic = 1.0;
            }

            String product = System.Text.Json.JsonSerializer.Serialize<SupervisedPredictionJson>(spj);
            product = product.Replace("adultsubadult_NLL", "adultsubadult_N LL");
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(product);
            var content = new FormUrlEncodedContent(values);
            string url = "http://18.221.127.150/predict";

            WebClient webClient = new WebClient();
            byte[] resByte;
            string resString;
            byte[] reqString;


            webClient.Headers["content-type"] = "application/json";
            reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(values, Formatting.Indented));
            resByte = webClient.UploadData(url, "post", reqString);
            resString = Encoding.Default.GetString(resByte);
            webClient.Dispose();

            resString = resString.Replace("{\"prediction\":\"", "");
            resString = resString.Replace("\"}", "");

            if (resString == "W")
            {
                resString = "Full or Nearly Full Wrapping Remains (W)";
            }
            else if (resString == "H")
            {
                resString = "Partial Wrapping Remains (H)";
            }
            else if (resString == "B")
            {
                resString = "Bones and/or Only Partial Remains of Wrapping Remains (B)";
            }
            else
            {
                resString = "Unknown (U)";
            }
            ViewData["Prediction"] = resString;


            return View();           
            
        }

        [HttpGet]
        public IActionResult UnsupervisedAnalysis()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete (long id)
        {
            
            var burial = context.Burialmain.Single(x => x.Id == id);
            return View(burial);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete (Burialmain ar)
        {
            context.Remove(ar);
            context.SaveChanges();
            return RedirectToAction("BurialList");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(long id)
        {
            var EditBurial = context.Burialmain.FirstOrDefault(x => x.Id == id);
            if (EditBurial == null)
            {
                return NotFound();
            }
            return View(EditBurial);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Burialmain EditEntry)
        {
            var Editor = context.Burialmain.FirstOrDefault(x => x.Id == EditEntry.Id);
            if (Editor == null)
            {
                return NotFound();
            }

            // Populate the Editor object
            Editor.Squarenorthsouth = EditEntry.Squarenorthsouth;
            Editor.Northsouth = EditEntry.Northsouth;
            Editor.Squareeastwest = EditEntry.Squareeastwest;
            Editor.Eastwest = EditEntry.Eastwest;
            Editor.Area = EditEntry.Area;
            Editor.Burialnumber = EditEntry.Burialnumber;
            Editor.Headdirection = EditEntry.Headdirection;
            Editor.Westtohead = EditEntry.Westtohead;
            Editor.Southtohead = EditEntry.Southtohead;
            Editor.Westtofeet = EditEntry.Westtofeet;
            Editor.Southtofeet = EditEntry.Southtofeet;
            Editor.Depth = EditEntry.Depth;
            Editor.Sex = EditEntry.Sex;
            Editor.Ageatdeath = EditEntry.Ageatdeath;
            Editor.Wrapping = EditEntry.Wrapping;
            Editor.Facebundles = EditEntry.Facebundles;
            Editor.Preservation = EditEntry.Preservation;
            Editor.Haircolor = EditEntry.Haircolor;
            Editor.Text = EditEntry.Text;
            Editor.Fieldbookpage = EditEntry.Fieldbookpage;

            context.Burialmain.Update(Editor);
            context.SaveChanges();
           

            return RedirectToAction("BurialList");
        }

    }
}

