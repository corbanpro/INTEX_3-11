using System;

namespace INTEX_3_11.Models.ViewModels
{
    public class PageInfo
    {
        public int TotalNumBurials { get; set; }
        public int BurialsPerPage { get; set; }
        public int CurrentPage { get; set; }

        //Figure out how many pages we need
        public int TotalPages => (int)Math.Ceiling((double)TotalNumBurials / BurialsPerPage);
    }
}
