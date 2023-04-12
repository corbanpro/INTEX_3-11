using System.Security.Cryptography.Xml;

namespace INTEX_3_11.Models.ViewModels
{
    public class SupervisedPredictionJson
    {
        public double depth { get; set; }
        public double southtohead { get; set; }
        public double westtohead { get; set; }
        public double length { get; set; }
        public double westtofeet { get; set; }
        public double southtofeet { get; set; }
        public double FemurHeadDiameter { get; set; }
        public double HumerusLength { get; set; }

        public double adultsubadult_C { get; set; } = 0.0;
        public double adultsubadult_NLL { get; set; } = 0.0;
        
        public double haircolor_B { get; set; } = 0.0;
        public double haircolor_D { get; set; } = 0.0;
        public double haircolor_K { get; set; } = 0.0;
        public double haircolor_R { get; set; } = 0.0;
        public double haircolor_U { get; set; } = 0.0;

        public double textilefunction_value_Fragmentary { get; set; } = 0.0;
        public double textilefunction_value_Other { get; set; } = 0.0;
        public double textilefunction_value_Ribbon { get; set; } = 0.0;
        public double textilefunction_value_Tunic { get; set; } = 0.0;
    }
}
