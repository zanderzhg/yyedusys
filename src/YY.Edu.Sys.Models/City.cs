﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace YY.Edu.Sys.Models
{
    public class City
    {

        private int cityID = 0;

        public int CityID
        {
            get { return cityID; }
            set { cityID = value; }
        }

        [Required(ErrorMessage = "城市名称不能为空")]
        public string CityName { get; set; }

    }
}