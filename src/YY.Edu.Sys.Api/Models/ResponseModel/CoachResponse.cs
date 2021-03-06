﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YY.Edu.Sys.Api.Models.ResponseModel
{
    public class CoachResponse : Sys.Models.Coach
    {

        public string VenueName { get; set; }

        public decimal? Wage { get; set; }

        public decimal? Price { get; set; }

        public decimal? WageMore { get; set; }

        public decimal? PriceMore { get; set; }

        public int ApplyCoachID { get; set; }

    }
}