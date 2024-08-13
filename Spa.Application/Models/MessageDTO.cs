﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class MessageDTO
    {
        public int? MessageId { get; set; }

        public string? UserName { get; set; }

        public string? Content { get; set; }

        public DateTime? MessageTime { get; set; }
    }
}
