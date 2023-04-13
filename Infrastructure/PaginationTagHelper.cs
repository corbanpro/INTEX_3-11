using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using INTEX_3_11.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX_3_11.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PaginationTagHelper : TagHelper
    {
        //Dynamically create the page links for us

        private IUrlHelperFactory uhf;

        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            uhf = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext vc { get; set; }
        public PageModel PageModel { get; set; }
        public string PageAction { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext thc, TagHelperOutput tho)
        {
            IUrlHelper uh = uhf.GetUrlHelper(vc);

            TagBuilder final = new TagBuilder("div");

            for (int i = 1; i <= PageModel.PageInfo.TotalPages; i++)
            {
                TagBuilder tb = new TagBuilder("a");


                tb.Attributes["href"] = uh.Action(PageAction, new { pageNum = i, ageAtDeath = PageModel.Filter.ageAtDeath, sex = PageModel.Filter.sex, depth = PageModel.Filter.depth, Headdirection = PageModel.Filter.Headdirection, haircolor = PageModel.Filter.haircolor});
                if (PageClassesEnabled)                                           
                {                                                                
                    tb.AddCssClass(PageClass);                                   
                    tb.AddCssClass(i == PageModel.PageInfo.CurrentPage                    
                        ? PageClassSelected : PageClassNormal);
                }
                tb.InnerHtml.Append(i.ToString());

                final.InnerHtml.AppendHtml(tb);
            }

            tho.Content.AppendHtml(final.InnerHtml);

        }

    }
}