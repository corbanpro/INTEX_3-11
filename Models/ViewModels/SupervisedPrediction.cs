using System.ComponentModel.DataAnnotations;
using System;

namespace INTEX_3_11.Models.ViewModels
{
    public class SupervisedPrediction
    {

        public double depth { get; set; }
        public double southtohead { get; set; }
        public double westtohead { get; set; }
        public double length { get; set; }
        public double westtofeet { get; set; }
        public double southtofeet { get; set; }
        public double FemurHeadDiameter { get; set; }
        public double HumerusLength { get; set; }
        public string adultsubadult { get; set; }
        public string haircolor { get; set; }
        public string textilefunction_value { get; set; }

    }
}
