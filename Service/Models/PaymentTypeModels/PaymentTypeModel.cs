﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.PaymentTypeModels
{
    public class PaymentTypeModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }
}
