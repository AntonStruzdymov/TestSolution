﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDB.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid Value { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
