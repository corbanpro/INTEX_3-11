using System.ComponentModel.DataAnnotations;
using System;

namespace INTEX_3_11.Models
{
    public class SupervisedPrediction
    {

        public double Depth { get; set; } 
        public double Adultsubadult { get; set; }
        public double Southtohead { get; set; } 
        public string Haircolor { get; set; } 
        public double Westtohead { get; set; }
        public double Length { get; set; } 
        public double Westtofeet { get; set; } 
        public double Southtofeet { get; set; } 
        public double femurHeadDiameter { get; set; }
        public double HumerusLength { get; set; }
        public double TextileFunciton { get; set; }

    }
}
