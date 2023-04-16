using System.Collections.Generic;

namespace INTEX_3_11.Models.ViewModels
{
    public class PageModel
    {
        public List<Burialmain> BurialList {get; set;}
        public PageInfo PageInfo { get; set;}
        public Filter Filter { get; set;}
    }
}
