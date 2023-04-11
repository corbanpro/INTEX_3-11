using System.Linq;

namespace INTEX_3_11.Models
{
    public class FullBurial
    {
        public Burialmain Burialmain { get; set; }
        public BurialmainTextile? BurialmainTextile { get; set; }
        public Textile Textile { get; set; }
        public Textilefunction Textilefunction { get; set; }
        public ColorTextile ColorTextile { get; set; }
        public Color Color { get; set; }
        public StructureTextile StructureTextile { get; set; }
        public Structure Structure { get; set; }
    }
}
