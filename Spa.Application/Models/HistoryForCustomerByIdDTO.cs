﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class HistoryForCustomerByIdDTO
    {
        public string CustomerName { get; set; }
        public string PhotoCustomer {  get; set; }

        public List<long> ServiceUsed {  get; set; }
        public DateTime Date { get; set; }
    }
}
