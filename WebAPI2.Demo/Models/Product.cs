﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Demo.Models
{
    public class Product
    {
        public string Id;
        public string ProductName;
        public Category Category;
        public double Price;
    }

    public class Category {

        public string Id;
        public string CategoryName;

    }
}