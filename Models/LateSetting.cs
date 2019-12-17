using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyMVCAssignment.Models
{
    public class LateSetting
    {
        public int Id { get; set; }
        [Range(0, Double.PositiveInfinity)]
        public double MoneyPerLate { get; set; }

        [Range(0, Double.PositiveInfinity)]
        public int PushPerLate { get; set; }

        public int SecondRate { get; set; }
        public int ThirdRate { get; set; }

    }
}